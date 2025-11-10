namespace LVLgroupApp.Areas.Home.Models.Error
{
    public class ErrorViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string OriginalPath { get; set; } = string.Empty;

        public string RedirectUrl { get; set; } = string.Empty;

        public string RequestId { get; set; }

        public bool ShowRequestId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}