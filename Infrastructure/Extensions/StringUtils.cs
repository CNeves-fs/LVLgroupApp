

using Core.Entities.Clientes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class StringUtils
    {

        //---------------------------------------------------------------------------------------------------


        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }



        //---------------------------------------------------------------------------------------------------


        public static string CleanTelefone(this string input)
        {
            var str = new string(input.Where(x => char.IsDigit(x) || x == '+').ToArray());
            return str;
        }


        //---------------------------------------------------------------------------------------------------


        public static string FormatTelefone(this string input)
        {

            var str = new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

            switch(str.Length)
                {
                    case 13:
                        return String.Format("{0} {1} {2} {3}"
                        , str.Substring(0, 4)
                        , str.Substring(4, 3)
                        , str.Substring(7, 3)
                        , str.Substring(10));
                    case 12:
                        return String.Format("{0} {1} {2} {3}"
                        , str.Substring(0, 3)
                        , str.Substring(3, 3)
                        , str.Substring(6, 3)
                        , str.Substring(9));
                    case 9:
                        return String.Format("{0} {1} {2}"
                        , str.Substring(0, 3)
                        , str.Substring(3, 3)
                        , str.Substring(6));
                default:
                    return str;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}