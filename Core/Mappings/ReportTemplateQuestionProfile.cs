using AutoMapper;
using Core.Entities.Reports;
using Core.Features.ReportTemplateQuestion.Commands.Create;
using Core.Features.ReportTemplateQuestion.Commands.Update;
using Core.Features.ReportTemplateQuestion.Response;

namespace Core.Mappings
{
    internal class ReportTemplateQuestionProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTemplateQuestionProfile()
        {
            CreateMap<CreateReportTemplateQuestionCommand, ReportTemplateQuestion>().ReverseMap();
            CreateMap<UpdateReportTemplateQuestionCommand, ReportTemplateQuestion>().ReverseMap();
            CreateMap<ReportTemplateQuestionCachedResponse, ReportTemplateQuestion>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}