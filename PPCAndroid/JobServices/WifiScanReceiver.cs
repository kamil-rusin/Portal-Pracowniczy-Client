using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Support.V4.App;
using Java.Lang;
using PPCAndroid.Mappers;
using PPCAndroid.Shared.Domain;


namespace PPCAndroid.JobServices
{
    public class WifiScanReceiver : BroadcastReceiver
    {
        private IWorkStorage _sessionManagerWorkStorage;
        private bool _wifiLost;

        private readonly List<string> _availableSsids = new List<string>
        {
            //"AndroidWifi"
            "Xperia Z2_bf43"
        };

        private WifiManager WifiManager { get; set; }
        private List<WifiNetwork> WifiNetworks { get; set; }

        public WifiScanReceiver(WifiManager wifiManager)
        {
            _wifiLost = false;
            WifiManager = wifiManager;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManagerWorkStorage = new WorkSessionManager(context);
            var wifiFound = false;
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            WifiNetworks = WifiManager.ScanResults.ToDomainWifiNetworks().ToList();

            foreach (var availableSsid in _availableSsids)
            {
                var wifi = WifiNetworks.FirstOrDefault(n => n.Ssid == availableSsid);
                if (wifi == null)
                {
                    _wifiLost = true;
                    continue;
                }
                _wifiLost = false;
                if (_sessionManagerWorkStorage.GetIsAtWork()) continue;
                wifiFound = true;
                FoundNetworkNotify(context, wifi);
            }

            if (!wifiFound & _wifiLost)
            {
                if (_sessionManagerWorkStorage.GetIsAtWork())
                {
                    LostNetworkNotify(context);
                }
            }

            Task.Run(() =>
            {
                Thread.Sleep((long) TimeSpan.FromSeconds(5).TotalMilliseconds);
                WifiManager.StartScan();
            });
            
        }

        private static void LostNetworkNotify(Context context)
        {
            var notificationIntent = new Intent(context, typeof(LeftWorkReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            var builder = new NotificationCompat.Builder(context, AppConstants.ChannelId)
                .SetContentTitle("Utracono firmową sieć")
                .SetContentText("Kliknij, jeżeli wyszedłeś z pracy.")
                .SetSmallIcon(Resource.Drawable.raports)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetDefaults((int) NotificationDefaults.Sound | (int) NotificationDefaults.Vibrate)
                .SetPriority(NotificationCompat.PriorityHigh);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(AppConstants.NotificationIdLeftWork, builder.Build());
        }

        private static void FoundNetworkNotify(Context context, WifiNetwork wifi)
        {
            var startWorkReceiverIntent = new Intent(context, typeof(EnteredWorkReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(context, 0, startWorkReceiverIntent, PendingIntentFlags.UpdateCurrent);
            var builder = new NotificationCompat.Builder(context, AppConstants.ChannelId)
                .SetContentTitle("Wykryto sieć " + wifi.Ssid)
                .SetContentText("Kliknij, jeżeli jesteś w pracy.")
                .SetSmallIcon(Resource.Drawable.raports)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetDefaults((int) NotificationDefaults.Sound | (int) NotificationDefaults.Vibrate)
                .SetPriority(NotificationCompat.PriorityHigh);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(AppConstants.NotificationIdStartedWork, builder.Build());
        }
    }
}