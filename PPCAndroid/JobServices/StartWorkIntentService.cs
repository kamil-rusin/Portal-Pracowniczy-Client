using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;

namespace PPCAndroid.JobServices
{/*
    [Service]
    public class StartWorkIntentService : IntentService
    {
        private WorkSessionManager _workSessionManager;
        
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
            _workSessionManager = new WorkSessionManager(ApplicationContext);
            if (!(_workSessionManager.GetIsLoggedIn() & (!_workSessionManager.GetIsAtWork()))) return;
            _workSessionManager.SaveEntryDate(DateTime.Now);
            _workSessionManager.SaveAtWork(true);
        
    }*/
}