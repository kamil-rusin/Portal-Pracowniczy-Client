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
        private ISharedPreferences pref;
        private ISharedPreferencesEditor editor;
        private Context _context;

        private int _privateMode = 0;
        private static string _prefName = "PPCAndroidPref";
        private static string KeyUsername = "username";
        private static string KeyLogInDate = "logInDate";
        private static string KeyLogOutDate = "logOutDate";
        private static string KeyIsLoggedIn = "isLoggedIn";
        private static string KeyIsAtWork = "isAtWork";
        
        public SessionManager(Context context){
            //_context = context;
            pref = PreferenceManager.GetDefaultSharedPreferences(context);
            editor = pref.Edit();
        }
        
        public void SaveUsername(string username)
        {
            editor.Remove(KeyUsername);
            editor.PutString(KeyUsername, username);
            Log.Info(KeyUsername, username);
            editor.Commit();
        }
        
        public void SaveLogInDate(DateTime entryDate)
        {
            editor.Remove(KeyLogInDate);
            editor.PutString(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            Log.Info(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            editor.Commit();
        }
        
        public void SaveLogOutDate(DateTime leavingDate)
        {
            editor.Remove(KeyLogOutDate);
            editor.PutString(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            Log.Info(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            editor.Commit();
        }
        
        public void SaveIsLogged(bool isLogged)
        {
            editor.Remove(KeyIsLoggedIn);
            editor.PutBoolean(KeyIsLoggedIn, isLogged);
            Log.Info(KeyIsLoggedIn, isLogged.ToString());
            editor.Commit();
        }
        
        public void SaveAtWork(bool atWork)
        {
            editor.Remove(KeyIsAtWork);
            editor.PutBoolean(KeyIsAtWork, atWork);
            Log.Info(KeyIsAtWork, atWork.ToString());
            editor.Commit();
        }

        public string GetUsername()
        {
            var x = pref.GetString(KeyUsername, "not_valid_user");
            Log.Info(KeyUsername, x);
            return x;
        }
        
        public DateTime GetLogInDate()
        {
            var dateInString = pref.GetString(KeyLogInDate, "");
            Log.Info(KeyLogInDate, dateInString);
            return DateTime.Parse(dateInString);
        }
        
        public DateTime GetLogOutDate()
        {
            var dateInString = pref.GetString(KeyLogOutDate, "");
            Log.Info(KeyLogOutDate, dateInString);
            return DateTime.Parse(dateInString);
        }

        public bool GetIsLoggedIn()
        {
            var x = pref.GetBoolean(KeyIsLoggedIn, false);
            Log.Info(KeyIsLoggedIn, x.ToString());
            return x;
        }
        
        public bool GetIsAtWork()
        {
            var x = pref.GetBoolean(KeyIsAtWork, false);
            Log.Info(KeyIsAtWork, x.ToString());
            return x;
        }

        public void LogOut()
        {
            editor.Clear();
            editor.Commit();
        }

        public void Dispose()
        {
            pref?.Dispose();
            editor?.Dispose();
            _context?.Dispose();
        }
    }
}