using Microsoft.AspNetCore.SignalR;

namespace LVLgroupApp.Hubs
{
    public class ProgressHub : Hub
    {

        //---------------------------------------------------------------------------------------------------


        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
