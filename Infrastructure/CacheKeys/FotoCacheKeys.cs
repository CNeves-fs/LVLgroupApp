namespace Infrastructure.CacheKeys
{
    public static class FotoCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "FotosList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromClaimKey(int claimId) => $"FotosFromClaimList-{claimId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromAllTempFolderKey => $"FotosFromAllTempFolderList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTempFolderKey(string tempFolder) => $"FotosFromTempFolderList-{tempFolder}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int fotoId) => $"Foto-{fotoId}";


        //---------------------------------------------------------------------------------------------------

    }
}