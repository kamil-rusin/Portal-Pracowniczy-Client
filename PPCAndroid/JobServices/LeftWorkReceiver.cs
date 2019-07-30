using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using PPCAndroid.Domain;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class LeftWorkReceiver : BroadcastReceiver
    {
        private IWorkStorage _workStorage;
        private IUserStorage _userStorage;
        private IEventService _eventService;
        
        public override void OnReceive(Context context, Intent intent)
        {
            _userStorage = AndroidObjectFactory.GetUserStorage(context);
            _workStorage = AndroidObjectFactory.GetWorkStorage(context);
            _eventService = AndroidObjectFactory.GetEventService();
            if (!(_userStorage.GetIsLoggedIn()) & _workStorage.GetIsAtWork()) return;
            _workStorage.SaveLeavingDate(DateTime.Now);
            _workStorage.SaveAtWork(false);
            
            _eventService.Add(new EndWorkEvent(DateTime.Now));
            
           SummaryNotify(context);
        }

        private void SummaryNotify(Context context)
        {
            var summaryActivityIntent = new Intent(context, typeof(SummaryActivity));
            var pendingIntent = PendingIntent.GetActivity(context, 0, summaryActivityIntent, 0);
            var builder = new NotificationCompat.Builder(context, AppConstants.ChannelId)
                .SetContentTitle("Koniec pracy!")
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(BuildNotificationText()))
                .SetContentText(BuildNotificationText())
                .SetSmallIcon(Resource.Drawable.raports)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetDefaults((int) NotificationDefaults.Vibrate)
                .SetPriority(NotificationCompat.PriorityHigh);
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(AppConstants.NotificationIdAlreadyLeftWork, builder.Build());
        }

        private string BuildNotificationText()
        {
            var x = EventsCounter.CountWorkExits(_eventService,DateTime.Now);
            return x > 0 ? $"Dzisiaj pracowałeś: {EventsCounter.CountWorkTime(_eventService.GetAll(),DateTime.Now):hh\\:mm\\:ss}. W czasie pracy wyszedłeś {x} raz(y)" : $"Dzisiaj pracowałeś: {EventsCounter.CountWorkTime(_eventService.GetAll(),DateTime.Now):hh\\:mm\\:ss}";
        }
    }
}