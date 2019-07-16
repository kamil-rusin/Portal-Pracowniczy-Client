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
        //TODO: Session reference
        private AppVariables _appVariables;
        
        #region Interactions
        public Interaction<Unit, Unit> GoToMainActivity { get; }
        #endregion
        
        public ReactiveCommand<Unit,Unit> LogOutCommand { get; private set; }
        
        public DashboardViewModel()    
        {
            GoToMainActivity= new Interaction<Unit, Unit>();
            _appVariables = new AppVariables();
            LogOutCommand = ReactiveCommand.CreateFromTask(async () => { await LogOut();  });
        }
        
        private async Task LogOut()
        {
            //TODO: Session reference
            _appVariables.UserName = null;
            _appVariables.IsLogged = false;
            _appVariables.OffDate = DateTime.Now;
            _appVariables.AtWork = false;
            
            await GoToMainActivity.Handle(Unit.Default);
        }
    }
}