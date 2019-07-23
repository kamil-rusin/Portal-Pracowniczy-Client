using System;
using Android.App;
using Android.Content;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class LeftWorkReceiver : BroadcastReceiver
    {
        private IStorage _sessionManagerStorage;
        
        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManagerStorage = AndroidObjectFactory.GetStorage(context);
            if (!(_sessionManagerStorage.GetIsLoggedIn()) & _sessionManagerStorage.GetIsAtWork()) return;
            _sessionManagerStorage.SaveLeavingDate(DateTime.Now);
            _sessionManagerStorage.SaveAtWork(false);
            //_sessionManager.Dispose();
        }
    }
}