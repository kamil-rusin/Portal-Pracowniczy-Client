using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Widget;
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
        private Button _logInButton;
        private EditText _usernameEditText;
        private EditText _passwordEditText;
        static WifiManager _wifiManager;
        private WifiScanReceiver _receiverWifi;
        private static IList<ScanResult> _wifiList;
        private bool _onResumeCalled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            _onResumeCalled = false;
            
            OnCreateBase(savedInstanceState);


            //workspace
            _wifiManager = (WifiManager) GetSystemService(Context.WifiService);

            if (_wifiManager.IsWifiEnabled == false)
            {
                _wifiManager.SetWifiEnabled(true);
            }

            _receiverWifi = new WifiScanReceiver();
            RegisterReceiver(_receiverWifi, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            var startedSuccess = _wifiManager.StartScan();

            /*if (_wifiList != null)
            {
                foreach (var network in _wifiList)
                {
                    Log.Info("sieci", network.Ssid);
                }
            }*/
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {
            this.BindCommand(ViewModel, x => x.LoginCommand, v => v._logInButton).DisposeWith(disposables);
        }

        protected override void BindProperties(CompositeDisposable disposables)
        {
            this.Bind(ViewModel, x => x.UserName, a => a._usernameEditText.Text).DisposeWith(disposables);
            this.Bind(ViewModel, x => x.Password, a => a._passwordEditText.Text).DisposeWith(disposables);
        }

        protected override void RegisterViewModel()
        {
            ViewModel = new LoginViewModel(new LoginService());
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

                    /*if (CheckCallingPermission(Manifest.Permission.AccessCoarseLocation) != (int) Permission.Granted ||
                        CheckCallingPermission(Manifest.Permission.AccessFineLocation) != (int) Permission.Granted)
                    {
                        RequestPermissions(
                            new string[] {Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation},
                            87);
                    }*/

                    /*if(CheckCallingPermission(Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
                    {
                        RequestPermissions(new String[]{Manifest.Permission.AccessFineLocation}, 88);
                    }*/
                }
                
            }
            
            base.OnResume();
        }

        private static bool HasPermissions(Context context, string[] permissions) {
            if (context == null || permissions == null) return true;
            foreach (var permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(context, permission) != Permission.Granted)
                {
                    return false;
                }
            }
            return true;
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
        

        private class WifiScanReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action.Equals(WifiManager.ScanResultsAvailableAction))
                {
                    _wifiList = _wifiManager.ScanResults;
                    var test = _wifiManager.ScanResults.ToDomainWifiNetworks();
                }
            }
        }
    }
}