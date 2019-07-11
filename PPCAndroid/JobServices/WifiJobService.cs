using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App.Job;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;
using Java.Lang;
using PPCAndroid.Mappers;

namespace PPCAndroid.JobServices
{
    public class WifiJobService: JobService
    {
        private const string DesiredSsid = "CBR";
        private WifiScanReceiver _receiverWifi;
        private bool alreadyWorking;

        public WifiJobService()
        {
            alreadyWorking = false;
            _receiverWifi = new WifiScanReceiver();
        }

        public override bool OnStartJob(JobParameters @params)
        {

            Task.Run(() =>
            {
                //TODO: work when wifi enabled 
                var wifiEnabled = CheckWiFiConnection();
                if (wifiEnabled)
                {
                    //TODO: skanować sieć! osobny task najlepiej!
                    RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                    var startedSuccess = _receiverWifi.WifiManager.StartScan();
                    if (_receiverWifi.WifiList.Contains(DesiredSsid))
                    {
                        //TODO: istnieje wifi rekordowe
                        if (!CheckIfAtWork())
                        {
                            var working = AskIfAtWork();
                        }
                    }
                }
            });
            
            JobFinished(@params,true);

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
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

        private async Task<bool> AskIfAtWork() 
        {
            //TODO: powiadomienia 
            
            return true;
        }
    }
    
    public class WifiScanReceiver : BroadcastReceiver
    {
        public WifiManager WifiManager;
        public IEnumerable<string> WifiList { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            var test = WifiManager.ScanResults.ToDomainWifiNetworks();
            foreach (var network in test)
            {
                Log.Info("network", network);
            }
        }
        
    }
}