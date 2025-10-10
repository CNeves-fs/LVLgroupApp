using Core.Entities.Hub;
using Core.Entities.Notifications;
using Core.Interfaces.Shared;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TableDependency.SqlClient;
using static Core.Constants.Permissions;

namespace Infrastructure.Data.DbContext
{

    public class SubscribeNotificationTableDependency : ISubscribeTableDependency
    {

        //---------------------------------------------------------------------------------------------------


        private SqlTableDependency<NotificationSended> tableDependency;

        private readonly IServiceProvider _serviceProvider;


        //---------------------------------------------------------------------------------------------------


        public SubscribeNotificationTableDependency(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        //---------------------------------------------------------------------------------------------------


        public void SubscribeTableDependency(string connectionString)
        {
            try
            {
                tableDependency = new SqlTableDependency<NotificationSended>(connectionString, "NotificationsSended");
                tableDependency.OnChanged += async (sender, e) =>
                {
                    // Criar um escopo para resolver serviços Scoped
                    using (var scope = _serviceProvider.CreateScope())
                    {

                        var dbContext = scope.ServiceProvider.GetRequiredService<LVLgroupDbContext>();
                        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                        // verificar se notificationSended change == deleted
                        // se notificationSended change == deleted não notificar o cliente
                        Console.WriteLine($"ChangeType: {e.ChangeType.ToString()}");
                        if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Delete) return;

                        // Obtenha a notificação e verifique se o destinatário está conectado
                        var notificationSended = e.Entity;
                        var hubConnections = dbContext.HubConnections.Where(con => con.UserId == notificationSended.ToUserId).ToList();

                        foreach (var hubConnection in hubConnections)
                        {
                            await hubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification");
                        }
                    }
                };
                tableDependency.OnError += TableDependency_OnError;
                //tableDependency.OnStatusChanged += (sender, e) => Console.WriteLine($"Status: {e.Status}");
                tableDependency.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SubscribeTableDependency Error: {ex.Message}");
            }
        }


        //---------------------------------------------------------------------------------------------------


        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(NotificationSended)} SqlTableDependency error: {e.Error.Message}");
        }


        //---------------------------------------------------------------------------------------------------

    }
}
