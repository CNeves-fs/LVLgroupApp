using System.Collections.Generic;

namespace Core.Constants
{
    public static class Permissions
    {

        //---------------------------------------------------------------------------------------------------


        public static List<string> GenerateAllPermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }


        //---------------------------------------------------------------------------------------------------


        public static List<string> GenerateEditPermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit"
            };
        }


        //---------------------------------------------------------------------------------------------------


        public static List<string> GenerateViewPermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View"
            };
        }


        //---------------------------------------------------------------------------------------------------


        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class AuditLogs
        {
            public const string View = "Permissions.AuditLogs.View";
            public const string Create = "Permissions.AuditLogs.Create";
            public const string Edit = "Permissions.AuditLogs.Edit";
            public const string Delete = "Permissions.AuditLogs.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Notifications
        {
            public const string View = "Permissions.Notifications.View";
            public const string Create = "Permissions.Notifications.Create";
            public const string Edit = "Permissions.Notifications.Edit";
            public const string Delete = "Permissions.Notifications.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Clientes
        {
            public const string View = "Permissions.Clientes.View";
            public const string Create = "Permissions.Clientes.Create";
            public const string Edit = "Permissions.Clientes.Edit";
            public const string Delete = "Permissions.Clientes.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Lojas
        {
            public const string View = "Permissions.Lojas.View";
            public const string Create = "Permissions.Lojas.Create";
            public const string Edit = "Permissions.Lojas.Edit";
            public const string Delete = "Permissions.Lojas.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class TiposOcorrencias
        {
            public const string View = "Permissions.TiposOcorrencias.View";
            public const string Create = "Permissions.TiposOcorrencias.Create";
            public const string Edit = "Permissions.TiposOcorrencias.Edit";
            public const string Delete = "Permissions.TiposOcorrencias.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Ocorrencias
        {
            public const string View = "Permissions.Ocorrencias.View";
            public const string Create = "Permissions.Ocorrencias.Create";
            public const string Edit = "Permissions.Ocorrencias.Edit";
            public const string Delete = "Permissions.Ocorrencias.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Gruposlojas
        {
            public const string View = "Permissions.Gruposlojas.View";
            public const string Create = "Permissions.Gruposlojas.Create";
            public const string Edit = "Permissions.Gruposlojas.Edit";
            public const string Delete = "Permissions.Gruposlojas.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Gruposupervisores
        {
            public const string View = "Permissions.Gruposupervisores.View";
            public const string Create = "Permissions.Gruposupervisores.Create";
            public const string Edit = "Permissions.Gruposupervisores.Edit";
            public const string Delete = "Permissions.Gruposupervisores.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Empresas
        {
            public const string View = "Permissions.Empresas.View";
            public const string Create = "Permissions.Empresas.Create";
            public const string Edit = "Permissions.Empresas.Edit";
            public const string Delete = "Permissions.Empresas.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Mercados
        {
            public const string View = "Permissions.Mercados.View";
            public const string Create = "Permissions.Mercados.Create";
            public const string Edit = "Permissions.Mercados.Edit";
            public const string Delete = "Permissions.Mercados.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Vendas
        {
            public const string View = "Permissions.Vendas.View";
            public const string Create = "Permissions.Vendas.Create";
            public const string Edit = "Permissions.Vendas.Edit";
            public const string Delete = "Permissions.Vendas.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Claims
        {
            public const string View = "Permissions.Claims.View";
            public const string Create = "Permissions.Claims.Create";
            public const string Edit = "Permissions.Claims.Edit";
            public const string Delete = "Permissions.Claims.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Fotos
        {
            public const string View = "Permissions.Fotos.View";
            public const string Create = "Permissions.Fotos.Create";
            public const string Edit = "Permissions.Fotos.Edit";
            public const string Delete = "Permissions.Fotos.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Fototags
        {
            public const string View = "Permissions.Fototags.View";
            public const string Create = "Permissions.Fototags.Create";
            public const string Edit = "Permissions.Fototags.Edit";
            public const string Delete = "Permissions.Fototags.Delete";
        }

        //---------------------------------------------------------------------------------------------------


        public static class Statuss
        {
            public const string View = "Permissions.Statuss.View";
            public const string Create = "Permissions.Statuss.Create";
            public const string Edit = "Permissions.Statuss.Edit";
            public const string Delete = "Permissions.Statuss.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Prazoslimite
        {
            public const string View = "Permissions.Prazoslimite.View";
            public const string Create = "Permissions.Prazoslimite.Create";
            public const string Edit = "Permissions.Prazoslimite.Edit";
            public const string Delete = "Permissions.Prazoslimite.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Pareceres
        {
            public const string View = "Permissions.Pareceres.View";
            public const string Create = "Permissions.Pareceres.Create";
            public const string Edit = "Permissions.Pareceres.Edit";
            public const string Delete = "Permissions.Pareceres.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Artigos
        {
            public const string View = "Permissions.Artigos.View";
            public const string Create = "Permissions.Artigos.Create";
            public const string Edit = "Permissions.Artigos.Edit";
            public const string Delete = "Permissions.Artigos.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Genders
        {
            public const string View = "Permissions.Genders.View";
            public const string Create = "Permissions.Genders.Create";
            public const string Edit = "Permissions.Genders.Edit";
            public const string Delete = "Permissions.Genders.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class QuestionTemplate
        {
            public const string View = "Permissions.QuestionTemplate.View";
            public const string Create = "Permissions.QuestionTemplate.Create";
            public const string Edit = "Permissions.QuestionTemplate.Edit";
            public const string Delete = "Permissions.QuestionTemplate.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class ReportTemplate
        {
            public const string View = "Permissions.ReportTemplate.View";
            public const string Create = "Permissions.ReportTemplate.Create";
            public const string Edit = "Permissions.ReportTemplate.Edit";
            public const string Delete = "Permissions.ReportTemplate.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class ReportType
        {
            public const string View = "Permissions.ReportType.View";
            public const string Create = "Permissions.ReportType.Create";
            public const string Edit = "Permissions.ReportType.Edit";
            public const string Delete = "Permissions.ReportType.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Report
        {
            public const string View = "Permissions.Report.View";
            public const string Create = "Permissions.Report.Create";
            public const string Edit = "Permissions.Report.Edit";
            public const string Delete = "Permissions.Report.Delete";
        }


        //---------------------------------------------------------------------------------------------------


        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }


        //---------------------------------------------------------------------------------------------------

    }
}