using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;
using PPCAndroid.Mappers;

namespace PPCAndroid.JobServices
{
    public class WifiScanReceiver : BroadcastReceiver
    {
        private readonly WifiManager _wifiManager;
        private readonly Subject<IEnumerable<WifiNetwork>> _wiFiNetworksSubject;
        
        public IObservable<IEnumerable<WifiNetwork>> WiFiNetworksObs => _wiFiNetworksSubject;
        public WifiScanReceiver(WifiManager wifiManager)    
        {
            _wifiManager = wifiManager;
            _wiFiNetworksSubject = new Subject<IEnumerable<WifiNetwork>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
            var wifiNetworks = _wifiManager.ScanResults.ToDomainWifiNetworks().ToList();
            _wiFiNetworksSubject.OnNext(wifiNetworks);
        }
    }
}