using Android.App.Job;
using Android.Content;
using Android.Net.Wifi;

namespace PPCAndroid.JobServices
{
    /*public class WifiJobService: JobService
    {
        private WifiScanReceiver _receiverWifi;
        private static WifiManager _wifiManager;
        private NotificationReceiver _receiverNotification;

        public override bool OnStartJob(JobParameters @params)
        {
                //TODO: Sprawdzić czy nowy dzień, albo czy wyszedł z pracy i jednak wrócił
                _receiverWifi = new WifiScanReceiver(_wifiManager);
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

            if (_receiverNotification == null) return true;
            UnregisterReceiver(_receiverNotification);
            _receiverNotification = null;

            return true;
        }

        private bool CheckWiFiConnection()
        {
            _wifiManager = (WifiManager) GetSystemService(Context.WifiService);
            return _wifiManager.IsWifiEnabled;
        }
    }*/
}