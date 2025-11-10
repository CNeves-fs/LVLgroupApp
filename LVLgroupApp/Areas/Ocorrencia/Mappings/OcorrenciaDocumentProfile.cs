using AutoMapper;
using Core.Features.OcorrenciaDocuments.Commands.Create;
using Core.Features.OcorrenciaDocuments.Commands.Update;
using Core.Features.OcorrenciaDocuments.Response;
using LVLgroupApp.Areas.Ocorrencia.Models.OcorrenciaDocument;

namespace LVLgroupApp.Areas.Ocorrencia.Mappings
{
    internal class OcorrenciaDocumentProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public OcorrenciaDocumentProfile()
        {
            CreateMap<OcorrenciaDocumentCachedResponse, OcorrenciaDocumentViewModel>().ReverseMap();
            CreateMap<CreateOcorrenciaDocumentCommand, OcorrenciaDocumentViewModel>().ReverseMap();
            CreateMap<UpdateOcorrenciaDocumentCommand, OcorrenciaDocumentViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}