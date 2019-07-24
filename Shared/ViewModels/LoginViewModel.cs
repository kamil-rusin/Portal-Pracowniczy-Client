using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Util;
using PPCAndroid;
using PPCAndroid.Shared.Domain;
using PPCAndroid.Shared.Service;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Interactions
        public Interaction<Unit, Unit> GoToDashboard { get; }
        #endregion

        #region Properties
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }
        
        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
        #endregion
        
        private readonly ILogin _loginService;
        private readonly IWorkStorage _workStorage;
        private readonly IUserStorage _userStorage;

        public ReactiveCommand<Unit,Unit> LoginCommand { get; private set; }
        
        public LoginViewModel(ILogin login, IWorkStorage workStorage, IUserStorage userStorage)
        {
            
            _loginService = login;
            _workStorage = workStorage;
            _userStorage = userStorage;
            GoToDashboard= new Interaction<Unit, Unit>();

            var canLogin = this.WhenAnyValue(x => x.UserName, x => x.Password, LoginInputValidator.Validate);
            LoginCommand = ReactiveCommand.CreateFromTask(async () => { await TryLogin();  }, canLogin);
        }

        private async Task<IObservable<bool>> TryLogin()
        {
            var lg = await _loginService.Login(_userName, _password);
            if (lg)
            {
                _workStorage.RemoveWorkData();
                _userStorage.ClearUsernameAndIsLoggedIn();
                _userStorage.SaveUsername(UserName);
                _userStorage.SaveIsLogged(true);
                await GoToDashboard.Handle(Unit.Default);
            }
            
            return Observable.Return(lg);
        }
    }

    public static class LoginInputValidator
    {
        private const int MinimumPasswordLength = 1;
        public static bool Validate(string username, string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length > MinimumPasswordLength && !string.IsNullOrEmpty(username);
        }
    }
}