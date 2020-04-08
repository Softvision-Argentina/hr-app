namespace ApiServer.Contracts.User
{
    public class SuccessAuthData
    {
        public string Token { get; set; }
        public ReadedUserViewModel User { get; set; }
    }
}
