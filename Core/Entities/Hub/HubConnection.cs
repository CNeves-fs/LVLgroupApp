using Core.Entities.Business;
using Core.Entities.Identity;

namespace Core.Entities.Hub
{
    public partial class HubConnection
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string ConnectionId { get; set; } = null!;

        public string UserId { get; set; } = null!;



        public virtual ApplicationUser ApplicationUser { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
