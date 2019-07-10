using System.Collections.Generic;
using System.Linq;
using Android.Net.Wifi;

namespace PPCAndroid.Mappers
{
    public static class WifiNetworkMapper
    {
        public static IEnumerable<string> ToDomainWifiNetworks(this IList<ScanResult> scanResults)
        {
            return scanResults.Select(n => n.Ssid);
        }
    }
}