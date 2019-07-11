using System.Collections.Generic;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;
using PPCAndroid.Mappers;

namespace PPCAndroid.JobServices
{
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
                //TODO:Usunąć po testach
                Log.Info("network", network);
            }
        }
    }
}