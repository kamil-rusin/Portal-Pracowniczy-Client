using System.Reactive.Disposables;
using Android.App;
using Android.OS;
using Shared.ViewModels;

namespace PPCAndroid
{
    [Activity(Label = "DashboardActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class DashboardActivity :  BaseActivity<DashboardViewModel>
    {
    
        protected override void OnCreate(Bundle savedInstanceState)
        {
            OnCreateBase(savedInstanceState);
            
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
         
        }

        protected override void RegisterControls()
        {

        }
    }
}