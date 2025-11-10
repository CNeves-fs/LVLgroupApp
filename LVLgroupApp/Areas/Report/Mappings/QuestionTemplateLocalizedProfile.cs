using AutoMapper;
using Core.Features.QuestionTemplateLocalized.Commands.Create;
using Core.Features.QuestionTemplateLocalized.Commands.Update;
using Core.Features.QuestionTemplateLocalized.Response;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class QuestionTemplateLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateLocalizedProfile()
        {
            CreateMap<QuestionTemplateLocalizedCachedResponse, QuestionTemplateLocalizedViewModel>().ReverseMap();
            CreateMap<CreateQuestionTemplateLocalizedCommand, QuestionTemplateLocalizedViewModel>().ReverseMap();
            CreateMap<UpdateQuestionTemplateLocalizedCommand, QuestionTemplateLocalizedViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}