using Core.Interfaces.CacheRepositories;
using Core.Interfaces.ChartCacheRepositories;
using Core.Interfaces.Repositories;
using Core.Interfaces.Shared;
using Infrastructure.CacheRepositories;
using Infrastructure.ChartCacheRepositories;
using Infrastructure.Data.DbContext;
using Infrastructure.Hubs;
using Infrastructure.Repositories;
using Infrastructure.Shared.Services;
using LVLgroupApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        //---------------------------------------------------------------------------------------------------


        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<LVLgroupDbContext, LVLgroupDbContext>();
            services.AddScoped<NotificationHub>();
            services.AddSingleton<SubscribeNotificationTableDependency>();
        }


        //---------------------------------------------------------------------------------------------------


        public static void AddMailService(this IServiceCollection services)
        {
            services.AddTransient<IMailService, SMTPMailService>();
        }


        //---------------------------------------------------------------------------------------------------


        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteCacheRepository, ClienteCacheRepository>();

            services.AddTransient<IClaimRepository, ClaimRepository>();
            services.AddTransient<IClaimCacheRepository, ClaimCacheRepository>();

            services.AddTransient<IFotoRepository, FotoRepository>();
            services.AddTransient<IFotoCacheRepository, FotoCacheRepository>();

            services.AddTransient<IFototagRepository, FototagRepository>();
            services.AddTransient<IFototagCacheRepository, FototagCacheRepository>();

            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<IStatusCacheRepository, StatusCacheRepository>();

            services.AddTransient<IPrazolimiteRepository, PrazolimiteRepository>();
            services.AddTransient<IPrazolimiteCacheRepository, PrazolimiteCacheRepository>();

            services.AddTransient<IParecerRepository, ParecerRepository>();
            services.AddTransient<IParecerCacheRepository, ParecerCacheRepository>();

            services.AddTransient<ILojaRepository, LojaRepository>();
            services.AddTransient<ILojaCacheRepository, LojaCacheRepository>();

            services.AddTransient<IOcorrenciaRepository, OcorrenciaRepository>();
            services.AddTransient<IOcorrenciaCacheRepository, OcorrenciaCacheRepository>();
            services.AddTransient<ITipoOcorrenciaRepository, TipoOcorrenciaRepository>();
            services.AddTransient<ITipoOcorrenciaCacheRepository, TipoOcorrenciaCacheRepository>();
            services.AddTransient<ITipoOcorrenciaLocalizedRepository, TipoOcorrenciaLocalizedRepository>();
            services.AddTransient<ITipoOcorrenciaLocalizedCacheRepository, TipoOcorrenciaLocalizedCacheRepository>();
            services.AddTransient<IOcorrenciaDocumentRepository, OcorrenciaDocumentRepository>();
            services.AddTransient<IOcorrenciaDocumentCacheRepository, OcorrenciaDocumentCacheRepository>();
            services.AddTransient<INotificacaoOcorrenciaRepository, NotificacaoOcorrenciaRepository>();
            services.AddTransient<INotificacaoOcorrenciaCacheRepository, NotificacaoOcorrenciaCacheRepository>();

            services.AddTransient<IGrupolojaRepository, GrupolojaRepository>();
            services.AddTransient<IGrupolojaCacheRepository, GrupolojaCacheRepository>();

            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<IEmpresaCacheRepository, EmpresaCacheRepository>();

            services.AddTransient<IMercadoRepository, MercadoRepository>();
            services.AddTransient<IMercadoCacheRepository, MercadoCacheRepository>();

            services.AddTransient<IMercadoRepository, MercadoRepository>();
            services.AddTransient<IMercadoCacheRepository, MercadoCacheRepository>();

            services.AddTransient<IVendaDiariaRepository, VendaDiariaRepository>();
            services.AddTransient<IVendaDiariaCacheRepository, VendaDiariaCacheRepository>();

            services.AddTransient<IVendaSemanalRepository, VendaSemanalRepository>();
            services.AddTransient<IVendaSemanalCacheRepository, VendaSemanalCacheRepository>();

            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationCacheRepository, NotificationCacheRepository>();

            services.AddTransient<INotificationSendedRepository, NotificationSendedRepository>();
            services.AddTransient<INotificationSendedCacheRepository, NotificationSendedCacheRepository>();

            services.AddTransient<IArtigoRepository, ArtigoRepository>();
            services.AddTransient<IArtigoCacheRepository, ArtigoCacheRepository>();

            services.AddTransient<IGenderRepository, GenderRepository>();
            services.AddTransient<IGenderCacheRepository, GenderCacheRepository>();

            services.AddTransient<IArtigoChartCacheRepository, ArtigoChartCacheRepository>();
            services.AddTransient<IClaimChartCacheRepository, ClaimChartCacheRepository>();
            services.AddTransient<IClienteChartCacheRepository, ClienteChartCacheRepository>();
            services.AddTransient<ILojaChartCacheRepository, LojaChartCacheRepository>();



            services.AddTransient<IQuestionOptionRepository, QuestionOptionRepository>();
            services.AddTransient<IQuestionOptionCacheRepository, QuestionOptionCacheRepository>();
            services.AddTransient<IQuestionTemplateRepository, QuestionTemplateRepository>();
            services.AddTransient<IQuestionTemplateCacheRepository, QuestionTemplateCacheRepository>();
            services.AddTransient<IQuestionTemplateLocalizedRepository, QuestionTemplateLocalizedRepository>();
            services.AddTransient<IQuestionTemplateLocalizedCacheRepository, QuestionTemplateLocalizedCacheRepository>();

            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportCacheRepository, ReportCacheRepository>();
            services.AddTransient<IReportTemplateRepository, ReportTemplateRepository>();
            services.AddTransient<IReportTemplateCacheRepository, ReportTemplateCacheRepository>();
            services.AddTransient<IReportTemplateQuestionRepository, ReportTemplateQuestionRepository>();
            services.AddTransient<IReportTemplateQuestionCacheRepository, ReportTemplateQuestionCacheRepository>();
            services.AddTransient<IReportTypeRepository, ReportTypeRepository>();
            services.AddTransient<IReportTypeCacheRepository, ReportTypeCacheRepository>();
            services.AddTransient<IReportTypeLocalizedRepository, ReportTypeLocalizedRepository>();
            services.AddTransient<IReportTypeLocalizedCacheRepository, ReportTypeLocalizedCacheRepository>();



            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }


        //---------------------------------------------------------------------------------------------------

    }
}