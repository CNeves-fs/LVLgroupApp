using AutoMapper;
using Core.Features.QuestionOption.Commands.Create;
using Core.Features.QuestionOption.Commands.Update;
using Core.Features.QuestionOption.Response;
using LVLgroupApp.Areas.Report.Models.QuestionOption;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class QuestionOptionProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionOptionProfile()
        {
            CreateMap<QuestionOptionCachedResponse, QuestionOptionViewModel>().ReverseMap();
            CreateMap<CreateQuestionOptionCommand, QuestionOptionViewModel>().ReverseMap();
            CreateMap<UpdateQuestionOptionCommand, QuestionOptionViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}