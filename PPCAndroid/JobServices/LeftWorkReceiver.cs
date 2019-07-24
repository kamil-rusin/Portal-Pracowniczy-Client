using System;
using Android.App;
using Android.Content;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class LeftWorkReceiver : BroadcastReceiver
    {
        private IWorkStorage _workStorage;
        private IUserStorage _userStorage;
        private IEventService _eventService;
        
        public override void OnReceive(Context context, Intent intent)
        {
            _userStorage = AndroidObjectFactory.GetUserStorage(context);
            _workStorage = AndroidObjectFactory.GetWorkStorage(context);
            _eventService = AndroidObjectFactory.GetEventService();
            if (!(_userStorage.GetIsLoggedIn()) & _workStorage.GetIsAtWork()) return;
            //_workStorage.SaveLeavingDate(DateTime.Now);
            _workStorage.SaveAtWork(false);
            
            _eventService.Add(new EndWorkEvent(DateTime.Now));
        }
    }
}