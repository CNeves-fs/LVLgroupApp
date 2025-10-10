using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia
{
    public class TipoOcorrenciaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }

        public int CategoriaId { get; set; }

        public SelectList Categorias { get; set; }

        public string EsName { get; set; }

        public string EnName { get; set; }

        public string ToUserIds { get; set; }                       // strings separated by ";"

        public string ToUserEmails { get; set; }                    // strings separated by ";"

        public string ToUserGroups { get; set; }                    // strings separated by ";"

        public bool EditMode { get; set; }




        public ICollection<TipoOcorrenciaLocalizedViewModel> Translations { get; set; }

        public ICollection<NotificacaoOcorrenciaViewModel> Notificações { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
