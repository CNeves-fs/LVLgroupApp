using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class ViewNotificationViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public FromUserDetailViewModel FromUser { get; set; }      // From ApplicationUser

        public string ToUserIds { get; set; }

        public string ToUserGroups { get; set; }                    // strings separated by ";"

        public List<ToUserDetailViewModel> ToUsers { get; set; }

        public int MercadoId { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public SelectList Mercados { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList Gruposlojas { get; set; }

        public SelectList Lojas { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRevisor { get; set; }

        public bool IsSupervisor { get; set; }

        public bool IsGerenteLoja { get; set; }

        public bool IsColaborador { get; set; }

        public bool IsBasic { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
