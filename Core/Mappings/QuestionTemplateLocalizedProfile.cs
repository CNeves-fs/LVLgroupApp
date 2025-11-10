using AutoMapper;
using Core.Entities.Reports;
using Core.Features.QuestionTemplateLocalized.Commands.Create;
using Core.Features.QuestionTemplateLocalized.Commands.Update;
using Core.Features.QuestionTemplateLocalized.Response;

namespace Core.Mappings
{
    internal class QuestionTemplateLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateLocalizedProfile()
        {
            CreateMap<CreateQuestionTemplateLocalizedCommand, QuestionTemplateLocalized>().ReverseMap();
            CreateMap<UpdateQuestionTemplateLocalizedCommand, QuestionTemplateLocalized>().ReverseMap();
            CreateMap<QuestionTemplateLocalizedCachedResponse, QuestionTemplateLocalized>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}