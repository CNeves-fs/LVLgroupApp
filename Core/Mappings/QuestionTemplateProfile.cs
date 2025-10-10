using AutoMapper;
using Core.Entities.Reports;
using Core.Features.QuestionTemplate.Commands.Create;
using Core.Features.QuestionTemplate.Commands.Update;
using Core.Features.QuestionTemplate.Response;

namespace Core.Mappings
{
    internal class QuestionTemplateProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateProfile()
        {
            CreateMap<CreateQuestionTemplateCommand, QuestionTemplate>().ReverseMap();
            CreateMap<UpdateQuestionTemplateCommand, QuestionTemplate>().ReverseMap();
            CreateMap<QuestionTemplateCachedResponse, QuestionTemplate>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}