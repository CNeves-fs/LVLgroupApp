using Core.Entities.Vendas;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VendaSemanalRepository : IVendaSemanalRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<VendaSemanal> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public VendaSemanalRepository(IDistributedCache distributedCache, IRepositoryAsync<VendaSemanal> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<VendaSemanal> VendasSemanais => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByAnoAsync(int ano)
        {
            return await _repository.Entities.Where(vs => (vs.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListBySemanaAsync(int ano, int numeroDaSemana)
        {
            return await _repository.Entities.Where(vs => (vs.Ano == ano) && (vs.NumeroDaSemana == numeroDaSemana)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListFromLojaIdAsync(int lojaId)
        {
            return await _repository.Entities.Where(vs => vs.LojaId == lojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task<VendaSemanal> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana)
        {
            return await _repository.Entities.Where(vs => (vs.LojaId == lojaId) && (vs.Ano == ano) && (vs.NumeroDaSemana == numeroDaSemana)).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByLojaIdMesAsync(int lojaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vs => (vs.LojaId == lojaId) && (vs.Ano == ano) && (vs.Mes == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByLojaIdQuarterAsync(int lojaId, int ano, int quarter)
        {
            return await _repository.Entities.Where(vs => (vs.LojaId == lojaId) && (vs.Ano == ano) && (vs.Quarter == quarter)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByLojaIdAnoAsync(int lojaId, int ano)
        {
            return await _repository.Entities.Where(vs => (vs.LojaId == lojaId) && (vs.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByMercadoIdCachedListAsync(int mercadoId)
        {
            return await _repository.Entities.Where(vs => vs.MercadoId == mercadoId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana)
        {
            return await _repository.Entities.Where(vs => (vs.MercadoId == mercadoId) && (vs.Ano == ano) && (vs.NumeroDaSemana == numeroDaSemana)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByMercadoIdMesAsync(int mercadoId, int ano, int mes)
        {
            return await _repository.Entities.Where(vs => (vs.MercadoId == mercadoId) && (vs.Ano == ano) && (vs.Mes == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter)
        {
            return await _repository.Entities.Where(vs => (vs.MercadoId == mercadoId) && (vs.Ano == ano) && (vs.Quarter == quarter)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByMercadoIdAnoAsync(int mercadoId, int ano)
        {
            return await _repository.Entities.Where(vs => (vs.MercadoId == mercadoId) && (vs.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByEmpresaIdCachedListAsync(int empresaId)
        {
            return await _repository.Entities.Where(vs => vs.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana)
        {
            return await _repository.Entities.Where(vs => (vs.EmpresaId == empresaId) && (vs.Ano == ano) && (vs.NumeroDaSemana == numeroDaSemana)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByEmpresaIdMesAsync(int empresaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vs => (vs.EmpresaId == empresaId) && (vs.Ano == ano) && (vs.Mes == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter)
        {
            return await _repository.Entities.Where(vs => (vs.EmpresaId == empresaId) && (vs.Ano == ano) && (vs.Quarter == quarter)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByEmpresaIdAnoAsync(int empresaId, int ano)
        {
            return await _repository.Entities.Where(vs => (vs.EmpresaId == empresaId) && (vs.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByGrupolojaIdCachedListAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(vs => vs.GrupolojaId == grupolojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana)
        {
            return await _repository.Entities.Where(vs => (vs.GrupolojaId == grupolojaId) && (vs.Ano == ano) && (vs.NumeroDaSemana == numeroDaSemana)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vs => (vs.GrupolojaId == grupolojaId) && (vs.Ano == ano) && (vs.Mes == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter)
        {
            return await _repository.Entities.Where(vs => (vs.GrupolojaId == grupolojaId) && (vs.Ano == ano) && (vs.Quarter == quarter)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetListByGrupolojaIdAnoAsync(int grupolojaId, int ano)
        {
            return await _repository.Entities.Where(vs => (vs.GrupolojaId == grupolojaId) && (vs.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaSemanal> GetByIdAsync(int vendaSemanalId)
        {
            return await _repository.Entities.Where(vs => vs.Id == vendaSemanalId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(VendaSemanal vendaSemanal)
        {
            await _repository.AddAsync(vendaSemanal);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.GetKey(vendaSemanal.Id));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaKey(vendaSemanal.LojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaSemanaKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaMesKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaQuarterKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaAnoKey(vendaSemanal.LojaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoKey(vendaSemanal.MercadoId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoSemanaKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoMesKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoQuarterKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoAnoKey(vendaSemanal.MercadoId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaKey(vendaSemanal.EmpresaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaSemanaKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaMesKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaQuarterKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaAnoKey(vendaSemanal.EmpresaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaKey(vendaSemanal.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaSemanaKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaMesKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaQuarterKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaAnoKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano));

            return vendaSemanal.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(VendaSemanal vendaSemanal)
        {
            await _repository.DeleteAsync(vendaSemanal);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.GetKey(vendaSemanal.Id));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaKey(vendaSemanal.LojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaSemanaKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaMesKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaQuarterKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaAnoKey(vendaSemanal.LojaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoKey(vendaSemanal.MercadoId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoSemanaKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoMesKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoQuarterKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoAnoKey(vendaSemanal.MercadoId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaKey(vendaSemanal.EmpresaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaSemanaKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaMesKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaQuarterKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaAnoKey(vendaSemanal.EmpresaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaKey(vendaSemanal.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaSemanaKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaMesKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaQuarterKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaAnoKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(VendaSemanal vendaSemanal)
        {
            await _repository.UpdateAsync(vendaSemanal);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.GetKey(vendaSemanal.Id));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaKey(vendaSemanal.LojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaSemanaKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaMesKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaQuarterKey(vendaSemanal.LojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromLojaAnoKey(vendaSemanal.LojaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoKey(vendaSemanal.MercadoId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoSemanaKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoMesKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoQuarterKey(vendaSemanal.MercadoId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromMercadoAnoKey(vendaSemanal.MercadoId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaKey(vendaSemanal.EmpresaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaSemanaKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaMesKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaQuarterKey(vendaSemanal.EmpresaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromEmpresaAnoKey(vendaSemanal.EmpresaId, vendaSemanal.Ano));

            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaKey(vendaSemanal.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaSemanaKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.NumeroDaSemana));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaMesKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Mes));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaQuarterKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano, vendaSemanal.Quarter));
            await _distributedCache.RemoveAsync(VendaSemanalCacheKeys.ListFromGrupolojaAnoKey(vendaSemanal.GrupolojaId, vendaSemanal.Ano));
        }


        //---------------------------------------------------------------------------------------------------

    }
}