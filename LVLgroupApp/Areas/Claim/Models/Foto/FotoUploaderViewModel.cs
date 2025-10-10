using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LVLgroupApp.Areas.Claim.Models.Foto
{
    public class FotoUploaderViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int ClaimId { get; set; }

        public string ClaimFolder { get; set; }

        public string Descrição { get; set; }

        public int? FototagId { get; set; }

        public IFormFile FilePhoto { get; set; }

        public SelectList Fototags { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
