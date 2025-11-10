using Core.Entities.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Core.Constants
{
    public static class QuestionTypeList
    {

        //---------------------------------------------------------------------------------------------------


        public const int QTYPE_NUMERICA = 1;
        public const int QTYPE_VERDADEIRO_FALSO = 2;
        public const int QTYPE_SIM_NAO = 3;
        public const int QTYPE_TEXTO = 4;
        public const int QTYPE_MONETARIO = 5;
        public const int QTYPE_ESCOLHA_MULTIPLA = 6;


        //---------------------------------------------------------------------------------------------------


        private static List<QuestionType> List_Pt { get; } = new List<QuestionType>()
        {
            new QuestionType() { Id = QTYPE_NUMERICA, Name = "Numérica" },
            new QuestionType() { Id = QTYPE_VERDADEIRO_FALSO, Name = "VerdadeiroOuFalso" },
            new QuestionType() { Id = QTYPE_SIM_NAO, Name = "SimOuNao" },
            new QuestionType() { Id = QTYPE_TEXTO, Name = "Texto" },
            new QuestionType() { Id = QTYPE_MONETARIO, Name = "Monetária" },
            new QuestionType() { Id = QTYPE_ESCOLHA_MULTIPLA, Name = "EscolhaMultipla" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<QuestionType> List_Es { get; } = new List<QuestionType>()
        {
            new QuestionType() { Id = QTYPE_NUMERICA, Name = "Numérico" },
            new QuestionType() { Id = QTYPE_VERDADEIRO_FALSO, Name = "VerdaderoOFalso" },
            new QuestionType() { Id = QTYPE_SIM_NAO, Name = "SiONo" },
            new QuestionType() { Id = QTYPE_TEXTO, Name = "Texto" },
            new QuestionType() { Id = QTYPE_MONETARIO, Name = "Monetario" },
            new QuestionType() { Id = QTYPE_ESCOLHA_MULTIPLA, Name = "OpciónMúltiple" }
        };


        //---------------------------------------------------------------------------------------------------


        private static List<QuestionType> List_En { get; } = new List<QuestionType>()
        {
            new QuestionType() { Id = QTYPE_NUMERICA, Name = "Numeric" },
            new QuestionType() { Id = QTYPE_VERDADEIRO_FALSO, Name = "TrueOrFalse" },
            new QuestionType() { Id = QTYPE_SIM_NAO, Name = "YesOrNo" },
            new QuestionType() { Id = QTYPE_TEXTO, Name = "Text" },
            new QuestionType() { Id = QTYPE_MONETARIO, Name = "Monetary" },
            new QuestionType() { Id = QTYPE_ESCOLHA_MULTIPLA, Name = "MultipleChoice" }
        };


        //---------------------------------------------------------------------------------------------------


        public static Dictionary<string, List<QuestionType>> List = new Dictionary<string, List<QuestionType>>
        {
            ["pt"] = List_Pt,
            ["es"] = List_Es,
            ["en"] = List_En
        };


        //---------------------------------------------------------------------------------------------------


        public static SelectList GetSelectListQuestionType(string lang, int selectedId)
        {
            return new SelectList(QuestionTypeList.List[lang], nameof(QuestionType.Id), nameof(QuestionType.Name), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static string GetQuestionTypeName(string lang, int questionTypeId)
        {
            var qtList = List[lang];
            var qtName = qtList.Where(qt => qt.Id == questionTypeId).Select(qt => qt.Name).FirstOrDefault();
            return qtName;
        }


        //---------------------------------------------------------------------------------------------------

    }
}