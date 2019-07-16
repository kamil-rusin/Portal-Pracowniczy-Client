using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;

namespace PPCAndroid
{
    public class SessionManager
    {
        //TODO: remember to dispose variables in functions
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
            _context = context;
            pref = PreferenceManager.GetDefaultSharedPreferences(_context);
            editor = pref.Edit();
        }

        public void SaveUsername(string username)
        {
            editor.Remove(KeyUsername);
            editor.PutString(KeyUsername, username);
            editor.Commit();
        }
        
        public void SaveLogInDate(DateTime entryDate)
        {
            editor.Remove(KeyLogInDate);
            editor.PutString(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            editor.Commit();
        }
        
        public void SaveLogOutDate(DateTime leavingDate)
        {
            editor.Remove(KeyLogOutDate);
            editor.PutString(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            editor.Commit();
        }
        
        public void SaveIsLogged(bool isLogged)
        {
            editor.Remove(KeyIsLoggedIn);
            editor.PutBoolean(KeyIsLoggedIn, isLogged);
            editor.Commit();
        }
        
        public void SaveAtWork(bool atWork)
        {
            editor.Remove(KeyIsAtWork);
            editor.PutBoolean(KeyIsAtWork, atWork);
            editor.Commit();
        }

        public string GetUsername()
        {
            return pref.GetString(KeyUsername, "not_valid_user");
        }
        
        public DateTime GetLogInDate()
        {
            var dateInString = pref.GetString(KeyLogInDate, "");
            return DateTime.Parse(dateInString);
        }
        
        public DateTime GetLogOutDate()
        {
            var dateInString = pref.GetString(KeyLogOutDate, "");
            return DateTime.Parse(dateInString);
        }

        public bool GetIsLoggedIn()
        {
            return pref.GetBoolean(KeyIsLoggedIn, false);
        }
        
        public bool GetIsAtWork()
        {
            return pref.GetBoolean(KeyIsAtWork, false);
        }

        public void LogOut()
        {
            editor.Clear();
            editor.Commit();
        }
    }
}