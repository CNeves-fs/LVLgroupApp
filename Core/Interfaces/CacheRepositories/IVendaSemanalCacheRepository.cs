using Core.Entities.Vendas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IVendaSemanalCacheRepository
    {

        //---------------------------------------------------------------------------------------------------
        
        Task<List<VendaSemanal>> GetCachedListAsync();

        Task<List<VendaSemanal>> GetByAnoAsync(int ano);

        Task<List<VendaSemanal>> GetBySemanaAsync(int ano, int numeroDaSemana);





        Task<List<VendaSemanal>> GetByLojaIdCachedListAsync(int lojaId);  
        
        Task<VendaSemanal> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetByLojaIdMesAsync(int lojaId, int ano, int mes);

        Task<List<VendaSemanal>> GetByLojaIdQuarterAsync(int lojaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetByLojaIdAnoAsync(int lojaId, int ano);



        Task<List<VendaSemanal>> GetByMercadoIdCachedListAsync(int mercadoId);

        Task<List<VendaSemanal>> GetByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetByMercadoIdMesAsync(int mercadoId, int ano, int mes);

        Task<List<VendaSemanal>> GetByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter);

        Task<List<VendaSemanal>> GetByMercadoIdAnoAsync(int mercadoId, int ano);



        Task<List<VendaSemanal>> GetByEmpresaIdCachedListAsync(int empresaId);

        Task<List<VendaSemanal>> GetByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetByEmpresaIdMesAsync(int empresaId, int ano, int mes);

        Task<List<VendaSemanal>> GetByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetByEmpresaIdAnoAsync(int empresaId, int ano);



        Task<List<VendaSemanal>> GetByGrupolojaIdCachedListAsync(int grupolojaId);

        Task<List<VendaSemanal>> GetByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes);

        Task<List<VendaSemanal>> GetByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetByGrupolojaIdAnoAsync(int grupolojaId, int ano);



        Task<VendaSemanal> GetByIdAsync(int vendaSemanalId);


        //---------------------------------------------------------------------------------------------------

    }
}