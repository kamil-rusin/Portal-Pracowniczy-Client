using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
<<<<<<< HEAD
using Android.Widget;
=======
using PPCAndroid.JobServices;
>>>>>>> wifi_task
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "DashboardActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DashboardActivity :  BaseActivity<DashboardViewModel>
    {
<<<<<<< HEAD
        private TextView _entryTextView;
        private TextView _timeTextView;
        
=======
        private WifiManager _wifiManager;
        private WifiScanReceiver _receiverWifi; 

>>>>>>> wifi_task
        protected override void OnCreate(Bundle savedInstanceState)
        {
            OnCreateBase(savedInstanceState);
            CreateNotificationChannel();
            
            _wifiManager = (WifiManager) GetSystemService(WifiService);
            if (_wifiManager.IsWifiEnabled == false)
            {
                _wifiManager.SetWifiEnabled(true);
            }

            _receiverWifi = new WifiScanReceiver(_wifiManager);
            
            RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            
            _wifiManager.StartScan();
        }
        
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var name = Resources.GetString(Resource.String.channel_name);
            var description = GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(MainActivity.ChannelId, name, NotificationImportance.High)
            {
                Description = description
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {

        }

        protected override void BindProperties(CompositeDisposable disposables)
        {

        }

        protected override void RegisterViewModel()
        {
            ViewModel = new DashboardViewModel();
        }

        protected override void RegisterInteractions()
        {
       
        }

        protected override void RegisterView()
        {
            SetContentView(Resource.Layout.activity_dashboard);
        }

        protected override void RegisterControls()
        {
            _entryTextView = FindViewById<TextView>(Resource.Id.entryTextView);
            _timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
        }
    }
}