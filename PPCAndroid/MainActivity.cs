using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Android.Util;
using Android.Widget;
using PPCAndroid.JobServices;
using PPCAndroid.Mappers;
using PPCAndroid.Shared.Service;
using ReactiveUI;
using Shared.ViewModels;
using AlertDialog = Android.App.AlertDialog;

namespace PPCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : BaseActivity<LoginViewModel>
    {
        public static readonly int NotificationId = 1000;
        public static readonly string ChannelId = "work_notification";
        private Button _logInButton;
        private EditText _usernameEditText;
        private EditText _passwordEditText;
        private ListView _wifiListView;
        private ArrayAdapter<string> adapter;
        private static WifiManager _wifiManager;
        private WifiScanReceiver _receiverWifi;
        private static IList<ScanResult> _wifiList;
        private bool _onResumeCalled;
        
        
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _onResumeCalled = false;
            OnCreateBase(savedInstanceState);
            CreateNotificationChannel();

            //workspace
            _wifiManager = (WifiManager) GetSystemService(Context.WifiService);

            if (_wifiManager.IsWifiEnabled == false)
            {
                _wifiManager.SetWifiEnabled(true);
            }

            _receiverWifi = new WifiScanReceiver(_wifiManager);
            RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            
            var startedSuccess = _wifiManager.StartScan();

            //TODO: Observable dać na listę wifi i ją wyświetlić
            //TODO: Kamil, jak to zbindować?
            List<string> carL = new List<string>();  
            carL.Add("rekord");
            carL.Add("android");

            adapter = new ArrayAdapter<string>(this, Resource.Layout.item_layout, carL);

            _wifiListView.Adapter = adapter;

        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var name = Resources.GetString(Resource.String.channel_name);
            var description = GetString(Resource.String.channel_description);
            var channel = new NotificationChannel(ChannelId, name, NotificationImportance.High)
            {
                Description = description
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {
            this.BindCommand(ViewModel, x => x.LoginCommand, v => v._logInButton).DisposeWith(disposables);
        }

        protected override void BindProperties(CompositeDisposable disposables)
        {
            this.Bind(ViewModel, x => x.UserName, a => a._usernameEditText.Text).DisposeWith(disposables);
            this.Bind(ViewModel, x => x.Password, a => a._passwordEditText.Text).DisposeWith(disposables);
            //TODO: Kamil, binding z listą
        }

        protected override void RegisterViewModel()
        {
            //TODO: dobrze dałem to _receiverWifi.WiFiNetworksObs?
            ViewModel = new LoginViewModel(new LoginService(), _receiverWifi.WiFiNetworksObs);
        }

        protected override void RegisterInteractions()
        {
            this.WhenActivated(d =>
            {
                d(ViewModel.Confirm.RegisterHandler(async interaction =>
                {
                    var confirmation = false;
                    var builder = new AlertDialog.Builder(this);
                    var alert = builder.Create();
                    alert.SetTitle("Potwierdzenie");
                    alert.SetMessage("Na pewno chcesz się zalogować?");
                    alert.SetButton("Tak", (sender, args) => confirmation = true);
                    alert.SetButton2("Nie", (sender, args) => confirmation = false);
                    alert.Show();

                    interaction.SetOutput(confirmation);
                }));
            });
            
            
        }

        protected override void RegisterView()
        {
            SetContentView(Resource.Layout.activity_main);
        }

        protected override void RegisterControls()
        {
            _logInButton = FindViewById<Button>(Resource.Id.loginButton);
            _usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            _passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            _wifiListView = FindViewById<ListView>(Resource.Id.wifiListView);
        }

        protected override void OnResume()
        {
            if (!_onResumeCalled)
            {
                _onResumeCalled = true;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    string[] permissions = {Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation};
                     
                    if (!HasPermissions(this, permissions))
                    {
                        RequestPermissions(permissions, 87);
                    }
                }
                
            }
            
            base.OnResume();
        }

        private static bool HasPermissions(Context context, string[] permissions) {
            if (context == null || permissions == null) return true;
            return permissions.All(permission => ContextCompat.CheckSelfPermission(context, permission) == Permission.Granted);
        }

        protected override void OnPause()
        {
            
            if (_receiverWifi != null) 
            {
                UnregisterReceiver(_receiverWifi);
                _receiverWifi = null;
            }

            base.OnPause();
        }
        

        /*private class WifiScanReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (!intent.Action.Equals(WifiManager.ScanResultsAvailableAction)) return;
                _wifiList = _wifiManager.ScanResults;
                var test = _wifiManager.ScanResults.ToDomainWifiNetworks();
                foreach (var network in test)
                {
                    Log.Info("network", network.Ssid);
                }
                
                
            }
        }*/
    }
}