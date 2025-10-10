using AutoMapper;
using Core.Features.ReportTypesLocalized.Commands.Create;
using Core.Features.ReportTypesLocalized.Commands.Update;
using Core.Features.ReportTypesLocalized.Response;
using LVLgroupApp.Areas.Report.Models.ReportType;

namespace LVLgroupApp.Areas.Report.Mappings
{
    internal class ReportTypeLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ReportTypeLocalizedProfile()
        {
            CreateMap<CreateReportTypeLocalizedCommand, ReportTypeLocalizedViewModel>().ReverseMap();
            CreateMap<UpdateReportTypeLocalizedCommand, ReportTypeLocalizedViewModel>().ReverseMap();
            CreateMap<ReportTypeLocalizedCachedResponse, ReportTypeLocalizedViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}