using AutoMapper;
using Core.Features.Clientes.Commands.Create;
using Core.Features.Clientes.Commands.Update;
using Core.Features.Clientes.Queries.GetByEmail;
using Core.Features.Clientes.Queries.GetById;
using Core.Features.Clientes.Queries.GetByNIF;
using Core.Features.Clientes.Queries.GetByNome;
using Core.Features.Clientes.Queries.GetByTelefone;
using Core.Features.Clientes.Response;
using LVLgroupApp.Areas.Cliente.Models.Cliente;

namespace LVLgroupApp.Areas.Cliente.Mappings
{
    internal class ClienteProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ClienteProfile()
        {
            CreateMap<ClienteCachedResponse, ClienteViewModel>().ReverseMap();
            CreateMap<CreateClienteCommand, ClienteViewModel>().ReverseMap();
            CreateMap<UpdateClienteCommand, ClienteViewModel>().ReverseMap();

            CreateMap<ClienteCachedResponse, TabClienteViewModel>().ReverseMap();
            CreateMap<CreateClienteCommand, TabClienteViewModel>().ReverseMap();
            CreateMap<UpdateClienteCommand, TabClienteViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}