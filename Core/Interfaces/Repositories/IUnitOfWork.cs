using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {

        //---------------------------------------------------------------------------------------------------


        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();


        //---------------------------------------------------------------------------------------------------

    }
}