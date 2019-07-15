using System.Collections.Generic;
using System.Linq;
using Android.Net.Wifi;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.Mappers
{
    public static class WifiNetworkMapper
    {
        public static IEnumerable<WifiNetwork> ToDomainWifiNetworks(this IList<ScanResult> scanResults)
        {
            return scanResults.Select(n => new WifiNetwork(n.Ssid));
        }
    }
}