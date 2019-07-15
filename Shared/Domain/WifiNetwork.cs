namespace PPCAndroid.Shared.Domain
{
    public class WifiNetwork
    {
        public WifiNetwork(string ssid)
        {
            Ssid = ssid;
        }
        public string Ssid { get; set; }
    }
}