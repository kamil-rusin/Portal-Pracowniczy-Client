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
    public class MainActivity : ReactiveActivity<ClickerViewModel>
    {
        private Button ClickerButton;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ViewModel = new ClickerViewModel(new ClickerModel());
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            ClickerButton = FindViewById<Button>(Resource.Id.clickerButton);

            ClickerButton.Text = "click me!";
            //tu bindujemy dane
            this.OneWayBind(ViewModel, x => x.ClickedResult, a => a.ClickerButton.Text);
            //tu bindujemy eventy
            this.BindCommand(ViewModel, x => x.IncrementClickerCommand, v => v.ClickerButton);


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