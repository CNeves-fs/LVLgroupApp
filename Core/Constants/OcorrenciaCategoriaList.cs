using Core.Entities.Claims;
using Core.Entities.Ocorrencias;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Owin.Security.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Core.Constants
{
    public static class OcorrenciaCategoriaList
    {

        //---------------------------------------------------------------------------------------------------


        public const int CAT_SIMPLES = 1;
        public const int CAT_INTERLOJAS = 2;
        public const int CAT_LOJASEDE = 3;


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaCategoria> List_Pt { get; } = new List<OcorrenciaCategoria>()
        {
            new OcorrenciaCategoria() { Id = CAT_SIMPLES, Categoria = "Simples" },
            //new OcorrenciaCategoria() { Id = CAT_INTERLOJAS, Categoria = "Interlojas" },
            //new OcorrenciaCategoria() { Id = CAT_LOJASEDE, Categoria = "Loja-Sede" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaCategoria> List_Es { get; } = new List<OcorrenciaCategoria>()
        {
            new OcorrenciaCategoria() { Id = CAT_SIMPLES, Categoria = "Simple" },
            //new OcorrenciaCategoria() { Id = CAT_INTERLOJAS, Categoria = "Intertienda" },
            //new OcorrenciaCategoria() { Id = CAT_LOJASEDE, Categoria = "Tienda-Sede" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<OcorrenciaCategoria> List_En { get; } = new List<OcorrenciaCategoria>()
        {
            new OcorrenciaCategoria() { Id = CAT_SIMPLES, Categoria = "Simple" },
            //new OcorrenciaCategoria() { Id = CAT_INTERLOJAS, Categoria = "Cross-Store" },
            //new OcorrenciaCategoria() { Id = CAT_LOJASEDE, Categoria = "Store-HQ" }
        };


        //---------------------------------------------------------------------------------------------------


        public static Dictionary<string, List<OcorrenciaCategoria>> List = new Dictionary<string, List<OcorrenciaCategoria>>
        {
            ["pt"] = List_Pt,
            ["es"] = List_Es,
            ["en"] = List_En
        };


        //---------------------------------------------------------------------------------------------------


        public static SelectList GetSelectListCategoria(string lang, int selectedId)
        {
            return new SelectList(OcorrenciaCategoriaList.List[lang], nameof(OcorrenciaCategoria.Id), nameof(OcorrenciaCategoria.Categoria), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static string GetCategoriaName(string lang, int categoriaId)
        {
            var cList = List[lang];
            var cName = cList.Where(c => c.Id == categoriaId).Select(c => c.Categoria).FirstOrDefault();
            return cName;
        }


        //---------------------------------------------------------------------------------------------------

    }
}