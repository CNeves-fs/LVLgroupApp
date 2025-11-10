using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Business.Models.Loja
{
    public class LojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string NomeCurto { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string País { get; set; }

        [Required]
        public int GrupolojaId { get; set; }    // _CreateOrEdit


        public string GrupolojaNome { get; set; }   // _ViewAll

        public SelectList Gruposlojas { get; set; } // _CreateOrEdit

        public int EmpresaId { get; set; }      // _CreateOrEdit

        public string EmpresaNome { get; set; }   // _ViewAll

        public SelectList Empresas { get; set; }    // _CreateOrEdit

        [Required]
        public int MercadoId { get; set; }    // _CreateOrEdit

        public string MercadoNome { get; set; }   // _ViewAll

        public SelectList Mercados { get; set; }    // _CreateOrEdit

        public string GerenteId { get; set; } //    _CreateOrEdit

        public string EmailGerente { get; set; }    // _CreateOrEdit

        public byte[] ProfilePictureGerente { get; set; }   // _CreateOrEdit

        public bool FechoClaimEmLoja { get; set; }     // _CreateOrEdit

        public bool Active { get; set; } // _CreateOrEdit


        //---------------------------------------------------------------------------------------------------

    }
}