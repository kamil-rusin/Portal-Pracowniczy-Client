﻿using System.Reactive;
using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Service.QuickSettings;
using Android.Widget;
using PPCAndroid.Shared.Service;
using ReactiveUI;
using Shared.ViewModels;
using AlertDialog = Android.App.AlertDialog;

namespace PPCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity :  BaseActivity<LoginViewModel>
    {
        private Button _logInButton;
        private EditText _usernameEditText;
        private EditText _passwordEditText;

       
        protected override void OnCreate(Bundle savedInstanceState)
        {
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
            this.WhenActivated(d => { d(ViewModel.GoToDashboard.RegisterHandler(async interaction =>
                {
                    //var intent = new Intent(this, typeof(DashboardActivity));
                    //StartActivity(intent);
                    interaction.SetOutput(new Unit());
                })); 
            });
            
            this.WhenActivated(d => { d(ViewModel.Confirm.RegisterHandler(async interaction =>
            {
                var builder = new AlertDialog.Builder(this);
                var alert = builder.Create();
                alert.SetTitle("Potwierdzenie");
                alert.SetMessage("Na pewno chcesz się zalogować?");
                alert.SetButton("Tak", (sender, args) => interaction.SetOutput(true));
                alert.SetButton2("Nie", (sender, args) => interaction.SetOutput(false));
                alert.Show();
            })); });
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
    }
}