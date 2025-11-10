using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LVL_SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Lojas",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "MercadoId",
                table: "Lojas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MercadoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HubConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubConnections_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mercados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCurto = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mercados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TiposOcorrencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposOcorrencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendasSemanais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicialDaSemana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinalDaSemana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDaSemana = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    ValorTotalDaVenda = table.Column<double>(type: "float", nullable: false),
                    ValorTotalDaVendaDoAnoAnterior = table.Column<double>(type: "float", nullable: false),
                    ObjetivoDaVendaSemanal = table.Column<double>(type: "float", nullable: false),
                    VariaçaoAnual = table.Column<double>(type: "float", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    GrupolojaId = table.Column<int>(type: "int", nullable: false),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    MercadoId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasSemanais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendasSemanais_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendasSemanais_Gruposlojas_GrupolojaId",
                        column: x => x.GrupolojaId,
                        principalTable: "Gruposlojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendasSemanais_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendasSemanais_Mercados_MercadoId",
                        column: x => x.MercadoId,
                        principalTable: "Mercados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NotificationsSended",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsSended", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationsSended_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationsSended_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificacoesOcorrencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoOcorrenciaId = table.Column<int>(type: "int", nullable: false),
                    TipoDestino = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicationUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacoesOcorrencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificacoesOcorrencias_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificacoesOcorrencias_TiposOcorrencias_TipoOcorrenciaId",
                        column: x => x.TipoOcorrenciaId,
                        principalTable: "TiposOcorrencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocorrencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DataOcorrencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailAutor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEntradaSistemaOcorrencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    TipoOcorrenciaId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    OcorrenciaNome = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Descrição = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Comentário = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TotalFicheiros = table.Column<int>(type: "int", nullable: false),
                    OcorrenciaFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    GrupolojaId = table.Column<int>(type: "int", nullable: false),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    MercadoId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocorrencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ocorrencias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Ocorrencias_Gruposlojas_GrupolojaId",
                        column: x => x.GrupolojaId,
                        principalTable: "Gruposlojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Ocorrencias_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ocorrencias_Mercados_MercadoId",
                        column: x => x.MercadoId,
                        principalTable: "Mercados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Ocorrencias_TiposOcorrencias_TipoOcorrenciaId",
                        column: x => x.TipoOcorrenciaId,
                        principalTable: "TiposOcorrencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TiposOcorrenciasLocaslized",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoOcorrenciaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposOcorrenciasLocaslized", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposOcorrenciasLocaslized_TiposOcorrencias_TipoOcorrenciaId",
                        column: x => x.TipoOcorrenciaId,
                        principalTable: "TiposOcorrencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendasDiarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendaSemanalId = table.Column<int>(type: "int", nullable: false),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mês = table.Column<int>(type: "int", nullable: false),
                    DiaDoMês = table.Column<int>(type: "int", nullable: false),
                    DiaDaSemana = table.Column<int>(type: "int", nullable: false),
                    DataDaVenda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorDaVenda = table.Column<double>(type: "float", nullable: false),
                    TotalArtigos = table.Column<int>(type: "int", nullable: false),
                    Weather = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasDiarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendasDiarias_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendasDiarias_VendasSemanais_VendaSemanalId",
                        column: x => x.VendaSemanalId,
                        principalTable: "VendasSemanais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OcorrenciasDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OcorrenciaId = table.Column<int>(type: "int", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OcorrenciaFolder = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Descrição = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcorrenciasDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcorrenciasDocuments_Ocorrencias_OcorrenciaId",
                        column: x => x.OcorrenciaId,
                        principalTable: "Ocorrencias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lojas_MercadoId",
                table: "Lojas",
                column: "MercadoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MercadoId",
                table: "AspNetUsers",
                column: "MercadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HubConnections_ApplicationUserId",
                table: "HubConnections",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacoesOcorrencias_ApplicationUserId",
                table: "NotificacoesOcorrencias",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacoesOcorrencias_TipoOcorrenciaId",
                table: "NotificacoesOcorrencias",
                column: "TipoOcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserId",
                table: "Notifications",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsSended_NotificationId",
                table: "NotificationsSended",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsSended_ToUserId",
                table: "NotificationsSended",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_EmpresaId",
                table: "Ocorrencias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_GrupolojaId",
                table: "Ocorrencias",
                column: "GrupolojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_LojaId",
                table: "Ocorrencias",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_MercadoId",
                table: "Ocorrencias",
                column: "MercadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_TipoOcorrenciaId",
                table: "Ocorrencias",
                column: "TipoOcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_OcorrenciasDocuments_OcorrenciaId",
                table: "OcorrenciasDocuments",
                column: "OcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposOcorrenciasLocaslized_TipoOcorrenciaId",
                table: "TiposOcorrenciasLocaslized",
                column: "TipoOcorrenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasDiarias_LojaId_Ano_Mês_DiaDoMês",
                table: "VendasDiarias",
                columns: new[] { "LojaId", "Ano", "Mês", "DiaDoMês" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendasDiarias_VendaSemanalId_DiaDaSemana",
                table: "VendasDiarias",
                columns: new[] { "VendaSemanalId", "DiaDaSemana" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendasSemanais_EmpresaId",
                table: "VendasSemanais",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasSemanais_GrupolojaId",
                table: "VendasSemanais",
                column: "GrupolojaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasSemanais_LojaId_Ano_NumeroDaSemana",
                table: "VendasSemanais",
                columns: new[] { "LojaId", "Ano", "NumeroDaSemana" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendasSemanais_MercadoId",
                table: "VendasSemanais",
                column: "MercadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Mercados_MercadoId",
                table: "AspNetUsers",
                column: "MercadoId",
                principalTable: "Mercados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lojas_Mercados_MercadoId",
                table: "Lojas",
                column: "MercadoId",
                principalTable: "Mercados",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Mercados_MercadoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lojas_Mercados_MercadoId",
                table: "Lojas");

            migrationBuilder.DropTable(
                name: "HubConnections");

            migrationBuilder.DropTable(
                name: "NotificacoesOcorrencias");

            migrationBuilder.DropTable(
                name: "NotificationsSended");

            migrationBuilder.DropTable(
                name: "OcorrenciasDocuments");

            migrationBuilder.DropTable(
                name: "TiposOcorrenciasLocaslized");

            migrationBuilder.DropTable(
                name: "VendasDiarias");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Ocorrencias");

            migrationBuilder.DropTable(
                name: "VendasSemanais");

            migrationBuilder.DropTable(
                name: "TiposOcorrencias");

            migrationBuilder.DropTable(
                name: "Mercados");

            migrationBuilder.DropIndex(
                name: "IX_Lojas_MercadoId",
                table: "Lojas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MercadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Lojas");

            migrationBuilder.DropColumn(
                name: "MercadoId",
                table: "Lojas");

            migrationBuilder.DropColumn(
                name: "Local",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MercadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "AspNetUsers");
        }
    }
}
