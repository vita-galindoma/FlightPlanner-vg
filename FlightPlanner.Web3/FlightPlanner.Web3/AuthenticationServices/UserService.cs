﻿namespace FlightPlanner.Web3.AuthenticationServices
{
    public class UserService : IUserService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("codelex-admin") && password.Equals("Password123");
        }
    }
}
