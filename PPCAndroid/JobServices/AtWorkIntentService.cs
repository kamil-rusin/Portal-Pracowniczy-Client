using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class AtWorkIntentService : IntentService
    {
        private SessionManager _sessionManager;

        public AtWorkIntentService() : base("AtWorkIntentService")
        {
            _sessionManager = new SessionManager(Application.Context);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
            _sessionManager.SaveLogInDate(DateTime.Now);
            _sessionManager.SaveAtWork(true);
            _sessionManager.Dispose();
        }
    }
}