using Core.Entities.Business;
using System;

namespace Core.Features.Pareceres.Response
{
    public class ParecerCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Email { get; set; }

        public string Opinião { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}