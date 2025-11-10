using AspNetCoreHero.Results;
using DataTables.AspNet.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {

        //---------------------------------------------------------------------------------------------------


        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - dt.DayOfWeek;
            return dt.AddDays(diff).Date;
        }


        //---------------------------------------------------------------------------------------------------


        public static DateTime MondayOfWeek(this DateTime dt)
        {
            int delta = DayOfWeek.Monday - dt.DayOfWeek;
            if (delta > 0) delta -= 7;
            DateTime monday = dt.AddDays(delta);
            return monday.Date;
        }


        //---------------------------------------------------------------------------------------------------

    }
}