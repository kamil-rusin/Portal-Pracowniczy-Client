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
using Android.Content;


namespace PPCAndroid.JobServices
{
    public class WifiScanReceiver : BroadcastReceiver
    {
        private AppVariables _appVariables;
        private readonly List<string> _availableSsids = new List<string>
        {
            "AndroidWifi"
        };

        public WifiManager WifiManager { get; private set; }
        private readonly Subject<IEnumerable<WifiNetwork>> _wiFiNetworksSubject;

        public List<WifiNetwork> WifiNetworks { get; set; }
        public IObservable<IEnumerable<WifiNetwork>> WiFiNetworksObs => _wiFiNetworksSubject;
        
        public WifiScanReceiver(WifiManager wifiManager)    
        {
            _appVariables = new AppVariables();
            WifiManager = wifiManager;
            _wiFiNetworksSubject = new Subject<IEnumerable<WifiNetwork>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (_appVariables.AtWork) return;
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            WifiNetworks = WifiManager.ScanResults.ToDomainWifiNetworks().ToList();
            _wiFiNetworksSubject.OnNext(WifiNetworks);

            var notificationIntent = new Intent(context, typeof(AtWorkIntentService));
            var pendingIntent = PendingIntent.GetForegroundService(context, 0, notificationIntent, 0);

            foreach (var availableSsid in _availableSsids)
            {
                var wifi = WifiNetworks.FirstOrDefault(n => n.Ssid == availableSsid);
                if (wifi == null) continue;
                var builder = new NotificationCompat.Builder(context, MainActivity.ChannelId)
                    .SetContentTitle("Wykryto sieć " + wifi.Ssid)
                    .SetContentText("Kliknij, jeżeli jesteś w pracy.")
                    .SetSmallIcon(Resource.Drawable.raports)
                    .SetContentIntent(pendingIntent)
                    .SetDefaults((int) NotificationDefaults.Sound | (int) NotificationDefaults.Vibrate)
                    .SetPriority(NotificationCompat.PriorityHigh);

                var notificationManager = NotificationManagerCompat.From(context);
                notificationManager.Notify(MainActivity.NotificationId, builder.Build());
            }

            Task.Run(() =>
            {
                Thread.Sleep((long) TimeSpan.FromMinutes(1).TotalMilliseconds);
                WifiManager.StartScan();
            });

        }
    }
}