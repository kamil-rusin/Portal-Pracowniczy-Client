using System;
using Android.App;
using Android.Content;

namespace PPCAndroid.JobServices
{
    [Service]
    public class AtWorkIntentService : IntentService
    {
        private AppVariables _appVariables;

        public AtWorkIntentService() : base("AtWorkIntentService")
        {
            _appVariables = new AppVariables();
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!(_appVariables.IsLogged & (!_appVariables.AtWork))) return;
            _appVariables.LogDate = DateTime.Now;
            _appVariables.AtWork = true;
        }
    }
}