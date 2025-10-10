using Core.Constants;
using Core.Entities.Claims;
using Infrastructure.Data.DbContext;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultStatus
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedDefaultStatus(LVLgroupDbContext db)
        {
            //Seed Default Status

            if (db.Status.Any()) return;


            //---------------------------------------------------------------------------------------------------


            var pendente_1 = new Status
            {
                Texto = StatusDisplay.StatusPendenteEmLoja.PL_Fotos,
                Tipo = TiposStatus.TipoStatusList[0].Id,
                Cortexto = StatusDisplay.StatusPendenteEmLoja.PL_cortexto,
                Corfundo = StatusDisplay.StatusPendenteEmLoja.PL_corfundo
            };
            DbStatusSeed(pendente_1, db);

            var pendente_2 = new Status
            {
                Texto = StatusDisplay.StatusPendenteEmLoja.PL_Informa,
                Tipo = TiposStatus.TipoStatusList[0].Id,
                Cortexto = StatusDisplay.StatusPendenteEmLoja.PL_cortexto,
                Corfundo = StatusDisplay.StatusPendenteEmLoja.PL_corfundo
            };
            DbStatusSeed(pendente_2, db);

            var pendente_3 = new Status
            {
                Texto = StatusDisplay.StatusPendenteEmLoja.PL_Infiltra,
                Tipo = TiposStatus.TipoStatusList[0].Id,
                Cortexto = StatusDisplay.StatusPendenteEmLoja.PL_cortexto,
                Corfundo = StatusDisplay.StatusPendenteEmLoja.PL_corfundo
            };
            DbStatusSeed(pendente_3, db);

            var pendente_4 = new Status
            {
                Texto = StatusDisplay.StatusPendenteEmLoja.PL_Camurça,
                Tipo = TiposStatus.TipoStatusList[0].Id,
                Cortexto = StatusDisplay.StatusPendenteEmLoja.PL_cortexto,
                Corfundo = StatusDisplay.StatusPendenteEmLoja.PL_corfundo
            };
            DbStatusSeed(pendente_4, db);

            var pendente_5 = new Status
            {
                Texto = StatusDisplay.StatusPendenteEmLoja.PL_Algodão,
                Tipo = TiposStatus.TipoStatusList[0].Id,
                Cortexto = StatusDisplay.StatusPendenteEmLoja.PL_cortexto,
                Corfundo = StatusDisplay.StatusPendenteEmLoja.PL_corfundo
            };
            DbStatusSeed(pendente_5, db);


            //---------------------------------------------------------------------------------------------------


            var validação_1 = new Status
            {
                Texto = StatusDisplay.StatusAguardaValidação.Avalidação,
                Tipo = TiposStatus.TipoStatusList[1].Id,
                Cortexto = StatusDisplay.StatusAguardaValidação.AV_cortexto,
                Corfundo = StatusDisplay.StatusAguardaValidação.AV_corfundo
            };
            DbStatusSeed(validação_1, db);

            var decisão_1 = new Status
            {
                Texto = StatusDisplay.StatusAguardaDecisão.ADecisão,
                Tipo = TiposStatus.TipoStatusList[2].Id,
                Cortexto = StatusDisplay.StatusAguardaDecisão.AD_cortexto,
                Corfundo = StatusDisplay.StatusAguardaDecisão.AD_corfundo
            };
            DbStatusSeed(decisão_1, db);


            //---------------------------------------------------------------------------------------------------


            var opinião_1 = new Status
            {
                Texto = StatusDisplay.StatusAguardaOpinião.AO_GerenteLoja,
                Tipo = TiposStatus.TipoStatusList[3].Id,
                Cortexto = StatusDisplay.StatusAguardaOpinião.AO_cortexto,
                Corfundo = StatusDisplay.StatusAguardaOpinião.AO_corfundo
            };
            DbStatusSeed(opinião_1, db);

            var opinião_2 = new Status
            {
                Texto = StatusDisplay.StatusAguardaOpinião.AO_Supervisor,
                Tipo = TiposStatus.TipoStatusList[4].Id,
                Cortexto = StatusDisplay.StatusAguardaOpinião.AO_cortexto,
                Corfundo = StatusDisplay.StatusAguardaOpinião.AO_corfundo
            };
            DbStatusSeed(opinião_2, db);

            var opinião_3 = new Status
            {
                Texto = StatusDisplay.StatusAguardaOpinião.AO_Revisor,
                Tipo = TiposStatus.TipoStatusList[5].Id,
                Cortexto = StatusDisplay.StatusAguardaOpinião.AO_cortexto,
                Corfundo = StatusDisplay.StatusAguardaOpinião.AO_corfundo
            };
            DbStatusSeed(opinião_3, db);

            var opinião_4 = new Status
            {
                Texto = StatusDisplay.StatusAguardaOpinião.AO_Fornecedor,
                Tipo = TiposStatus.TipoStatusList[6].Id,
                Cortexto = StatusDisplay.StatusAguardaOpinião.AO_cortexto,
                Corfundo = StatusDisplay.StatusAguardaOpinião.AO_corfundo
            };
            DbStatusSeed(opinião_4, db);


            //---------------------------------------------------------------------------------------------------


            var aceite_1 = new Status
            {
                Texto = StatusDisplay.StatusAceiteAguardaResoluçãoCliente.Aceite,
                Tipo = TiposStatus.TipoStatusList[7].Id,
                Cortexto = StatusDisplay.StatusAceiteAguardaResoluçãoCliente.AA_cortexto,
                Corfundo = StatusDisplay.StatusAceiteAguardaResoluçãoCliente.AA_corfundo
            };
            DbStatusSeed(aceite_1, db);

            var nãoaceite_1 = new Status
            {
                Texto = StatusDisplay.StatusRecusadaAguardaResoluçãoCliente.NãoAceite,
                Tipo = TiposStatus.TipoStatusList[8].Id,
                Cortexto = StatusDisplay.StatusRecusadaAguardaResoluçãoCliente.NA_cortexto,
                Corfundo = StatusDisplay.StatusRecusadaAguardaResoluçãoCliente.NA_corfundo
            };
            DbStatusSeed(nãoaceite_1, db);


            //---------------------------------------------------------------------------------------------------


            var fechar_1 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Nãoaceite,
                Tipo = TiposStatus.TipoStatusList[9].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_1, db);

            var fechar_2 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Trocadireta,
                Tipo = TiposStatus.TipoStatusList[10].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_2, db);

            var fechar_3 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Reparação,
                Tipo = TiposStatus.TipoStatusList[11].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_3, db);

            var fechar_4 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Trocaartigo,
                Tipo = TiposStatus.TipoStatusList[12].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_4, db);

            var fechar_5 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Devdinheiro,
                Tipo = TiposStatus.TipoStatusList[12].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_5, db);

            var fechar_6 = new Status
            {
                Texto = StatusDisplay.StatusFecharEmLoja.FL_Notacrédito,
                Tipo = TiposStatus.TipoStatusList[13].Id,
                Cortexto = StatusDisplay.StatusFecharEmLoja.FL_cortexto,
                Corfundo = StatusDisplay.StatusFecharEmLoja.FL_corfundo
            };
            DbStatusSeed(fechar_6, db);


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------


        private static int DbStatusSeed(Status status, LVLgroupDbContext db)
        {
            db.Status.Add(status);
            db.SaveChanges();
            return status.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
