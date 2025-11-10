using Core.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class Parecer
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Email { get; set; }

        public string Opinião { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}