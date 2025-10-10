using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Business.Models.Mercado
{
    public class MercadoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string NomeCurto { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}