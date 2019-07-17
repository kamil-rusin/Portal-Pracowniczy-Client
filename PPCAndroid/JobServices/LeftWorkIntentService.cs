using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class LeftWorkIntentService : IntentService
    {
        private SessionManager _sessionManager;

        public LeftWorkIntentService() : base("LeftWorkIntentService")
        {
            _sessionManager = new SessionManager(Application.Context);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!(_sessionManager.GetIsLoggedIn()) & _sessionManager.GetIsAtWork()) return;
            _sessionManager.SaveLogOutDate(DateTime.Now);
            _sessionManager.SaveAtWork(false);
            _sessionManager.Dispose();
        }
    }
}