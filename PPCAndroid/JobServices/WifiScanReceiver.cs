using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
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
        //TODO: Session reference
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
            //TODO: Session reference
            _appVariables = new AppVariables();
            WifiManager = wifiManager;
            _wiFiNetworksSubject = new Subject<IEnumerable<WifiNetwork>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var wifiFound = false;
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            WifiNetworks = WifiManager.ScanResults.ToDomainWifiNetworks().ToList();
            _wiFiNetworksSubject.OnNext(WifiNetworks);

            foreach (var availableSsid in _availableSsids)
            {
                var wifi = WifiNetworks.FirstOrDefault(n => n.Ssid == availableSsid);
                if (wifi == null) continue;
                //TODO: Session reference
                if (_appVariables.AtWork) continue;
                wifiFound = true;
                var notificationIntent = new Intent(context, typeof(AtWorkIntentService));
                var pendingIntent = PendingIntent.GetService(context, 0, notificationIntent, 0);
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

            if (!wifiFound)
            {
                //TODO: Session reference
                if (_appVariables.AtWork)
                {
                    var notificationIntent = new Intent(context, typeof(LeftWorkIntentService));
                    var pendingIntent = PendingIntent.GetService(context, 0, notificationIntent, 0);
                    var builder = new NotificationCompat.Builder(context, MainActivity.ChannelId)
                        .SetContentTitle("Utracono firmową sieć")
                        .SetContentText("Kliknij, jeżeli wyszedłeś z pracy.")
                        .SetSmallIcon(Resource.Drawable.raports)
                        .SetContentIntent(pendingIntent)
                        .SetDefaults((int) NotificationDefaults.Sound | (int) NotificationDefaults.Vibrate)
                        .SetPriority(NotificationCompat.PriorityHigh);

                    var notificationManager = NotificationManagerCompat.From(context);
                    notificationManager.Notify(MainActivity.NotificationId, builder.Build());
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