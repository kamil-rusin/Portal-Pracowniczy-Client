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