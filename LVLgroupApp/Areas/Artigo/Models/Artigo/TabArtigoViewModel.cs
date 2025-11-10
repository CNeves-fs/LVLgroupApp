using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Artigo.Models.Artigo
{
    public class TabArtigoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int ArtigoId { get; set; }

        [Required]
        public string Referencia { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        public string Empresa { get; set; }

        [Required]
        public string Tamanho { get; set; }

        [Required]
        public int TamanhoId { get; set; }

        [Required]
        public int GenderId { get; set; }

        [Required]
        public string Gender { get; set; }

        public SelectList Tamanhos { get; set; } 

        public string Pele { get; set; }

        public string Cor { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }

        [Required]
        public string DefeitoDoArtigo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}