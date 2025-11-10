using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Admin.Models
{
    public class UserViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string Local { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; } = true;

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int MercadoId { get; set; }

        public string NomeMercado { get; set; }

        public int  EmpresaId { get; set; }

        public string NomeEmpresa { get; set; }

        public SelectList Empresas { get; set; }

        public int GrupolojaId { get; set; }

        public string NomeGrupoloja { get; set; }

        public SelectList Gruposlojas { get; set; }

        public int LojaId { get; set; }

        public string NomeLoja { get; set; }

        public SelectList Lojas { get; set; }

        public byte[] ProfilePicture { get; set; }

        public DateTime LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; } = true;

        public int AccessFailedCount { get; set; }

        public string Id { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}