using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Business.Models.Grupoloja
{
    public class GrupolojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int MaxDiasDecisão { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public SelectList Empresas { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}