using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Artigo.Models.Artigo
{
    public class ArtigoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Referencia { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        public string Empresa { get; set; } //_ViewAll

        public SelectList Empresas { get; set; } //_CreateOrEdit

        [Required]
        public int GenderId { get; set; }

        public string Gender { get; set; } //_ViewAll

        public SelectList Genders { get; set; } //_CreateOrEdit

        public string Pele { get; set; }

        public string Cor { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}