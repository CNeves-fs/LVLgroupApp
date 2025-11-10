using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Features.OcorrenciaDocuments.Commands.Create;
using Core.Features.OcorrenciaDocuments.Commands.Update;
using Core.Features.OcorrenciaDocuments.Response;

namespace Core.Mappings
{
    internal class OcorrenciaDocumentProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public OcorrenciaDocumentProfile()
        {
            CreateMap<CreateOcorrenciaDocumentCommand, OcorrenciaDocument>().ReverseMap();
            CreateMap<UpdateOcorrenciaDocumentCommand, OcorrenciaDocument>().ReverseMap();
            CreateMap<OcorrenciaDocumentCachedResponse, OcorrenciaDocument>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}