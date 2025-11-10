using Core.Entities.Ocorrencias;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Core.Constants
{
    public static class OcorrenciaStatusList
    {

        //---------------------------------------------------------------------------------------------------


        public const int PEDIDO_ENVIADO = 1;
        public const int PEDIDO_POR_ATENDER = 2;
        public const int PEDIDO_EM_PROCESSAMENTO = 3;
        public const int PEDIDO_PROCESSADO_AGUARDA_FECHO = 4;
        public const int PEDIDO_POR_FECHAR = 5;
        public const int TERMINADO = 6;


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaStatus> List_Pt { get; } = new List<OcorrenciaStatus>()
        {
            new OcorrenciaStatus() { Id = PEDIDO_ENVIADO, Status = "PEDIDO ENVIADO" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_ATENDER, Status = "PEDIDO POR ATENDER" },
            new OcorrenciaStatus() { Id = PEDIDO_EM_PROCESSAMENTO, Status = "PEDIDO EM PROCESSAMENTO" },
            new OcorrenciaStatus() { Id = PEDIDO_PROCESSADO_AGUARDA_FECHO, Status = "PEDIDO PROCESSADO - AGUARDA FECHO" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_FECHAR, Status = "PEDIDO POR FECHAR" },
            new OcorrenciaStatus() { Id = TERMINADO, Status = "TERMINADO" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaStatus> List_Es { get; } = new List<OcorrenciaStatus>()
        {
            new OcorrenciaStatus() { Id = PEDIDO_ENVIADO, Status = "PEDIDO ENVIADO" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_ATENDER, Status = "PEDIDO PENDIENTE DE ATENCIÓN" },
            new OcorrenciaStatus() { Id = PEDIDO_EM_PROCESSAMENTO, Status = "PEDIDO EN PROCESO" },
            new OcorrenciaStatus() { Id = PEDIDO_PROCESSADO_AGUARDA_FECHO, Status = "PEDIDO PROCESADO - EN ESPERA DE CIERRE" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_FECHAR, Status = "PEDIDO PENDIENTE DE CIERRE" },
            new OcorrenciaStatus() { Id = TERMINADO, Status = "FINALIZADO" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaStatus> List_En { get; } = new List<OcorrenciaStatus>()
        {
            new OcorrenciaStatus() { Id = PEDIDO_ENVIADO, Status = "ORDER SENT" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_ATENDER, Status = "ORDER PENDING PROCESSING" },
            new OcorrenciaStatus() { Id = PEDIDO_EM_PROCESSAMENTO, Status = "ORDER IN PROGRESS" },
            new OcorrenciaStatus() { Id = PEDIDO_PROCESSADO_AGUARDA_FECHO, Status = "PROCESSED ORDER - AWAITING CLOSURE" },
            new OcorrenciaStatus() { Id = PEDIDO_POR_FECHAR, Status = "ORDER PENDING CLOSURE" },
            new OcorrenciaStatus() { Id = TERMINADO, Status = "COMPLETED" }
        };


        //---------------------------------------------------------------------------------------------------


        public static Dictionary<string, List<OcorrenciaStatus>> List = new Dictionary<string, List<OcorrenciaStatus>>
        {
            ["pt"] = List_Pt,
            ["es"] = List_Es,
            ["en"] = List_En
        };


        //---------------------------------------------------------------------------------------------------


        private static SelectList GetSelectListStatus(int[] options, string lang)
        {
            if (options == null || options.Length == 0) return new SelectList(new List<OcorrenciaStatus>(), nameof(OcorrenciaStatus.Id), nameof(OcorrenciaStatus.Status), null, null);
            var statusList = new List<OcorrenciaStatus>();
            foreach (var opt in options)
            {
                var status = OcorrenciaStatusList.List[lang][opt - 1];
                statusList.Add(status);
            }
            return new SelectList(statusList, nameof(OcorrenciaStatus.Id), nameof(OcorrenciaStatus.Status), null, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// mecanismo de troca de status por lookup table.
        /// dado o tipo de status corrente é devolvido um array com os
        /// status possíveis para o status seguinte.
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <param name="currentStatusId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>

        public static SelectList GetNextStatusOptions(int categoriaId, int currentStatusId, string lang)
        {
            int[][] Simples_lookupTable =
            {
                new int[] { TERMINADO },        //0 ===> "TERMINADO"
                new int[] { TERMINADO },        //1 "PEDIDO ENVIADO" ===> "TERMINADO"
                new int[] { TERMINADO },        //2 "PEDIDO POR ATENDER" ===> "TERMINADO"
                new int[] { TERMINADO },        //3 "PEDIDO EM PROCESSAMENTO" ===> "TERMINADO"
                new int[] { TERMINADO },        //4 "PEDIDO PROCESSADO - AGUARDA FECHO" ===> "TERMINADO"
                new int[] { TERMINADO },        //5 "PEDIDO POR FECHAR" ===> "TERMINADO"
                new int[] { TERMINADO }         //6 "TERMINADO" ===> "TERMINADO"
            };

            int[][] Interlojas_lookupTable =
            {
                new int[] { PEDIDO_ENVIADO, TERMINADO },                                                            //0 ===> "PEDIDO ENVIADO", "TERMINADO"
                new int[] { PEDIDO_ENVIADO, TERMINADO },                                                            //1 "PEDIDO ENVIADO" ===> "PEDIDO ENVIADO", "TERMINADO"
                new int[] { PEDIDO_POR_ATENDER, PEDIDO_EM_PROCESSAMENTO, PEDIDO_PROCESSADO_AGUARDA_FECHO },         //2 "PEDIDO POR ATENDER" ===> "PEDIDO POR ATENDER", "PEDIDO EM PROCESSAMENTO", "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_EM_PROCESSAMENTO, PEDIDO_PROCESSADO_AGUARDA_FECHO },                             //3 "PEDIDO EM PROCESSAMENTO" ===> "PEDIDO EM PROCESSAMENTO", "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_PROCESSADO_AGUARDA_FECHO },                                                      //4 "PEDIDO PROCESSADO - AGUARDA FECHO" ===> "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_POR_FECHAR, TERMINADO },                                                         //5 "PEDIDO POR FECHAR" ===> "PEDIDO POR FECHAR", "TERMINADO"
                new int[] { TERMINADO }                                                                             //6 "TERMINADO" ===> "TERMINADO"
            };

            int[][] LojaSede_lookupTable =
            {
                new int[] { PEDIDO_ENVIADO, TERMINADO },                                    //0 ===> "PEDIDO ENVIADO", "TERMINADO"
                new int[] { PEDIDO_ENVIADO, TERMINADO },                                    //1 "PEDIDO ENVIADO" ===> "PEDIDO ENVIADO", "TERMINADO"
                new int[] { PEDIDO_POR_ATENDER, PEDIDO_EM_PROCESSAMENTO, 4 },               //2 "PEDIDO POR ATENDER" ===> "PEDIDO POR ATENDER", "PEDIDO EM PROCESSAMENTO", "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_EM_PROCESSAMENTO, PEDIDO_PROCESSADO_AGUARDA_FECHO },     //3 "PEDIDO EM PROCESSAMENTO" ===> "PEDIDO EM PROCESSAMENTO", "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_PROCESSADO_AGUARDA_FECHO },                              //4 "PEDIDO PROCESSADO - AGUARDA FECHO" ===> "PEDIDO PROCESSADO - AGUARDA FECHO"
                new int[] { PEDIDO_POR_FECHAR, TERMINADO },                                 //5 "PEDIDO POR FECHAR" ===> "PEDIDO POR FECHAR", "TERMINADO"
                new int[] { TERMINADO }                                                     //6 "TERMINADO" ===> "TERMINADO"
            }; 

            switch (categoriaId)
            {
                case OcorrenciaCategoriaList.CAT_SIMPLES: return GetSelectListStatus(Simples_lookupTable[currentStatusId], lang);
                case OcorrenciaCategoriaList.CAT_INTERLOJAS: return GetSelectListStatus(Interlojas_lookupTable[currentStatusId], lang);
                case OcorrenciaCategoriaList.CAT_LOJASEDE: return GetSelectListStatus(LojaSede_lookupTable[currentStatusId], lang);
                default: return GetSelectListStatus(null, lang);
            }
        }


        //---------------------------------------------------------------------------------------------------


        public static string GetStatusName(string lang, int statusId)
        {
            var sList = List[lang];
            var sName = sList.Where(s => s.Id == statusId).Select(c => c.Status).FirstOrDefault();
            return sName;
        }


        //---------------------------------------------------------------------------------------------------

    }
}