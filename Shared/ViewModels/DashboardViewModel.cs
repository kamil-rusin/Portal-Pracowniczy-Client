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
        private readonly IStorage _storage;
        
        #region Interactions
        public Interaction<Unit, Unit> GoToMainActivity { get; }
        #endregion
        
        #region Properties
        private string _entryDate;
        public string EntryDate
        {
            get => _storage.GetEnteredWorkDate().ToString(@"HH:mm");
            set => this.RaiseAndSetIfChanged(ref _entryDate, value);
        }
        
        private string _workDate;
        public string WorkDate
        {
            get
            {
                try
                {
                    var d = _storage.GetEnteredWorkDate();
                    if (d.ToString(@"HH:mm").Equals("00:00"))
                    {
                        return "--:--";
                    }
                    var x = DateTime.Now - _storage.GetEnteredWorkDate();
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
        
        
        public DashboardViewModel(IStorage storage)    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _storage = storage;
            
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
        }
        
        private async Task LogOut()
        {
            _storage.LogOut();
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}