using AutoMapper;
using Core.Features.QuestionTemplate.Commands.Create;
using Core.Features.QuestionTemplate.Commands.Update;
using Core.Features.QuestionTemplate.Response;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class QuestionTemplateProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateProfile()
        {
            CreateMap<QuestionTemplateCachedResponse, QuestionTemplateViewModel>().ReverseMap();
            CreateMap<CreateQuestionTemplateCommand, QuestionTemplateViewModel>().ReverseMap();
            CreateMap<UpdateQuestionTemplateCommand, QuestionTemplateViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}