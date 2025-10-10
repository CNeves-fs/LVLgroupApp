namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class ToUserDetailViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Id { get; set; }              //To ApplicationUser

        public string Email { get; set; }           //To ApplicationUser

        public string Name { get; set; }            //To ApplicationUser

        public byte[] ProfilePicture { get; set; }  //To ApplicationUser

        public string RoleName { get; set; }        //To ApplicationUser

        public string Local { get; set; }           //To ApplicationUser (Loja/GrupoLoja)

        public int NotificationId { get; set; }     //Notification.Id

        public bool IsRead { get; set; } = false;   //NotificationSended.IsRead


        //---------------------------------------------------------------------------------------------------

    }
}
