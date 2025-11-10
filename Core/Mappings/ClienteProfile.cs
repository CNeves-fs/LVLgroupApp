using AutoMapper;
using Core.Entities.Clientes;
using Core.Features.Clientes.Commands.Create;
using Core.Features.Clientes.Queries.GetByEmail;
using Core.Features.Clientes.Queries.GetById;
using Core.Features.Clientes.Queries.GetByNIF;
using Core.Features.Clientes.Queries.GetByNome;
using Core.Features.Clientes.Queries.GetByTelefone;
using Core.Features.Clientes.Response;

namespace Core.Mappings
{
    internal class ClienteProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ClienteProfile()
        {
            CreateMap<CreateClienteCommand, Cliente>().ReverseMap();
            CreateMap<ClienteCachedResponse, Cliente>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}