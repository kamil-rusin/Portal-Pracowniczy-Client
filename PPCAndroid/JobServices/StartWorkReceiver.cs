using System;
using Android.App;
using Android.Content;
using PPCAndroid;

namespace PPCAndroid.JobServices
{

    public class StartWorkReceiver : BroadcastReceiver
    {
        private SessionManager _sessionManager;
        
       public override void OnReceive(Context context, Intent intent)
       {
            _sessionManager = new SessionManager(context);
            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
            _sessionManager.SaveLogInDate(DateTime.Now);
            _sessionManager.SaveAtWork(true);
            //_sessionManager.Dispose();
       }
    }
    /*public sealed class StartWorkReceiver1 : BroadcastReceiver
    {
        private SessionManager _sessionManager;
        private static StartWorkReceiver1 instance = null;
        private static readonly object padlock = new object();

        private StartWorkReceiver1()
        {
        }

        public static StartWorkReceiver1 Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StartWorkReceiver1();
                    }

                    return instance;
                }
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            _sessionManager = new SessionManager(context);
            if (!(_sessionManager.GetIsLoggedIn() & (!_sessionManager.GetIsAtWork()))) return;
            _sessionManager.SaveLogInDate(DateTime.Now);
            _sessionManager.SaveAtWork(true);
            //_sessionManager.Dispose();    }
        }
    }*/
}

