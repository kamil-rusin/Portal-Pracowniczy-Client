using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;

namespace PPCAndroid.JobServices
{
    [Service]
    public class StartWorkIntentService : IntentService
    {
        private SessionManagerStorage _sessionManagerStorage;
        
        public StartWorkIntentService() : base("StartWorkIntentService")
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, AppConstants.ChannelId)
                .SetSmallIcon(Resource.Drawable.raports)
                .SetContentTitle("Aktualizuję dane")
                .SetContentText("Jesteś w pracy")
                .SetPriority(NotificationCompat.PriorityDefault);
            StartForeground(1337,builder.Build());
        }
        
        protected override void OnHandleIntent(Intent intent)
        {
            //TODO: change context
            _sessionManagerStorage = new SessionManagerStorage(ApplicationContext);
            if (!(_sessionManagerStorage.GetIsLoggedIn() & (!_sessionManagerStorage.GetIsAtWork()))) return;
            _sessionManagerStorage.SaveEntryDate(DateTime.Now);
            _sessionManagerStorage.SaveAtWork(true);
        }
    }
}