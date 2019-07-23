using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;

namespace PPCAndroid.Shared.Domain
{
    public interface IStorage : IDisposable
    {
        void SaveEntryDate(DateTime entryDate);
        void SaveLeavingDate(DateTime leavingDate);
        void SaveAtWork(bool atWork);
        DateTime GetEnteredWorkDate();
        DateTime GetLeftWorkDate();
        bool GetIsAtWork();
        
     
        
        void SaveUsername(string username);
        void SaveIsLogged(bool isLogged);
        bool GetIsLoggedIn();
        void LogOut();
        string GetUsername();
    }
}