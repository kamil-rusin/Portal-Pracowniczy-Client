using System.Collections.Generic;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;
using PPCAndroid.Mappers;

namespace PPCAndroid.JobServices
{
    /*public class WifiScanReceiver2 : BroadcastReceiver
    {
        public WifiManager _wifiManager;
        public static IList<ScanResult> WifiList
        {
            get;
            private set;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            WifiList = _wifiManager.ScanResults;
            var test = _wifiManager.ScanResults.ToDomainWifiNetworks();
            foreach (var network in test)
            {
                Log.Info("network", network);
            }
        }
        
    }*/
}