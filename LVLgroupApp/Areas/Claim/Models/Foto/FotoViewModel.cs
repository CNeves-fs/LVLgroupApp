namespace LVLgroupApp.Areas.Claim.Models.Foto
{
    public class FotoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public string ImageSource { get; set; }

        public string ImageWidth { get; set; }

        public string ImageHeight { get; set; }

        public int? FototagId { get; set; }

        public string Tag {  get; set; }

        public string Descrição { get; set; }

        public int? ClaimId { get; set; }

        public string ClaimFolder { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
