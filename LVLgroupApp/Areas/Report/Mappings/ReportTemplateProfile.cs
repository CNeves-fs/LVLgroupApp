using AutoMapper;
using Core.Features.ReportTemplate.Commands.Create;
using Core.Features.ReportTemplate.Commands.Update;
using Core.Features.ReportTemplate.Response;
using LVLgroupApp.Areas.Report.Models.ReportTemplate;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class ReportTemplateProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTemplateProfile()
        {
            CreateMap<ReportTemplateCachedResponse, ReportTemplateViewModel>().ReverseMap();
            CreateMap<CreateReportTemplateCommand, ReportTemplateViewModel>().ReverseMap();
            CreateMap<UpdateReportTemplateCommand, ReportTemplateViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}