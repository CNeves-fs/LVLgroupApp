using AutoMapper;
using Core.Features.ReportTemplateQuestion.Commands.Create;
using Core.Features.ReportTemplateQuestion.Commands.Update;
using Core.Features.ReportTemplateQuestion.Response;
using LVLgroupApp.Areas.Report.Models.ReportTemplate;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class ReportTemplateQuestionProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTemplateQuestionProfile()
        {
            CreateMap<ReportTemplateQuestionCachedResponse, ReportTemplateQuestionViewModel>().ReverseMap();
            CreateMap<CreateReportTemplateQuestionCommand, ReportTemplateQuestionViewModel>().ReverseMap();
            CreateMap<UpdateReportTemplateQuestionCommand, ReportTemplateQuestionViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}