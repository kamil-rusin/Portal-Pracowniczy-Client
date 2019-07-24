using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        
        
        public DashboardViewModel(IUserStorage userStorage, IWorkStorage workStorage, IEventService eventService)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _workStorage = workStorage;
            _userStorage = userStorage;
            _eventService = eventService;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
            
            var interval = TimeSpan.FromSeconds(5);
            
            _workTime = Observable.Timer(TimeSpan.FromSeconds(15), interval)
                .Select(unit => this.UpdateWorkingTime())
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, n => n.WorkTime);
        }
        
        private string UpdateWorkingTime()
        {
            IEnumerable<EventBase> eventBases = _eventService.GetEventsFromDay(DateTime.Now);
            var baseTimeSpan = TimeSpan.Zero;
            var enumerable = eventBases as EventBase[] ?? eventBases.ToArray();
            if (enumerable.Last().EventType == nameof(EndWorkEvent))
            {
                baseTimeSpan = _eventService.CountWorkTime(DateTime.Now);
            }
            else if (enumerable.Last().EventType == nameof(StartWorkEvent))
            {
                for (var i = 0; i < enumerable.Count(); i++)
                {
                    if (i == enumerable.Length - 1)
                        baseTimeSpan += DateTime.Now - enumerable[i].When;
                    else if (enumerable[i].EventType == nameof(EndWorkEvent))
                        continue;
                    else
                    {
                        baseTimeSpan += enumerable[i + 1].When - enumerable[i].When;
                    }
                }
            }

            if (baseTimeSpan.ToString(@"hh\:mm\:ss").Equals("00:00:00"))
            {
                return "--:--";
            }
            
            var s = $"{baseTimeSpan.Hours:D2}:{baseTimeSpan.Minutes:D2}:{baseTimeSpan.Seconds:D2}";
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