using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Claim.Models.Aprovação
{
    public class TabAprovaçãoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string DecisãoFinal { get; set; }

        public bool Rejeitada { get; set; }

        public bool TrocaArtigo { get; set; }

        public bool DevoluçãoDinheiro { get; set; }

        public bool ReparaçãoArtigo { get; set; }

        public bool NotaCrédito { get; set; }

        public DateTime DataDecisão { get; set; }

        public string EmailAutorDecisão { get; set; }

        public string ObservaçõesFecho { get; set; }

        public DateTime DataFecho { get; set; }

        public string EmailAutorFechoEmLoja { get; set; }

        public bool EnableAllEditarDecisão { get; set; }     // permite a todos editar 'decisão final'

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRevisor { get; set; }

        public bool IsSupervisor { get; set; }

        public bool IsGerenteLoja { get; set; }

        public bool IsColaborador { get; set; }

        public bool IsBasic { get; set; }

        public string EmailCurrentUser { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}