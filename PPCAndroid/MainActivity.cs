using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Widget;

namespace PPCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState); 
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
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
            };
        } 
    }
}