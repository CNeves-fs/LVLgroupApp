using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LVLgroupApp.Hubs
{
    public class ProgressBar
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task StartProgressBarAsync(string startMessage, IHubContext<ProgressHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("ReceiveStartMessage", startMessage);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task SetProgressBarAsync(string setMessage, int percentage, IHubContext<ProgressHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("ReceiveSetMessage", setMessage, percentage);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task StopProgressBarAsync(string stopMessage, IHubContext<ProgressHub> hubContext)
        {
            await hubContext.Clients.All.SendAsync("ReceiveStopMessage", stopMessage);
        }


        //---------------------------------------------------------------------------------------------------

    }
}
