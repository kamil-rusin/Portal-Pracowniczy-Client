using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;

namespace PPCAndroid.JobServices
{
    [Service]
    public class StartWorkIntentService : IntentService
    {
        private SessionManager _sessionManager;
        
        public StartWorkIntentService() : base("StartWorkIntentService")
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, AppConstant.ChannelId)
                .SetSmallIcon(Resource.Drawable.raports)
                .SetContentTitle("Aktualizuję dane")
                .SetContentText("Jesteś w pracy")
                .SetPriority(NotificationCompat.PriorityDefault);
            StartForeground(1337,builder.Build());
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