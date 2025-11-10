using AutoMapper;
using Core.Entities.Reports;
using Core.Features.ReportTypes.Commands.Create;
using Core.Features.ReportTypes.Commands.Update;
using Core.Features.ReportTypes.Response;

namespace Core.Mappings
{
    internal class ReportTypeProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTypeProfile()
        {
            CreateMap<CreateReportTypeCommand, ReportType>().ReverseMap();
            CreateMap<UpdateReportTypeCommand, ReportType>().ReverseMap();
            CreateMap<ReportTypeCachedResponse, ReportType>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}