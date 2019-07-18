using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class StartWorkIntentService : IntentService
    {
        private SessionManager _sessionManager;
        
        public StartWorkIntentService() : base("StartWorkIntentService")
        {
            SetIntentRedelivery(true);
        }
        
        protected override void OnHandleIntent(Intent intent)
        {
            //TODO: change context
            _sessionManager = new SessionManager(ApplicationContext);
            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
            _sessionManager.SaveLogInDate(DateTime.Now);
            _sessionManager.SaveAtWork(true);
        }
    }
}