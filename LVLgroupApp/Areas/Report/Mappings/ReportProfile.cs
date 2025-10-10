using AutoMapper;
using Core.Features.Report.Commands.Create;
using Core.Features.Report.Commands.Update;
using Core.Features.Report.Response;
using LVLgroupApp.Areas.Report.Models.Report;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class ReportProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportProfile()
        {
            CreateMap<ReportCachedResponse, ReportViewModel>().ReverseMap();
            CreateMap<CreateReportCommand, ReportViewModel>().ReverseMap();
            CreateMap<UpdateReportCommand, ReportViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}