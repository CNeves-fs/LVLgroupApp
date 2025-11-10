using Core.Entities.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface INotificationSendedRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<NotificationSended> NotificationsSended { get; }

        Task<List<NotificationSended>> GetListAsync();

        Task<List<NotificationSended>> GetListToUserIdAsync(string userId);

        Task<List<NotificationSended>> GetNotReadListAsync();

        Task<List<NotificationSended>> GetNotReadListToUserIdAsync(string userId);

        Task<List<NotificationSended>> GetListNotificationIdAsync(int notificationId);

        Task<NotificationSended> GetByIdAsync(int notificationSendedId);

        Task<int> InsertAsync(NotificationSended NotificationSended);

        Task UpdateAsync(NotificationSended NotificationSended);

        Task DeleteAsync(NotificationSended NotificationSended);


        //---------------------------------------------------------------------------------------------------

    }
}