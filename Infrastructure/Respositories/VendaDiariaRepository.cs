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
    public class VendaDiariaRepository : IVendaDiariaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<VendaDiaria> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public VendaDiariaRepository(IDistributedCache distributedCache, IRepositoryAsync<VendaDiaria> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<VendaDiaria> VendasDiarias => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByMercadoIdCachedListAsync(int mercadoId)
        {
            return await _repository.Entities.Where(vd => vd.MercadoId == mercadoId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByMercadoIdAnoAsync(int mercadoId, int ano)
        {
            return await _repository.Entities.Where(vd => (vd.MercadoId == mercadoId) && (vd.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByMercadoIdMesAsync(int mercadoId, int ano, int mes)
        {
            return await _repository.Entities.Where(vd => (vd.MercadoId == mercadoId) && (vd.Ano == ano) && (vd.Mês == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByEmpresaIdCachedListAsync(int empresaId)
        {
            return await _repository.Entities.Where(vd => vd.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByEmpresaIdAnoAsync(int empresaId, int ano)
        {
            return await _repository.Entities.Where(vd => (vd.EmpresaId == empresaId) && (vd.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByEmpresaIdMesAsync(int empresaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vd => (vd.EmpresaId == empresaId) && (vd.Ano == ano) && (vd.Mês == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByGrupolojaIdCachedListAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(vd =>(vd.GrupolojaId == grupolojaId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vd => (vd.GrupolojaId == grupolojaId) && (vd.Ano == ano) && (vd.Mês == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByGrupolojaIdAnoAsync(int grupolojaId, int ano)
        {
            return await _repository.Entities.Where(vd => (vd.GrupolojaId == grupolojaId) && (vd.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromLojaIdAsync(int lojaId)
        {
            return await _repository.Entities.Where(vd => vd.LojaId == lojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByLojaIdAnoAsync(int lojaId, int ano)
        {
            return await _repository.Entities.Where(vd => (vd.LojaId == lojaId) && (vd.Ano == ano)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListByLojaIdMesAsync(int lojaId, int ano, int mes)
        {
            return await _repository.Entities.Where(vd => (vd.LojaId == lojaId) && (vd.Ano == ano) && (vd.Mês == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByLojaIdDataAsync(int lojaId, int ano, int mês, int diaDoMês)
        {
            return await _repository.Entities.Where(vd => (vd.LojaId == lojaId) && (vd.Ano == ano) && (vd.Mês == mês) && (vd.DiaDoMês == diaDoMês)).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromAnoAsync(int ano)
        {
            return await _repository.Entities.Where(vd => vd.Ano == ano).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromTrimestreAsync(int ano, int trimestre)
        {
            var firstMes = trimestre * 3 - 2; // Calcula o primeiro mês do trimestre
            var lastMes = trimestre * 3; // Calcula o último mês do trimestre
            return await _repository.Entities.Where(vd => (vd.Ano == ano) && (vd.Mês >= firstMes) && (vd.Mês <= lastMes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromMesAsync(int ano, int mes)
        {
            return await _repository.Entities.Where(vd => (vd.Ano == ano) && (vd.Mês == mes)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromDiaAsync(int ano, int mes, int dia)
        {
            return await _repository.Entities.Where(vd => (vd.Ano == ano) && (vd.Mês == mes) && (vd.DiaDoMês == dia)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        // não utilizar
        public async Task<List<VendaDiaria>> GetListFromSemanaAsync(int ano, int semana)
        {
            return await _repository.Entities.Where(vd => (vd.Ano == ano) && (vd.Mês == semana)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetListFromVendaSemanalIdAsync(int vendaSemanalId)
        {
            return await _repository.Entities.Where(vd => vd.VendaSemanalId == vendaSemanalId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByVendaSemanalIdDiaAsync(int vendaSemanalId, int diaDaSemana)
        {
            return await _repository.Entities.Where(vd => (vd.VendaSemanalId == vendaSemanalId) && (vd.DiaDaSemana == diaDaSemana)).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByIdAsync(int vendaDiariaId)
        {
            return await _repository.Entities.Where(vd => vd.Id == vendaDiariaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(VendaDiaria vendaDiaria)
        {
            await _repository.AddAsync(vendaDiaria);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.SelectListKey);

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoKey(vendaDiaria.MercadoId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoAnoKey(vendaDiaria.MercadoId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoMesKey(vendaDiaria.MercadoId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaKey(vendaDiaria.EmpresaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaAnoKey(vendaDiaria.EmpresaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaMesKey(vendaDiaria.EmpresaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromLojaData(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromAnoKey(vendaDiaria.Ano));
            var trimestre = (vendaDiaria.Mês - 1) / 3 + 1; // Calcula o trimestre baseado no mês
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromTrimestreKey(vendaDiaria.Ano, trimestre));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMesKey(vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromDiaKey(vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromVendaSemanalKey(vendaDiaria.VendaSemanalId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromVendaSemanalDia(vendaDiaria.VendaSemanalId, vendaDiaria.DiaDaSemana));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKey(vendaDiaria.Id));
            return vendaDiaria.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(VendaDiaria vendaDiaria)
        {
            await _repository.DeleteAsync(vendaDiaria);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.SelectListKey);

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoKey(vendaDiaria.MercadoId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoAnoKey(vendaDiaria.MercadoId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoMesKey(vendaDiaria.MercadoId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaKey(vendaDiaria.EmpresaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaAnoKey(vendaDiaria.EmpresaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaMesKey(vendaDiaria.EmpresaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromLojaData(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromAnoKey(vendaDiaria.Ano));
            var trimestre = (vendaDiaria.Mês - 1) / 3 + 1; // Calcula o trimestre baseado no mês
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromTrimestreKey(vendaDiaria.Ano, trimestre));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMesKey(vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromDiaKey(vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromVendaSemanalKey(vendaDiaria.VendaSemanalId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromVendaSemanalDia(vendaDiaria.VendaSemanalId, vendaDiaria.DiaDaSemana));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKey(vendaDiaria.Id));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(VendaDiaria vendaDiaria)
        {
            await _repository.UpdateAsync(vendaDiaria);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.SelectListKey);

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoKey(vendaDiaria.MercadoId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoAnoKey(vendaDiaria.MercadoId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMercadoMesKey(vendaDiaria.MercadoId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaKey(vendaDiaria.EmpresaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaAnoKey(vendaDiaria.EmpresaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromEmpresaMesKey(vendaDiaria.EmpresaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromGrupolojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaKey(vendaDiaria.GrupolojaId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaAnoKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromLojaMesKey(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromLojaData(vendaDiaria.GrupolojaId, vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromAnoKey(vendaDiaria.Ano));
            var trimestre = (vendaDiaria.Mês - 1) / 3 + 1; // Calcula o trimestre baseado no mês
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromTrimestreKey(vendaDiaria.Ano, trimestre));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromMesKey(vendaDiaria.Ano, vendaDiaria.Mês));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromDiaKey(vendaDiaria.Ano, vendaDiaria.Mês, vendaDiaria.DiaDoMês));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.ListFromVendaSemanalKey(vendaDiaria.VendaSemanalId));
            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKeyFromVendaSemanalDia(vendaDiaria.VendaSemanalId, vendaDiaria.DiaDaSemana));

            await _distributedCache.RemoveAsync(VendaDiariaCacheKeys.GetKey(vendaDiaria.Id));
        }


        //---------------------------------------------------------------------------------------------------

    }
}