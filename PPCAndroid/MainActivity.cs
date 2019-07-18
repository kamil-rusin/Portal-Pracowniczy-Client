using System.Reactive;
using System.Linq;
using System.Reactive.Disposables;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Content;
using Android.Widget;
using PPCAndroid.JobServices;
using PPCAndroid.Shared.Service;
using ReactiveUI;
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : BaseActivity<LoginViewModel>
    {
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
            ViewModel = new LoginViewModel(new LoginService(), new SessionManager(Application.Context));
        }

        protected override void RegisterInteractions()
        {
            this.WhenActivated(d => { d(ViewModel.GoToDashboard.RegisterHandler(interaction =>
                {
                    var intent = new Intent(this, typeof(DashboardActivity));
                    StartActivity(intent);
                    interaction.SetOutput(Unit.Default);
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
    }
}