using AutoMapper;
using Core.Entities.Reports;
using Core.Features.Report.Commands.Create;
using Core.Features.Report.Commands.Update;
using Core.Features.Report.Response;

namespace Core.Mappings
{
    internal class ReportProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportProfile()
        {
            CreateMap<CreateReportCommand, Report>().ReverseMap();
            CreateMap<UpdateReportCommand, Report>().ReverseMap();
            CreateMap<ReportCachedResponse, Report>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}