using System;
using Android.App;
using Android.Content;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class LeftWorkReceiver : BroadcastReceiver
    {
        private IWorkStorage _sessionManagerWorkStorage;
        
        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManagerWorkStorage = AndroidObjectFactory.GetWorkStorage(context);
            if (!(_sessionManagerWorkStorage.GetIsLoggedIn()) & _sessionManagerWorkStorage.GetIsAtWork()) return;
            _sessionManagerWorkStorage.SaveLeavingDate(DateTime.Now);
            _sessionManagerWorkStorage.SaveAtWork(false);
            //_sessionManager.Dispose();
        }
    }
}