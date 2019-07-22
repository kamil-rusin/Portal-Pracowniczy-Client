using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Support.V4.App;
using Android.Util;
using Java.Lang;
using PPCAndroid.Mappers;
using PPCAndroid.Shared.Domain;


namespace PPCAndroid.JobServices
{
    public class WifiScanReceiver : BroadcastReceiver
    {
        private SessionManager _sessionManager;
        private bool _wifiLost;


        private readonly List<string> _availableSsids = new List<string>
        {
            "AndroidWifi"
        };

        private WifiManager WifiManager { get; set; }
        private readonly Subject<IEnumerable<WifiNetwork>> _wiFiNetworksSubject;

        private List<WifiNetwork> WifiNetworks { get; set; }
        public IObservable<IEnumerable<WifiNetwork>> WiFiNetworksObs => _wiFiNetworksSubject;
        
        public WifiScanReceiver(WifiManager wifiManager)
        {
            _wifiLost = false;
            WifiManager = wifiManager;
            _wiFiNetworksSubject = new Subject<IEnumerable<WifiNetwork>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManager = new SessionManager(context);
            var wifiFound = false;
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            WifiNetworks = WifiManager.ScanResults.ToDomainWifiNetworks().ToList();
            _wiFiNetworksSubject.OnNext(WifiNetworks);

            foreach (var availableSsid in _availableSsids)
            {
                var wifi = WifiNetworks.FirstOrDefault(n => n.Ssid == availableSsid);
                if (wifi == null)
                {
                    _wifiLost = true;
                    continue;
                }

                _wifiLost = false;
                if (_sessionManager.GetIsAtWork()) continue;
                wifiFound = true;
                var startWorkReceiverIntent = new Intent(context, typeof(EnteredWorkReceiver));
                var pendingIntent = PendingIntent.GetBroadcast(context, 0, startWorkReceiverIntent, 0);
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

            if (!wifiFound & _wifiLost)
            {
                if (_sessionManager.GetIsAtWork())
                {
                    var notificationIntent = new Intent(context, typeof(LeftWorkReceiver));
                    var pendingIntent = PendingIntent.GetBroadcast(context, 0, notificationIntent, 0);
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
            }

            Task.Run(() =>
            {
                Thread.Sleep((long) TimeSpan.FromMinutes(1).TotalMilliseconds);
                WifiManager.StartScan();
            });
            
        }
    }
}