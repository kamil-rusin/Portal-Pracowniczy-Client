using System.Reactive;
using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using PPCAndroid.JobServices;
using ReactiveUI;
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "DashboardActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DashboardActivity :  BaseActivity<DashboardViewModel>
    {
        private TextView _entryTextView;
        private TextView _timeTextView;
        private TextView _entryDateTextView;
        private TextView _workDateTextView;
        private BottomNavigationView _bottomNavigation;
        private View _logOutItem;
        
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
            var channel = new NotificationChannel(AppConstants.ChannelId, name, NotificationImportance.High)
            {
                Description = description
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void BindCommands(CompositeDisposable disposables)
        {
            this.BindCommand(ViewModel, x => x.LogOutCommand, v=>v._logOutItem).DisposeWith(disposables);
        }
        

        protected override void BindProperties(CompositeDisposable disposables)
        {
            this.OneWayBind(ViewModel, x => x.EntryTimeObservable, a => a._entryDateTextView.Text).DisposeWith(disposables);
            this.OneWayBind(ViewModel, x => x.WorkTime, a => a._workDateTextView.Text).DisposeWith(disposables);
        }

        protected override void RegisterViewModel()
        {
            ViewModel = new DashboardViewModel(
                AndroidObjectFactory.GetUserStorage(Application.Context),
                AndroidObjectFactory.GetWorkStorage(Application.Context),
                AndroidObjectFactory.GetEventService()
                );
        }

        protected override void RegisterInteractions()
        {
            this.WhenActivated(d => { d(ViewModel.GoToMainActivity.RegisterHandler(interaction =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                interaction.SetOutput(Unit.Default);
            })); });
        }

        protected override void RegisterView()
        {
            SetContentView(Resource.Layout.activity_dashboard);
        }

        protected override void RegisterControls()
        {
            _entryTextView = FindViewById<TextView>(Resource.Id.entryTextView);
            _timeTextView = FindViewById<TextView>(Resource.Id.timeTextView);
            _bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            _entryDateTextView = FindViewById<TextView>(Resource.Id.entryDateTextView);
            _workDateTextView = FindViewById<TextView>(Resource.Id.workDateTextView);
            _logOutItem = FindViewById(Resource.Id.action_logout);
        }
    }
}