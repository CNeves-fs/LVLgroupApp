using Core.Entities.Mail;
using System.Threading.Tasks;

namespace Core.Interfaces.Shared
{
    public interface IMailService
    {

        //---------------------------------------------------------------------------------------------------


        Task SendAsync(MailRequest mailRequest);


        //---------------------------------------------------------------------------------------------------

    }
}