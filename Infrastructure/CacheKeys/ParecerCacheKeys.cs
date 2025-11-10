using Core.Entities.Claims;

namespace Infrastructure.CacheKeys
{
    public static class ParecerCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "PareceresList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "PareceresSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromClaimKey(int claimId) => $"PareceresFromClaimList-{claimId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromClaimKey(int claimId) => $"PareceresSelectListFromClaim-{claimId}";



        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int parecerId) => $"Parecer-{parecerId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int parecerId) => $"ParecerDetails-{parecerId}";


        //---------------------------------------------------------------------------------------------------

    }
}