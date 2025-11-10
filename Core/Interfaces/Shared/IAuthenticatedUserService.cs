namespace Core.Interfaces.Shared
{
    public interface IAuthenticatedUserService
    {

        //---------------------------------------------------------------------------------------------------


        string UserId { get; }
        public string Username { get; }
        public string UserEmail { get; }


        //---------------------------------------------------------------------------------------------------

    }
}