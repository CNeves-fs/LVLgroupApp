namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class FromUserDetailViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Id { get; set; }              //From ApplicationUser

        public string Email { get; set; }           //From ApplicationUser

        public string Name { get; set; }            //From ApplicationUser

        public byte[] ProfilePicture { get; set; }  //From ApplicationUser

        public string RoleName { get; set; }        //From ApplicationUser

        public string Local { get; set; }           //From ApplicationUser (Loja/GrupoLoja)

        public int NotificationId { get; set; }     //Notification.Id


        //---------------------------------------------------------------------------------------------------

    }
}
