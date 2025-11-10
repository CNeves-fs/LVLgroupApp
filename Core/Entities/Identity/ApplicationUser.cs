using Core.Entities.Business;
using Core.Entities.Notifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Core.Entities.Identity

{
    public class ApplicationUser : IdentityUser
    {

        //---------------------------------------------------------------------------------------------------


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool IsActive { get; set; } = false;

        public string Local { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiryTime { get; set; }

        public int? LojaId { get; set; }

        public int? GrupolojaId { get; set; }

        public int? EmpresaId { get; set; }

        public int? MercadoId { get; set; }







        public virtual Loja Loja { get; set; }

        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual Mercado Mercado { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
