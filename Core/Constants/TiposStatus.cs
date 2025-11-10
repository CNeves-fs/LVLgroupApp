using Core.Entities.Claims;
using System.Collections.Generic;

namespace Core.Constants
{
    public static class TiposStatus
    {

        //---------------------------------------------------------------------------------------------------


        public static List<TipoStatus> TipoStatusList { get; } = new List<TipoStatus>()
        {
            new TipoStatus() { Id = 1, Tipo = "Pendente em Loja" },
            new TipoStatus() { Id = 2, Tipo = "Aguarda Validação" },
            new TipoStatus() { Id = 3, Tipo = "Aguarda Decisão" },
            new TipoStatus() { Id = 4, Tipo = "Aguarda Opinião Gerente de Loja" },
            new TipoStatus() { Id = 5, Tipo = "Aguarda Opinião Supervisor" },
            new TipoStatus() { Id = 6, Tipo = "Aguarda Opinião Revisor" },
            new TipoStatus() { Id = 7, Tipo = "Aguarda Opinião Fornecedor" },
            new TipoStatus() { Id = 8, Tipo = "Aceite" },
            new TipoStatus() { Id = 9, Tipo = "Não Aceite" },
            new TipoStatus() { Id = 10, Tipo = "Fechada em Loja Rejeitada" },
            new TipoStatus() { Id = 11, Tipo = "Fechada em Loja Troca Direta" },
            new TipoStatus() { Id = 12, Tipo = "Fechada em Loja Reparação Artigo" },
            new TipoStatus() { Id = 13, Tipo = "Fechada em Loja Troca Artigo" },
            new TipoStatus() { Id = 14, Tipo = "Fechada em Loja Devolução Dinheiro" },
            new TipoStatus() { Id = 15, Tipo = "Fechada em Loja Nota de Crédito" },
            new TipoStatus() { Id = 16, Tipo = "Aguarda Resposta Fornecedor" },
            new TipoStatus() { Id = 17, Tipo = "Aguarda Relatório" },
            new TipoStatus() { Id = 18, Tipo = "Relatório Disponível" }
        };

        public static int FirstClosedTipoStatus { get; } = 10;

        public static int LastClosedTipoStatus { get; } = 15;


        //---------------------------------------------------------------------------------------------------

    }
}