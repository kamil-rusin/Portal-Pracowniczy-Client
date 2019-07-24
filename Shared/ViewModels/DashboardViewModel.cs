using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.Content;
using PPCAndroid;
using PPCAndroid.Shared.Domain;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IWorkStorage _workStorage;
        private readonly IUserStorage _userStorage;
        private IEventService _eventService;
        
        #region Interactions
        public Interaction<Unit, Unit> GoToMainActivity { get; }
        #endregion
        
        #region Properties
        private string _entryDate;
        public string EntryDate
        {
            get => _workStorage.GetEnteredWorkDate().ToString(@"HH:mm");
            set => this.RaiseAndSetIfChanged(ref _entryDate, value);
        }
        #endregion
        
        private ObservableAsPropertyHelper<string> _workTime;
        public string WorkTime => _workTime.Value;
        public ReactiveCommand<Unit,Unit> LogOutCommand { get; private set; }
        
        
        public DashboardViewModel(IUserStorage userStorage, IWorkStorage workStorage)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _workStorage = workStorage;
            _userStorage = userStorage;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
            
            var interval = TimeSpan.FromSeconds(1);
            
            _workTime = Observable.Timer(TimeSpan.FromSeconds(20), interval)
                .Select(unit => this.UpdateWorkingTime())
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, n => n.WorkTime);
        }
        
        private string UpdateWorkingTime()
        {
            IEnumerable<EventBase> eventBases = _eventService.GetEventsFromDay(DateTime.Now);

            var d = _workStorage.GetEnteredWorkDate();
            if (d.ToString(@"HH:mm").Equals("00:00"))
            {
                return "--:--";
            }
            var x = DateTime.Now - _workStorage.GetEnteredWorkDate();
            var s = $"{x.Hours:D2}:{x.Minutes:D2}:{x.Seconds:D2}";
            return s;
        }

        
        private async Task LogOut()
        {
            _workStorage.RemoveWorkData();
            _userStorage.ClearUsernameAndIsLoggedIn();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}