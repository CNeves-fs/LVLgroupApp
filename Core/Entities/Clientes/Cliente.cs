using Core.Abstractions;
using Core.Entities.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Clientes
{
    public class Cliente : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Nome { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string NIF { get; set; }

        [Required]
        public string Telefone { get; set; }

        [StringLength(50)]
        public string IBAN { get; set; }

        [StringLength(255)]
        public string Morada { get; set; }

        public DateTime DataUltimoContacto { get; set; }

        public string TipoContacto { get; set; }

        public string DescriçãoContacto { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
