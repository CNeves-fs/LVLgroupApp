using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Artigo.Models.Gender
{
    public class GenderViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string TamanhosNum { get; set; }

        [Required]
        public string TamanhosAlf { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}