using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;
using Android.Util;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid
{
    public class SessionManager: ISessionManager
    {
        private readonly ISharedPreferences _pref;
        private readonly ISharedPreferencesEditor _editor;

        private const string KeyUsername = "username";
        private const string KeyLogInDate = "logInDate";
        private const string KeyLogOutDate = "logOutDate";
        private const string KeyIsLoggedIn = "isLoggedIn";
        private const string KeyIsAtWork = "isAtWork";

        public SessionManager(Context context)
        {
            _pref = PreferenceManager.GetDefaultSharedPreferences(context);
            _editor = _pref.Edit();
        }
        
        public void SaveUsername(string username)
        {
            _editor.Remove(KeyUsername);
            _editor.PutString(KeyUsername, username);
            Log.Info(KeyUsername, username);
            _editor.Commit();
        }
        
        public void SaveLogInDate(DateTime entryDate)
        {
            _editor.Remove(KeyLogInDate);
            _editor.PutString(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            Log.Info(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            _editor.Commit();
        }
        
        public void SaveLogOutDate(DateTime leavingDate)
        {
            _editor.Remove(KeyLogOutDate);
            _editor.PutString(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            Log.Info(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            _editor.Commit();
        }
        
        public void SaveIsLogged(bool isLogged)
        {
            _editor.Remove(KeyIsLoggedIn);
            _editor.PutBoolean(KeyIsLoggedIn, isLogged);
            Log.Info(KeyIsLoggedIn, isLogged.ToString());
            _editor.Commit();
        }
        
        public void SaveAtWork(bool atWork)
        {
            _editor.Remove(KeyIsAtWork);
            _editor.PutBoolean(KeyIsAtWork, atWork);
            Log.Info(KeyIsAtWork, atWork.ToString());
            _editor.Commit();
        }

        public string GetUsername()
        {
            var x = _pref.GetString(KeyUsername, "not_valid_user");
            Log.Info(KeyUsername, x);
            return x;
        }
        
        public DateTime GetEnteredWorkDate()
        {
            var dateInString = _pref.GetString(KeyLogInDate, "");
            Log.Info(KeyLogInDate, dateInString);
            return DateTime.Parse(dateInString);
        }
        
        public DateTime GetLeftWorkDate()
        {
            var dateInString = _pref.GetString(KeyLogOutDate, "");
            Log.Info(KeyLogOutDate, dateInString);
            return DateTime.Parse(dateInString);
        }

        public bool GetIsLoggedIn()
        {
            var x = _pref.GetBoolean(KeyIsLoggedIn, false);
            Log.Info(KeyIsLoggedIn, x.ToString());
            return x;
        }
        
        public bool GetIsAtWork()
        {
            var x = _pref.GetBoolean(KeyIsAtWork, false);
            Log.Info(KeyIsAtWork, x.ToString());
            return x;
        }

        public void LogOut()
        {
            _editor.Clear();
            _editor.Commit();
        }

        public void Dispose()
        {
            _pref?.Dispose();
            _editor?.Dispose();
        }
    }
}