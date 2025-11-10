using Core.Entities.Clientes;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Cliente> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ClienteRepository(IDistributedCache distributedCache, IRepositoryAsync<Cliente> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Cliente> Clientes => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Cliente cliente)
        {
            await _repository.DeleteAsync(cliente);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetKey(cliente.Id));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetTelefoneKey(cliente.Telefone));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetEmailKey(cliente.Email));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNIFKey(cliente.NIF));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNomeKey(cliente.Nome));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByIdAsync(int clienteId)
        {
            return await _repository.Entities.Where(c => c.Id == clienteId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByTelefoneAsync(string telefone)
        {
            return await _repository.Entities.Where(c => c.Telefone == telefone.CleanTelefone()).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<bool> ExistByTelefoneAsync(string telefone)
        {
            var cli = await _repository.Entities.Where(c => c.Telefone == telefone.CleanTelefone()).FirstOrDefaultAsync();
            return cli != null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByEmailAsync(string email)
        {
            return await _repository.Entities.Where(c => c.Email == email).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<bool> ExistByEmailAsync(string email)
        {
            var cli = await _repository.Entities.Where(c => c.Email == email).FirstOrDefaultAsync();
            return cli != null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByNIFAsync(string nif)
        {
            return await _repository.Entities.Where(c => c.NIF == nif).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByNomeAsync(string nome)
        {
            return await _repository.Entities.Where(c => c.Nome == nome).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<bool> ExistByNomeAsync(string nome)
        {
            var cli = await _repository.Entities.Where(c => c.Nome == nome).FirstOrDefaultAsync();
            return cli != null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Cliente>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Cliente cliente)
        {
            await _repository.AddAsync(cliente);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetKey(cliente.Id));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetTelefoneKey(cliente.Telefone));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetEmailKey(cliente.Email));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNIFKey(cliente.NIF));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNomeKey(cliente.Nome));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.SelectListKey);
            return cliente.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Cliente cliente)
        {
            await _repository.UpdateAsync(cliente);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetKey(cliente.Id));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetTelefoneKey(cliente.Telefone));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetEmailKey(cliente.Email));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNIFKey(cliente.NIF));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.GetNomeKey(cliente.Nome));
            await _distributedCache.RemoveAsync(ClienteCacheKeys.SelectListKey);
        }



        //---------------------------------------------------------------------------------------------------

    }
}