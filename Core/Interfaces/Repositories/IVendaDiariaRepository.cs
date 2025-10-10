using Core.Entities.Vendas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IVendaDiariaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<VendaDiaria> VendasDiarias { get; }


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListAsync();


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByMercadoIdCachedListAsync(int mercadoId);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByMercadoIdAnoAsync(int mercadoId, int ano);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByMercadoIdMesAsync(int mercadoId, int ano, int mes);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByEmpresaIdCachedListAsync(int empresaId);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByEmpresaIdAnoAsync(int empresaId, int ano);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByEmpresaIdMesAsync(int empresaId, int ano, int mes);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByGrupolojaIdCachedListAsync(int grupolojaId);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByGrupolojaIdAnoAsync(int grupolojaId, int ano);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromLojaIdAsync(int lojaId);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByLojaIdAnoAsync(int lojaId, int ano);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListByLojaIdMesAsync(int lojaId, int ano, int mes);


        //---------------------------------------------------------------------------------------------------


        Task<VendaDiaria> GetByLojaIdDataAsync(int lojaId, int ano, int mês, int diaDoMês);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromAnoAsync(int ano);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromTrimestreAsync(int ano, int trimestre);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromMesAsync(int ano, int mes);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromSemanaAsync(int ano, int semana);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromDiaAsync(int ano, int mes, int dia);


        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetListFromVendaSemanalIdAsync(int vendaSemanalId);


        //---------------------------------------------------------------------------------------------------


        Task<VendaDiaria> GetByVendaSemanalIdDiaAsync(int vendaSemanalId, int diaDaSemana);


        //---------------------------------------------------------------------------------------------------


        Task<VendaDiaria> GetByIdAsync(int vendaDiariaId);


        //---------------------------------------------------------------------------------------------------


        Task<int> InsertAsync(VendaDiaria vendaDiaria);


        //---------------------------------------------------------------------------------------------------


        Task UpdateAsync(VendaDiaria vendaDiaria);


        //---------------------------------------------------------------------------------------------------


        Task DeleteAsync(VendaDiaria vendaDiaria);


        //---------------------------------------------------------------------------------------------------

    }
}