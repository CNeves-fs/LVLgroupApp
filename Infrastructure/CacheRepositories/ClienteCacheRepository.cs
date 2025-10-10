
using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Clientes;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class ClienteCacheRepository : IClienteCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IClienteRepository _clienteRepository;


        //---------------------------------------------------------------------------------------------------


        public ClienteCacheRepository(IDistributedCache distributedCache, IClienteRepository clienteRepository)
        {
            _distributedCache = distributedCache;
            _clienteRepository = clienteRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByIdAsync(int clienteId)
        {
            string cacheKey = ClienteCacheKeys.GetKey(clienteId);
            var cliente = await _distributedCache.GetAsync<Cliente>(cacheKey);
            if (cliente == null)
            {
                cliente = await _clienteRepository.GetByIdAsync(clienteId);
                // Throw.Exception.IfNull(cliente, "Cliente", "Cliente not Found");
                if (cliente == null) return null;
                await _distributedCache.SetAsync(cacheKey, cliente);
            }
            return cliente;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByTelefoneAsync(string telefone)
        {
            string cacheKey = ClienteCacheKeys.GetTelefoneKey(telefone);
            var cliente = await _distributedCache.GetAsync<Cliente>(cacheKey);
            if (cliente == null)
            {
                cliente = await _clienteRepository.GetByTelefoneAsync(telefone);
                // Throw.Exception.IfNull(cliente, "Cliente", "Cliente not Found");
                if (cliente == null) return null;
                await _distributedCache.SetAsync(cacheKey, cliente);
            }
            return cliente;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByEmailAsync(string email)
        {
            string cacheKey = ClienteCacheKeys.GetEmailKey(email);
            var cliente = await _distributedCache.GetAsync<Cliente>(cacheKey);
            if (cliente == null)
            {
                cliente = await _clienteRepository.GetByEmailAsync(email);
                // Throw.Exception.IfNull(cliente, "Cliente", "Cliente not Found");
                if (cliente == null) return null;
                await _distributedCache.SetAsync(cacheKey, cliente);
            }
            return cliente;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByNIFAsync(string nif)
        {
            string cacheKey = ClienteCacheKeys.GetNIFKey(nif);
            var cliente = await _distributedCache.GetAsync<Cliente>(cacheKey);
            if (cliente == null)
            {
                cliente = await _clienteRepository.GetByNIFAsync(nif);
                // Throw.Exception.IfNull(cliente, "Cliente", "Cliente not Found");
                if (cliente == null) return null;
                await _distributedCache.SetAsync(cacheKey, cliente);
            }
            return cliente;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Cliente> GetByNomeAsync(string nome)
        {
            string cacheKey = ClienteCacheKeys.GetNomeKey(nome);
            var cliente = await _distributedCache.GetAsync<Cliente>(cacheKey);
            if (cliente == null)
            {
                cliente = await _clienteRepository.GetByNomeAsync(nome);
                // Throw.Exception.IfNull(cliente, "Cliente", "Cliente not Found");
                if (cliente == null) return null;
                await _distributedCache.SetAsync(cacheKey, cliente);
            }
            return cliente;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Cliente>> GetCachedListAsync()
        {
            string cacheKey = ClienteCacheKeys.ListKey;
            var clienteList = await _distributedCache.GetAsync<List<Cliente>>(cacheKey);
            if (clienteList == null)
            {
                clienteList = await _clienteRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, clienteList);
            }
            return clienteList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}