using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Business.Models.Empresa
{
    public class EmpresaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string NomeCurto { get; set; }

        public byte[] LogoPicture { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}