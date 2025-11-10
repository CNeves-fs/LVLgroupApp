using Core.Abstractions;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Entities.Clientes;
using Core.Entities.Hub;
using Core.Entities.Identity;
using Core.Entities.Logs;
using Core.Entities.Notifications;
using Core.Entities.Ocorrencias;
using Core.Entities.Reports;
using Core.Entities.Vendas;
using Core.Enums;
using Core.Interfaces.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.DbContext
{
    public class LVLgroupDbContext : IdentityDbContext<ApplicationUser>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDateTimeService _dateTime;

        private readonly IAuthenticatedUserService _authenticatedUser;


        //---------------------------------------------------------------------------------------------------


        public DbSet<Audit> AuditLogs { get; set; }





        public DbSet<Cliente> Clientes { get; set; }





        public DbSet<Artigo> Artigos { get; set; }

        public DbSet<Gender> Genders { get; set; }





        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Grupoloja> Gruposlojas { get; set; }

        public DbSet<Loja> Lojas { get; set; }

        public DbSet<Mercado> Mercados { get; set; }





        public DbSet<VendaDiaria> VendasDiarias { get; set; }

        public DbSet<VendaSemanal> VendasSemanais { get; set; }





        public DbSet<Claim> Claims { get; set; }

        public DbSet<Foto> Fotos { get; set; }

        public DbSet<Fototag> Fototags { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Prazolimite> Prazoslimite { get; set; }

        public DbSet<Parecer> Pareceres { get; set; }





        public DbSet<Ocorrencia> Ocorrencias { get; set; }

        public DbSet<TipoOcorrencia> TiposOcorrencias { get; set; }

        public DbSet<TipoOcorrenciaLocalized> TiposOcorrenciasLocaslized { get; set; }

        public DbSet<OcorrenciaDocument> OcorrenciasDocuments { get; set; }

        public DbSet<NotificacaoOcorrencia> NotificacoesOcorrencias { get; set; }





        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<QuestionTemplate> QuestionTemplates { get; set; }

        public DbSet<QuestionTemplateLocalized> QuestionTemplatesLocalized { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<ReportType> ReportTypes { get; set; }

        public DbSet<ReportTypeLocalized> ReportTypesLocalized { get; set; }

        public DbSet<ReportTemplateQuestion> ReportTemplateQuestions { get; set; }

        public DbSet<ReportTemplate> ReportTemplates { get; set; }

        public DbSet<ReportAnswer> ReportAnswers { get; set; }

        public DbSet<ReportAnswerAttachment> ReportAnswerAttachments { get; set; }







        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationSended> NotificationsSended { get; set; }

        public DbSet<HubConnection> HubConnections { get; set; }


        //---------------------------------------------------------------------------------------------------


        public LVLgroupDbContext(DbContextOptions<LVLgroupDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }


        //---------------------------------------------------------------------------------------------------


        public IDbConnection Connection => Database.GetDbConnection();


        //---------------------------------------------------------------------------------------------------


        public bool HasChanges => ChangeTracker.HasChanges();


        //---------------------------------------------------------------------------------------------------


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationSended>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<VendaDiaria>()
                .ToTable(tb => tb.UseSqlOutputClause(false));
            modelBuilder.Entity<VendaSemanal>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            base.OnModelCreating(modelBuilder);
        }


        //---------------------------------------------------------------------------------------------------


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            if (_authenticatedUser.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await AuditSaveChangesAsync(_authenticatedUser.UserEmail, _authenticatedUser.UserId);
            }
        }


        //---------------------------------------------------------------------------------------------------


        private async Task<int> AuditSaveChangesAsync(string email = null, string userId = null)
        {
            List<AuditEntry> auditEntries = OnBeforeSaveChanges(userId, email);
            int result = await base.SaveChangesAsync();
            await OnAfterSaveChanges(auditEntries);
            return result;
        }


        //---------------------------------------------------------------------------------------------------


        private List<AuditEntry> OnBeforeSaveChanges(string userId, string email)
        {
            ChangeTracker.DetectChanges();
            List<AuditEntry> list = new List<AuditEntry>();
            foreach (EntityEntry item in ChangeTracker.Entries())
            {
                if (item.Entity is Audit || item.State == EntityState.Detached || item.State == EntityState.Unchanged)
                {
                    continue;
                }

                AuditEntry auditEntry = new AuditEntry(item);
                auditEntry.TableName = item.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntry.Email = email;
                list.Add(auditEntry);
                foreach (PropertyEntry property in item.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string name = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[name] = property.CurrentValue;
                        continue;
                    }

                    switch (item.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[name] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[name] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(name);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[name] = property.OriginalValue;
                                auditEntry.NewValues[name] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (AuditEntry item2 in list.Where((AuditEntry _) => !_.HasTemporaryProperties))
            {
                //AuditLogs.Add(item2.ToAudit());
            }

            return list.Where((AuditEntry _) => _.HasTemporaryProperties).ToList();
        }


        //---------------------------------------------------------------------------------------------------


        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (AuditEntry auditEntry in auditEntries)
            {
                foreach (PropertyEntry temporaryProperty in auditEntry.TemporaryProperties)
                {
                    if (temporaryProperty.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[temporaryProperty.Metadata.Name] = temporaryProperty.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[temporaryProperty.Metadata.Name] = temporaryProperty.CurrentValue;
                    }
                }

                //AuditLogs.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }


        //---------------------------------------------------------------------------------------------------

    }
}
