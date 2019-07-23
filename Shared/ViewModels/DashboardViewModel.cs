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
        
        private string _workDate;
        public string WorkDate
        {
            get
            {
                try
                {
                    var d = _workStorage.GetEnteredWorkDate();
                    if (d.ToString(@"HH:mm").Equals("00:00"))
                    {
                        return "--:--";
                    }
                    var x = DateTime.Now - _workStorage.GetEnteredWorkDate();
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
        
        
        public DashboardViewModel(IUserStorage userStorage, IWorkStorage workStorage)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _workStorage = workStorage;
            _userStorage = userStorage;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
        }
        
        private async Task LogOut()
        {
            _workStorage.RemoveWorkData();
            _userStorage.ClearUsernameAndIsLoggedIn();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}