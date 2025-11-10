using AutoMapper;
using Core.Features.Ocorrencias.Commands.Create;
using Core.Features.Ocorrencias.Commands.Update;
using Core.Features.Ocorrencias.Response;
using LVLgroupApp.Areas.Ocorrencia.Models.Ocorrencia;

namespace LVLgroupApp.Areas.Ocorrencia.Mappings
{
    internal class OcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public OcorrenciaProfile()
        {
            CreateMap<OcorrenciaCachedResponse, OcorrenciaViewModel>().ReverseMap();
            CreateMap<CreateOcorrenciaCommand, OcorrenciaViewModel>().ReverseMap();
            CreateMap<UpdateOcorrenciaCommand, OcorrenciaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}