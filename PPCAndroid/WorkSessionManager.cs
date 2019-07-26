using System;
using System.Globalization;
using Android.Content;
using Android.Preferences;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid
{
    public class WorkSessionManager: IWorkStorage
    {
        private readonly ISharedPreferences _pref;
        private readonly ISharedPreferencesEditor _editor;
        
        private const string KeyLogInDate = "logInDate";
        private const string KeyLogOutDate = "logOutDate";
        private const string KeyIsAtWork = "isAtWork";
        
        public WorkSessionManager(Context context)
        {
            _pref = PreferenceManager.GetDefaultSharedPreferences(context);
            _editor = _pref.Edit();
        }
        
        public void Dispose()
        {
            _pref?.Dispose();
            _editor?.Dispose();
        }

        public void SaveEntryDate(DateTime entryDate)
        {
            _editor.Remove(KeyLogInDate);
            _editor.PutString(KeyLogInDate, entryDate.ToString(CultureInfo.InvariantCulture));
            _editor.Commit();
        }

        public void SaveLeavingDate(DateTime leavingDate)
        {
            _editor.Remove(KeyLogOutDate);
            _editor.PutString(KeyLogOutDate, leavingDate.ToString(CultureInfo.InvariantCulture));
            _editor.Commit();
        }

        public void SaveAtWork(bool atWork)
        {
            _editor.Remove(KeyIsAtWork);
            _editor.PutBoolean(KeyIsAtWork, atWork);
            _editor.Commit();
        }

        public DateTime GetEnteredWorkDate()
        {
            return DateTime.Parse(_pref.GetString(KeyLogInDate, "00:00:00"),CultureInfo.InvariantCulture);
        }

        public DateTime GetLeftWorkDate()
        {
            return DateTime.Parse(_pref.GetString(KeyLogOutDate, ""),CultureInfo.InvariantCulture);
        }

        public bool GetIsAtWork()
        {
            return _pref.GetBoolean(KeyIsAtWork, false);
        }

        public void RemoveWorkData()
        {
            _editor.Remove(KeyIsAtWork);
            _editor.Remove(KeyLogOutDate);
            _editor.Remove(KeyLogInDate);
            _editor.Commit();
        }
    }
}