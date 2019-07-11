using Android.App;
using Android.Content;
using Android.Widget;

namespace PPCAndroid
{
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // TODO Auto-generated method stub
            string action = intent.Action;
            if (AppConstant.ConfirmationAction.Equals(action)) {
                //TODO: jest w pracy, dobry toast?
                Toast.MakeText(Application.Context, "Jesteś w pracy", ToastLength.Short);
            } 
            //TODO: nie jest w pracy
            
        }
    }
}