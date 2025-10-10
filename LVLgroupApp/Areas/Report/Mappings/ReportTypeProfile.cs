using AutoMapper;
using Core.Features.ReportTypes.Commands.Create;
using Core.Features.ReportTypes.Commands.Update;
using Core.Features.ReportTypes.Response;
using LVLgroupApp.Areas.Report.Models.ReportType;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class ReportTypeProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTypeProfile()
        {
            CreateMap<ReportTypeCachedResponse, ReportTypeViewModel>().ReverseMap();
            CreateMap<CreateReportTypeCommand, ReportTypeViewModel>().ReverseMap();
            CreateMap<UpdateReportTypeCommand, ReportTypeViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}