using AutoMapper;
using Core.Entities.Reports;
using Core.Features.QuestionOption.Commands.Create;
using Core.Features.QuestionOption.Commands.Update;
using Core.Features.QuestionOption.Response;

namespace Core.Mappings
{
    internal class QuestionOptionProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public QuestionOptionProfile()
        {
            CreateMap<CreateQuestionOptionCommand, QuestionOption>().ReverseMap();
            CreateMap<UpdateQuestionOptionCommand, QuestionOption>().ReverseMap();
            CreateMap<QuestionOptionCachedResponse, QuestionOption>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}