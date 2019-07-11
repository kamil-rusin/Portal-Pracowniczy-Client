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

namespace PPCAndroid.JobServices
{
    public class WifiJobService: JobService
    {
        private const string DesiredSsid = "CBR";
        private WifiScanReceiver _receiverWifi;
        private NotificationReceiver _receiverNotification;
        private bool alreadyWorking;

        public WifiJobService()
        {
            alreadyWorking = false;
        }

        public override bool OnStartJob(JobParameters @params)
        {
            Task.Run(() =>
            {
                _receiverWifi = new WifiScanReceiver();
                var wifiEnabled = CheckWiFiConnection();
                if (wifiEnabled)
                {
                    RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                    var startedSuccess = _receiverWifi.WifiManager.StartScan();
                    _receiverNotification = new NotificationReceiver();
                    RegisterReceiver(_receiverNotification, new IntentFilter(AppConstant.ConfirmationAction));

                    if (_receiverWifi.WifiList.Contains(DesiredSsid))
                    {
                        //TODO: istnieje wifi rekordowe
                        if (!CheckIfAtWork())
                        {
                            AskIfAtWork();
                        }
                    }
                    else
                    {
                        //TODO: nie istnieje wifi rekordowe
                    }
                }
            });
            
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
            _receiverWifi.WifiManager = (WifiManager) GetSystemService(Context.WifiService);
            return _receiverWifi.WifiManager.IsWifiEnabled;
        }

        private bool CheckIfAtWork()
        {
            return alreadyWorking;
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