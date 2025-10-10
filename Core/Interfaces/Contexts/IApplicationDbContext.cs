using Core.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {

        //---------------------------------------------------------------------------------------------------


        IDbConnection Connection { get; }

        bool HasChanges { get; }

        EntityEntry Entry(object entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<Empresa> Empresas { get; set; }

        DbSet<Loja> Lojas { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}