using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class AtWorkIntentService : IntentService
    {

        public AtWorkIntentService() : base("AtWorkIntentService")
        {    
        }

        
        
        protected override void OnHandleIntent(Intent intent)
        {
//            _sessionManager = new SessionManager(BaseContext);
//            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
//            _sessionManager.SaveLogInDate(DateTime.Now);
//            _sessionManager.SaveAtWork(true);
        }
    }
}