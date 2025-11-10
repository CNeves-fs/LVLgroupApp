using Core.Entities.Vendas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IVendaSemanalRepository
    {

        //---------------------------------------------------------------------------------------------------

        IQueryable<VendaSemanal> VendasSemanais { get; }

        Task<List<VendaSemanal>> GetListAsync();

        Task<List<VendaSemanal>> GetListByAnoAsync(int ano);

        Task<List<VendaSemanal>> GetListBySemanaAsync(int ano, int numeroDaSemana);



        Task<List<VendaSemanal>> GetListFromLojaIdAsync(int lojaId);
        
        Task<VendaSemanal> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetListByLojaIdMesAsync(int lojaId, int ano, int mes);

        Task<List<VendaSemanal>> GetListByLojaIdQuarterAsync(int lojaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetListByLojaIdAnoAsync(int lojaId, int ano);



        Task<List<VendaSemanal>> GetListByMercadoIdCachedListAsync(int mercadoId);

        Task<List<VendaSemanal>> GetListByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetListByMercadoIdMesAsync(int mercadoId, int ano, int mes);

        Task<List<VendaSemanal>> GetListByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter);

        Task<List<VendaSemanal>> GetListByMercadoIdAnoAsync(int mercadoId, int ano);



        Task<List<VendaSemanal>> GetListByEmpresaIdCachedListAsync(int empresaId);

        Task<List<VendaSemanal>> GetListByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetListByEmpresaIdMesAsync(int empresaId, int ano, int mes);

        Task<List<VendaSemanal>> GetListByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetListByEmpresaIdAnoAsync(int empresaId, int ano);



        Task<List<VendaSemanal>> GetListByGrupolojaIdCachedListAsync(int grupolojaId);

        Task<List<VendaSemanal>> GetListByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana);

        Task<List<VendaSemanal>> GetListByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes);

        Task<List<VendaSemanal>> GetListByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter);

        Task<List<VendaSemanal>> GetListByGrupolojaIdAnoAsync(int grupolojaId, int ano);



        Task<VendaSemanal> GetByIdAsync(int vendaSemanalId);

        Task<int> InsertAsync(VendaSemanal vendaSemanal);

        Task UpdateAsync(VendaSemanal vendaSemanal);

        Task DeleteAsync(VendaSemanal vendaSemanal);


        //---------------------------------------------------------------------------------------------------

    }
}