using System.Collections.Generic;

namespace LVLgroupApp.Areas.Claim.Models.Foto
{
    public class FotoGalleryViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int? ClaimId { get; set; }

        public string ClaimFolder { get; set; }

        public List<FotoViewModel> Fotos { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
