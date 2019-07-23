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
        private IStorage _storage;

        public override void OnReceive(Context context, Intent intent)
       {
            _storage = AndroidObjectFactory.GetStorage(context);
            if (!(_storage.GetIsLoggedIn() & (!_storage.GetIsAtWork()))) return;
            _storage.SaveEntryDate(DateTime.Now);
            _storage.SaveAtWork(true);
       }
    }
}

