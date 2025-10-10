using System.ComponentModel.DataAnnotations;

namespace Core.Features.Fotos.Response
{
    public class FotoCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string FileName { get; set; }

        public string ClaimFolder { get; set; }     // se null => file in Claim.CodeId folder

        public string Path { get; set; }                // path no server /wwwroot/Claims/20221022-GEX-LLLL-XXXX/filename

        public int? FototagId { get; set; }

        public string Descrição { get; set; }

        public int? ClaimId { get; set; }          // null => Claim a ser criada ainda não tem Id


        //---------------------------------------------------------------------------------------------------

    }
}