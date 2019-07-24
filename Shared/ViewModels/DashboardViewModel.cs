using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Support.Annotation;
using Java.Lang;
using PPCAndroid;
using PPCAndroid.Shared.Domain;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly ISessionManager _sessionManager;
        
        #region Interactions
        public Interaction<Unit, Unit> GoToMainActivity { get; }
        #endregion
        
        #region Properties
        private string _entryDate;
        public string EntryDate
        {
            get => _sessionManager.GetEnteredWorkDate().ToString(@"HH:mm");
            set => this.RaiseAndSetIfChanged(ref _entryDate, value);
        }
        
        private ObservableAsPropertyHelper<string> _workTime;
        public string WorkTime => _workTime.Value;
        #endregion
        
        public ReactiveCommand<Unit,Unit> LogOutCommand { get; private set; }
        
        public DashboardViewModel(ISessionManager sessionManager)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _sessionManager = sessionManager;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
            
            var interval = TimeSpan.FromSeconds(1);
            _workTime = Observable.Timer(TimeSpan.FromSeconds(20), interval)
                .Select(unit => this.UpdateWorkingTime())
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, n => n.WorkTime);
        }
        
        private string UpdateWorkingTime()
        {
            var d = _sessionManager.GetEnteredWorkDate();
            if (d.ToString(@"HH:mm").Equals("00:00"))
            {
                return "--:--";
            }
            var x = DateTime.Now - _sessionManager.GetEnteredWorkDate();
            var s = $"{x.Hours:D2}:{x.Minutes:D2}:{x.Seconds:D2}";
            return s;
        }

        private async Task LogOut()
        {
            _sessionManager.LogOut();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}