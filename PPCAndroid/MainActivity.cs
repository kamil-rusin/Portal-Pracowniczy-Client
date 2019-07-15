using System.Linq;
using System.Reactive.Disposables;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Support.V4.Content;
using Android.Widget;
using Java.Util.Functions;
using PPCAndroid.JobServices;
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
        private bool _onResumeCalled;
        
        
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _onResumeCalled = false;
            OnCreateBase(savedInstanceState);
           
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
                    
                    var intent = new Intent(BaseContext, typeof(DashboardActivity));
                    StartActivity(intent);
//                    var confirmation = false;
//                    var builder = new AlertDialog.Builder(this);
//                    var alert = builder.Create();
//                    alert.SetTitle("Potwierdzenie");
//                    alert.SetMessage("Na pewno chcesz się zalogować?");
//                    alert.SetButton("Tak", (sender, args) => confirmation = true);
//                    alert.SetButton2("Nie", (sender, args) => confirmation = false);
//                    alert.Show();

                    interaction.SetOutput(true);
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
            //TODO: Jeżeli nie będzie receiveraWifi w mainActivity to usunąć to
           /* if (_receiverWifi != null) 
            {
                UnregisterReceiver(_receiverWifi);
                _receiverWifi = null;
            }*/

            base.OnPause();
        }
    }
}