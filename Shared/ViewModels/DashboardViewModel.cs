using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
        
        private string _workDate;
        public string WorkDate
        {
            get
            {
                try
                {
                    var d = _sessionManager.GetEnteredWorkDate();
                    if (d.ToString(@"HH:mm").Equals("00:00"))
                    {
                        throw new Exception();
                    }
                    var x = DateTime.Now - _sessionManager.GetEnteredWorkDate();
                    var s = $"{x.Hours:D2}:{x.Minutes:D2}:{x.Seconds:D2}";
                    return s;
                }
                catch (Exception e)
                {
                    return "--:--";
                }
            }
            
            set => this.RaiseAndSetIfChanged(ref _workDate, value);
        }
        #endregion
        
        public ReactiveCommand<Unit,Unit> LogOutCommand { get; private set; }
        
        
        public DashboardViewModel(ISessionManager sessionManager)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _sessionManager = sessionManager;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
        }
        
        private async Task LogOut()
        {
            _sessionManager.LogOut();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}