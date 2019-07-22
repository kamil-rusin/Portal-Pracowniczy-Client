using System;
using Android.App;
using Android.Content;
using PPCAndroid;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class EnteredWorkReceiver : BroadcastReceiver
    {
        private SessionManager _sessionManager;
        
       public override void OnReceive(Context context, Intent intent)
       {
            _sessionManager = new SessionManager(context);
            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
            _sessionManager.SaveEntryDate(DateTime.Now);
            _sessionManager.SaveAtWork(true);
            //_sessionManager.Dispose();
       }
    }
}

