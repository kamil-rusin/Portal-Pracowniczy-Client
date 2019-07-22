using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Views;
using Android.Widget;
using PPCAndroid.JobServices;
using Shared.ViewModels;
using System.Timers;
using System;

namespace PPCAndroid
{
    [Activity(Label = "DashboardActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DashboardActivity :  BaseActivity<DashboardViewModel>
    {
        private TextView _entryTextView;
        private TextView _timeTextView;
        
        private WifiManager _wifiManager;
        private WifiScanReceiver _receiverWifi; 
        

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
            if (LoginViewModel.beginning == true)
            {
                TextView textV = FindViewById<TextView>(Resource.Id.entryTextView);
                textV.Text = "Jesteś w pracy od:\n " + LoginViewModel.logging;
                textV.TextAlignment = TextAlignment.Center;
                TimeSpan ts = TimeSpan.Parse((LoginViewModel.logged).ToString());

                TextView text = FindViewById<TextView>(Resource.Id.timeTextView);
                RunOnUiThread(() => { text.Text = "Dzisiaj pracujesz już:\n" + ts.ToString(@"hh\:mm"); });
                text.TextAlignment = TextAlignment.Center;
            }

        }
    }
}