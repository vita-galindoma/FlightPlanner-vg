namespace FlightPlanner.Web3.AuthenticationServices
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}
