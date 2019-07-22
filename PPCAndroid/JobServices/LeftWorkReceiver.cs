using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class LeftWorkReceiver : BroadcastReceiver
    {
        private SessionManager _sessionManager;

        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManager = new SessionManager(Application.Context);
            if (!(_sessionManager.GetIsLoggedIn()) & _sessionManager.GetIsAtWork()) return;
            _sessionManager.SaveLeavingDate(DateTime.Now);
            _sessionManager.SaveAtWork(false);
            //_sessionManager.Dispose();
        }
    }
}