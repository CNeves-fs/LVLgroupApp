using Core.Entities.Vendas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IVendaDiariaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<VendaDiaria>> GetCachedListAsync();







        Task<List<VendaDiaria>> GetByLojaIdAsync(int lojaId);

        Task<List<VendaDiaria>> GetByLojaIdAnoAsync(int lojaId, int ano);

        Task<List<VendaDiaria>> GetByLojaIdQuarterAsync(int lojaId, int ano, int quarter);

        Task<List<VendaDiaria>> GetByLojaIdMesAsync(int lojaId, int ano, int mes);

        Task<VendaDiaria> GetByLojaIdDataAsync(int lojaId, int ano, int mês, int diaDoMês);

        Task<List<VendaDiaria>> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana);

        

        

        



        Task<List<VendaDiaria>> GetByMercadoIdAsync(int mercadoId);

        Task<List<VendaDiaria>> GetByMercadoIdAnoAsync(int mercadoId, int ano);

        Task<List<VendaDiaria>> GetByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter);

        Task<List<VendaDiaria>> GetByMercadoIdMesAsync(int mercadoId, int ano, int mes);

        Task<List<VendaDiaria>> GetByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana);

        

        

        



        Task<List<VendaDiaria>> GetByEmpresaIdAsync(int empresaId);

        Task<List<VendaDiaria>> GetByEmpresaIdAnoAsync(int empresaId, int ano);

        Task<List<VendaDiaria>> GetByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter);

        Task<List<VendaDiaria>> GetByEmpresaIdMesAsync(int empresaId, int ano, int mes);

        Task<List<VendaDiaria>> GetByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana);

        

        

        



        Task<List<VendaDiaria>> GetByGrupolojaIdAsync(int grupolojaId);

        Task<List<VendaDiaria>> GetByGrupolojaIdAnoAsync(int grupolojaId, int ano);

        Task<List<VendaDiaria>> GetByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter);

        Task<List<VendaDiaria>> GetByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes);

        Task<List<VendaDiaria>> GetByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana);









        Task<List<VendaDiaria>> GetByMesAsync(int ano, int mês);

        Task<List<VendaDiaria>> GetByTrimestreAsync(int ano, int trimestre);

        Task<List<VendaDiaria>> GetByAnoAsync(int ano);

        Task<List<VendaDiaria>> GetByDiaAsync(int ano, int mês, int dia);

        Task<List<VendaDiaria>> GetBySemanaAsync(int ano, int numeroDaSemana);

        Task<List<VendaDiaria>> GetByVendaSemanalIdAsync(int vendaSemanalId);

        Task<VendaDiaria> GetByVendaSemanalIdDiaAsync(int vendaSemanalId, int diaDaSemana);

        Task<VendaDiaria> GetByIdAsync(int vendaDiariaId);


        //---------------------------------------------------------------------------------------------------

    }
}