using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;

namespace PPCAndroid.Shared.Domain
{
    public interface ISessionManager : IDisposable
    {
        void SaveUsername(string username);
        void SaveEntryDate(DateTime entryDate);
        void SaveLeavingDate(DateTime leavingDate);
        void SaveIsLogged(bool isLogged);
        void SaveAtWork(bool atWork);
        string GetUsername();
        DateTime GetEnteredWorkDate();
        DateTime GetLeftWorkDate();
        bool GetIsLoggedIn();
        bool GetIsAtWork();
        void LogOut();
    }
}