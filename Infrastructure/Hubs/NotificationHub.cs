using Core.Entities.Hub;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{

    public class NotificationHub : Hub
    {

        //---------------------------------------------------------------------------------------------------


        private readonly LVLgroupDbContext _dbContext;


        //---------------------------------------------------------------------------------------------------


        public NotificationHub(LVLgroupDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //---------------------------------------------------------------------------------------------------


        //public async Task SendNotificationToAll()
        //{
        //    await Clients.All.SendAsync("ReceivedNotification");
        //}


        //---------------------------------------------------------------------------------------------------


        //public async Task SendNotificationToClient(string userId)
        //{
        //    var hubConnections = _dbContext.HubConnections.Where(con => con.UserId == userId).ToList();
        //    foreach (var hubConnection in hubConnections)
        //    {
        //        await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification");
        //    }
        //}


        //---------------------------------------------------------------------------------------------------


        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnConnected");
            await base.OnConnectedAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task SaveUserConnection(string userId)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection()
            {
                ConnectionId = connectionId,
                UserId = userId
            };

            _dbContext.HubConnections.Add(hubConnection);
            await _dbContext.SaveChangesAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = _dbContext.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if(hubConnection != null)
            {
                _dbContext.HubConnections.Remove(hubConnection);
                await _dbContext.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }


        //---------------------------------------------------------------------------------------------------

    }
}
