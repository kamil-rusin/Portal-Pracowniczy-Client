using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;

namespace PPCAndroid.Shared.Domain
{
    public interface ISessionManager : IDisposable
    {
        void SaveUsername(string username);
        void SaveLogInDate(DateTime entryDate);
        void SaveLogOutDate(DateTime leavingDate);
        void SaveIsLogged(bool isLogged);
        void SaveAtWork(bool atWork);
        string GetUsername();
        DateTime GetLogInDate();
        DateTime GetLogOutDate();
        bool GetIsLoggedIn();
        bool GetIsAtWork();
        void LogOut();
    }
}