using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class LeftWorkIntentService : IntentService
    {
        private AppVariables _appVariables;

        public LeftWorkIntentService() : base("LeftWorkIntentService")
        {
            _appVariables = new AppVariables();
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!(_appVariables.IsLogged & _appVariables.AtWork)) return;
            _appVariables.OffDate = DateTime.Now;
            _appVariables.AtWork = false;
        }
    }
}