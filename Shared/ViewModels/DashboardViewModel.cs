using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PPCAndroid.Domain;
using PPCAndroid.Shared.Domain;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IWorkStorage _workStorage;
        private readonly IUserStorage _userStorage;
        private readonly IEventService _eventService;
        
        #region Interactions
        public Interaction<Unit, Unit> GoToMainActivity { get; }
        #endregion

        private readonly ObservableAsPropertyHelper<string> _entryTimeObservable;
        private readonly ObservableAsPropertyHelper<string> _workTime;
        
        public string EntryTimeObservable => _entryTimeObservable.Value;
        public string WorkTime => _workTime.Value;
        private string _entryTime;

        private string EntryTime
        {
            get => _entryTime;
            set => this.RaiseAndSetIfChanged(ref _entryTime, value);
        }
        public ReactiveCommand<Unit,Unit> LogOutCommand { get; private set; }

        public DashboardViewModel(IUserStorage userStorage, IWorkStorage workStorage, IEventService eventService)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _workStorage = workStorage;
            _userStorage = userStorage;
            _eventService = eventService;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
            
            _entryTimeObservable = this
                .WhenAnyValue(x => x.EntryTime)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this,n => n.EntryTimeObservable);
            
            var interval = TimeSpan.FromSeconds(1);
            _workTime = Observable.Timer(TimeSpan.FromSeconds(5), interval)
                .Select(unit => this.UpdateWorkingTime())
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, n => n.WorkTime);
        }
        
        private string UpdateWorkingTime()
        {
            var eventBases = _eventService.GetEventsFromDay(DateTime.Now);
            var baseTimeSpan = TimeSpan.Zero;
            if (eventBases != null)
            {
                var enumerable = eventBases as EventBase[] ?? eventBases.ToArray();
                EntryTime = enumerable.First().When.ToString(@"hh\:mm\:ss");
                if (enumerable.Last().EventType == nameof(EndWorkEvent))
                {
                    baseTimeSpan = EventsCounter.CountWorkTime(_eventService.GetAll(),DateTime.Now);
                }
                else if (enumerable.Last().EventType == nameof(StartWorkEvent))
                {
                    for (var i = 0; i < enumerable.Count(); i++)
                    {
                        if (i == enumerable.Length - 1)
                            baseTimeSpan += DateTime.Now - enumerable[i].When;
                        else
                        {
                            baseTimeSpan += enumerable[i + 1].When - enumerable[i].When;
                        }
                    }
                }
            }

            if (baseTimeSpan.ToString(@"hh\:mm\:ss").Equals("00:00:00"))
            {
                EntryTime = "--:--";
                return "--:--";
            }
            
            var s = $"{baseTimeSpan.Hours:D2}:{baseTimeSpan.Minutes:D2}:{baseTimeSpan.Seconds:D2}";
            return s;
        }

        private async Task LogOut()
        {
            _workStorage.RemoveWorkData();
            _userStorage.ClearUsernameAndIsLoggedIn();
            _eventService.ClearAllEvents();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}