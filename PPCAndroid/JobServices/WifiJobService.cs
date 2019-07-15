using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Net.Wifi;
using Android.Renderscripts;
using Android.Support.V4.App;
using Android.Util;
using Java.Lang;
using PPCAndroid.Mappers;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    public class WifiJobService: JobService
    {
        private const string DesiredSsid = "CBR";
        private WifiScanReceiver _receiverWifi;
        private static WifiManager _wifiManager;
        private NotificationReceiver _receiverNotification;
        private bool _alreadyWorking;
        private readonly WifiNetwork _desiredNetwork;

        public WifiJobService()
        {
            _desiredNetwork = new WifiNetwork(DesiredSsid);
            _alreadyWorking = false;
        }

        public override bool OnStartJob(JobParameters @params)
        {
                //TODO: Sprawdzić czy nowy dzień, albo czy wyszedł z pracy i jednak wrócił
                _receiverWifi = new WifiScanReceiver(_wifiManager); //TODO: statyczny wifimanager z mainactivity??
                var wifiEnabled = CheckWiFiConnection();
                if (wifiEnabled)
                {
                    RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                   
                    var startedSuccess = _wifiManager.StartScan();
                    
                    _receiverNotification = new NotificationReceiver();
                    RegisterReceiver(_receiverNotification, new IntentFilter(AppConstant.ConfirmationAction));

                 
                }
            
            
            JobFinished(@params,true);

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            if (_receiverWifi != null) 
            {
                UnregisterReceiver(_receiverWifi);
                _receiverWifi = null;
            }
            
            if (_receiverNotification != null) 
            {
                UnregisterReceiver(_receiverNotification);
                _receiverNotification = null;
            }
            
            return true;
        }

        private bool CheckWiFiConnection()
        {
            _wifiManager = (WifiManager) GetSystemService(Context.WifiService);
            return _wifiManager.IsWifiEnabled;
        }

        private bool CheckIfAtWork()
        {
            return _alreadyWorking;
        }

        //TODO: czy to ma być async task czy jak?
        private void AskIfAtWork() 
        {
            var notificationManager = NotificationManagerCompat.From(this);
            Intent confirmationReceive = new Intent();
            confirmationReceive.SetAction(AppConstant.ConfirmationAction);
            PendingIntent pendingIntentConfirmation = PendingIntent.GetBroadcast(this, 12345, confirmationReceive, PendingIntentFlags.UpdateCurrent);


            var builder = new NotificationCompat.Builder(this, MainActivity.ChannelId)
                .SetContentTitle("Wi-Fi wykryte")
                .SetPriority(1)
                .SetSmallIcon(Resource.Drawable.raports)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetContentText("Kliknij w powiadomienie, aby potwierdzić, że jesteś pracy.")
                .AddAction(Resource.Drawable.abc_list_selector_holo_light,"Potwierdź",pendingIntentConfirmation); 
           
            notificationManager.Notify(MainActivity.NotificationId, builder.Build());
        }
    }
}