using Core.Entities.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface INotificationRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Notification> Notifications { get; }

        Task<List<Notification>> GetListAsync();

        Task<List<Notification>> GetListFromUserIdAsync(string userId);

        Task<Notification> GetByIdAsync(int notificationId);

        Task<int> InsertAsync(Notification notification);

        Task UpdateAsync(Notification notification);

        Task DeleteAsync(Notification notification);


        //---------------------------------------------------------------------------------------------------

    }
}