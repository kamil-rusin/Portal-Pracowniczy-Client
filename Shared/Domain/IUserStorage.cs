using System;

namespace PPCAndroid.Shared.Domain
{
    public interface IUserStorage : IDisposable
    {
        void SaveUsername(string username);
        void SaveIsLogged(bool isLogged);
        bool GetIsLoggedIn();
        void ClearUsernameAndIsLoggedIn();
        string GetUsername();
    }
}