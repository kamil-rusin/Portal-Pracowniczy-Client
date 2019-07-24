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

        public override void OnReceive(Context context, Intent intent)
       {
           //TODO: poprawić na eventy
            /*_workStorage = AndroidObjectFactory.GetWorkStorage(context);
            if (!(_workStorage.GetIsLoggedIn() & (!_workStorage.GetIsAtWork()))) return;
            _workStorage.SaveEntryDate(DateTime.Now);
            _workStorage.SaveAtWork(true);*/
       }
    }
}

