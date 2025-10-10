using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class Foto : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string TempFolderGuid { get; set; }     // se null => file in Claim.CodeId folder

        [StringLength(255)]
        public string Path { get; set; }                // path no server /wwwroot/Claims/20221022-GEX-LLLL-XXXX/filename

        public int? FototagId { get; set; }

        [StringLength(128)]
        public string Descrição { get; set; }

        public int? ClaimId { get; set; }      // null => Claim a ser criada ainda não tem Id



        public virtual Claim Claim { get; set; }

        public virtual Fototag Fototag { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
