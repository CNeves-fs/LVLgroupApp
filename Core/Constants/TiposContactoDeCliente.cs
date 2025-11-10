using Core.Entities.Clientes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace Core.Constants
{
    public static class TiposContactoDeCliente
    {

        //---------------------------------------------------------------------------------------------------


        private static readonly List<TipoContacto> _TipoContactoList = new List<TipoContacto>()
        {
            new TipoContacto() { Id = 1, Tipo = "Em Loja" },
            new TipoContacto() { Id = 2, Tipo = "Por Telefone" },
            new TipoContacto() { Id = 3, Tipo = "Por Email" }
        };


        //---------------------------------------------------------------------------------------------------


        public static List<TipoContacto> TipoContactoList { get; } = _TipoContactoList;


        //---------------------------------------------------------------------------------------------------


        public static string GetTipoContacto(int Id)
        {
            if (Id > 0  && Id < _TipoContactoList.Count + 1) return _TipoContactoList[Id-1].Tipo;
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static int GetTipoContactoId(string Tipo)
        {
            var tipo = _TipoContactoList.Where(t => t.Tipo == Tipo).FirstOrDefault();
            if (tipo != null) return tipo.Id;
            return 1;
        }


        //---------------------------------------------------------------------------------------------------

    }
}