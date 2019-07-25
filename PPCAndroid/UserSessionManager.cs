using Android.Content;
using Android.Preferences;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid
{
    public class UserSessionManager : IUserStorage
    {
        private readonly ISharedPreferences _pref;
        private readonly ISharedPreferencesEditor _editor;

        private const string KeyUsername = "username";
        private const string KeyIsLoggedIn = "isLoggedIn";

        public UserSessionManager(Context context)
        {
            _pref = PreferenceManager.GetDefaultSharedPreferences(context);
            _editor = _pref.Edit();
        }
        
        public void SaveUsername(string username)
        {
            _editor.Remove(KeyUsername);
            _editor.PutString(KeyUsername, username);
            _editor.Commit();
        }

        public void SaveIsLogged(bool isLogged)
        {
            _editor.Remove(KeyIsLoggedIn);
            _editor.PutBoolean(KeyIsLoggedIn, isLogged);
            _editor.Commit();
        }

        public void ClearUsernameAndIsLoggedIn()
        {
            _editor.Remove(KeyUsername);
            _editor.Remove(KeyIsLoggedIn);
            _editor.Commit();
        }

        public bool GetIsLoggedIn()
        {
            return _pref.GetBoolean(KeyIsLoggedIn, false);
        }
        
        public string GetUsername()
        {
            return  _pref.GetString(KeyUsername, "not_valid_user");
        }

        public void Dispose()
        {
            _pref?.Dispose();
            _editor?.Dispose();
        }
    }
}