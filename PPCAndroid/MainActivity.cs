using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Model;
using ReactiveUI;
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : ReactiveActivity<LoginViewModel>
    {
        private Button LogInButton;
        private EditText UsernameEditText;
        private EditText PasswordEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ViewModel = new LoginViewModel(new LoginModel());
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            LogInButton = FindViewById<Button>(Resource.Id.loginButton);
            UsernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            PasswordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);

            LogInButton.Text = "click me!";
            //tu bindujemy dane
            this.Bind(ViewModel, x => x.Username, a => a.UsernameEditText.Text);
            this.Bind(ViewModel, x => x.Password, a => a.PasswordEditText.Text);
            //tu bindujemy eventy
            this.BindCommand(ViewModel, x => x.CheckCommand, v => v.LogInButton);

            LogInButton.Click += (s, e) =>
            {
                Toast.MakeText(this, "Login " + ViewModel.CheckResult.ToString(), ToastLength.Short).Show();
            };

            /*var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (s, e) =>
            {
                switch (e.Item.ItemId)
                {
                    case Resource.Id.action_showpersonaldata:
                        Toast.MakeText(this, "Action ShowPersonalData clicked", ToastLength.Short).Show();
                        break;
                    case Resource.Id.action_backtodashboard:
                        Toast.MakeText(this, "Action BackToDashboard clicked", ToastLength.Short).Show();
                        break;
                    case Resource.Id.action_logout:
                        Toast.MakeText(this, "Action Logout clicked", ToastLength.Short).Show();
                        break;
                }
            };*/
        } 
    }
}