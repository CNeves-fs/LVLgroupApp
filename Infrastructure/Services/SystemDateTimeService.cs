using Core.Interfaces.Shared;
using System;

namespace Infrastructure.Services
{
    public class SystemDateTimeService : IDateTimeService
    {

        //---------------------------------------------------------------------------------------------------


        //public DateTime NowUtc => DateTime.UtcNow;
        public DateTime NowUtc => DateTime.Now;


        //---------------------------------------------------------------------------------------------------

    }
}