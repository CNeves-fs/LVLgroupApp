using Core.Entities.Identity;
using Core.Interfaces.Shared;
using Infrastructure.Data.DbContext;
using Infrastructure.Services;
using LVLgroupApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configurar o serviço de DateTime
builder.Services.AddTransient<IDateTimeService, SystemDateTimeService>();

// Configurar o serviço de User Authentication
builder.Services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();

// Configurar o DbContext com SQL Server
builder.Services.AddDbContext<LVLgroupDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LVLgroupDbConnection")));

// Configurar Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true; // Garante que o e-mail é único
    options.SignIn.RequireConfirmedEmail = true; // Exige e-mail confirmado
}).AddEntityFrameworkStores<LVLgroupDbContext>().AddDefaultUI().AddDefaultTokenProviders();

// Configurar o e-mail como identificador principal
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true; // Garante que o e-mail é único
});

// Configurar o serviço de multilingual
//builder.Services.AddMultiLingualSupport();

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Configurar middleware
//app.UseMultiLingualFeature();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
