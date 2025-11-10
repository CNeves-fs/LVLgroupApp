using System;

namespace Core.Features.Clientes.Response
{
    public class ClienteCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string NIF { get; set; }

        public string Telefone { get; set; }

        public string IBAN { get; set; }

        public string Morada { get; set; }

        public DateTime? DataUltimoContacto { get; set; }

        public string TipoContacto { get; set; }

        public string DescriçãoContacto { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}