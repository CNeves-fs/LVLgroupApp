using Core.Interfaces.Repositories;
using Core.Interfaces.Shared;
using Infrastructure.Data.DbContext;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly LVLgroupDbContext _dbContext;
        private bool disposed;


        //---------------------------------------------------------------------------------------------------


        public UnitOfWork(LVLgroupDbContext dbContext, IAuthenticatedUserService authenticatedUserService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _authenticatedUserService = authenticatedUserService;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        //---------------------------------------------------------------------------------------------------


        public Task Rollback()
        {
            //todo
            return Task.CompletedTask;
        }


        //---------------------------------------------------------------------------------------------------


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        //---------------------------------------------------------------------------------------------------


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }


        //---------------------------------------------------------------------------------------------------

    }
}