using AutoMapper;
using Core.Entities.Reports;
using Core.Features.ReportTemplate.Commands.Create;
using Core.Features.ReportTemplate.Commands.Update;
using Core.Features.ReportTemplate.Response;

namespace Core.Mappings
{
    internal class ReportTemplateProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTemplateProfile()
        {
            CreateMap<CreateReportTemplateCommand, ReportTemplate>().ReverseMap();
            CreateMap<UpdateReportTemplateCommand, ReportTemplate>().ReverseMap();
            CreateMap<ReportTemplateCachedResponse, ReportTemplate>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}