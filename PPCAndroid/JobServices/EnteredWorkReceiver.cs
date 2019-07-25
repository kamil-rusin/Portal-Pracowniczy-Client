using System;
using System.Linq;
using System.Reactive.Subjects;
using Android.App;
using Android.Content;
using PPCAndroid;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.JobServices
{
    [BroadcastReceiver]
    public class EnteredWorkReceiver : BroadcastReceiver
    {
        private IWorkStorage _workStorage;
        private IUserStorage _userStorage;
        private IEventService _eventService;
        
        public override void OnReceive(Context context, Intent intent)
       {
           _workStorage = AndroidObjectFactory.GetWorkStorage(context);
            _userStorage = AndroidObjectFactory.GetUserStorage(context);
            _eventService = AndroidObjectFactory.GetEventService();
            if (!(_userStorage.GetIsLoggedIn() & (!_workStorage.GetIsAtWork()))) return;
            _workStorage.SaveEntryDate(DateTime.Now);
            _workStorage.SaveAtWork(true);

            _eventService.Add(new StartWorkEvent(DateTime.Now));
       }
    }
}

