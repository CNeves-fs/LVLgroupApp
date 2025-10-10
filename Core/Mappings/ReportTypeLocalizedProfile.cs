using AutoMapper;
using Core.Entities.Reports;
using Core.Features.ReportTypesLocalized.Commands.Create;
using Core.Features.ReportTypesLocalized.Commands.Update;
using Core.Features.ReportTypesLocalized.Response;

namespace Core.Mappings
{
    internal class ReportTypeLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTypeLocalizedProfile()
        {
            CreateMap<CreateReportTypeLocalizedCommand, ReportTypeLocalized>().ReverseMap();
            CreateMap<UpdateReportTypeLocalizedCommand, ReportTypeLocalized>().ReverseMap();
            CreateMap<ReportTypeLocalizedCachedResponse, ReportTypeLocalized>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}