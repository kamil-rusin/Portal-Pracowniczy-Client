using System;
using Android.App;
using Android.Content;
using PPCAndroid;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class EnteredWorkReceiver : BroadcastReceiver
    {
        private IWorkStorage _workStorage;
        private IUserStorage _userStorage;

        public override void OnReceive(Context context, Intent intent)
       {
           //TODO: poprawić na eventy
            _workStorage = AndroidObjectFactory.GetWorkStorage(context);
            _userStorage = AndroidObjectFactory.GetUserStorage(context);
            if (!(_userStorage.GetIsLoggedIn() & (!_workStorage.GetIsAtWork()))) return;
            _workStorage.SaveEntryDate(DateTime.Now);
            _workStorage.SaveAtWork(true);
       }
    }
}

