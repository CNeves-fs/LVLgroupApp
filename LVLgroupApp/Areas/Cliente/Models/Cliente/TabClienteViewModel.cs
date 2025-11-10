using Core.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVLgroupApp.Areas.Cliente.Models.Cliente
{
    public class TabClienteViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string NIF { get; set; }

        [Required]
        public string Telefone { get; set; }

        public string IBAN { get; set; }

        public string Morada { get; set; }

        public DateTime DataUltimoContacto { get; set; } = DateTime.Now;

        public int TipoContactoId { get; set; } = TiposContactoDeCliente.TipoContactoList[0].Id;

        public string TipoContacto { get; set; } = TiposContactoDeCliente.TipoContactoList[0].Tipo;

        public string DescriçãoContacto { get; set; }


        [NotMapped]
        public SelectList TipoContactoList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}