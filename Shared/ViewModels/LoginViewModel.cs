using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PPCAndroid.Shared.Service;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Interactions

        public Interaction<Unit, bool> Confirm { get; }
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
        
        private ILogin _loginService;
        public ReactiveCommand<Unit,Unit> LoginCommand { get; private set; }
        
        public LoginViewModel(ILogin login)
        {
            _loginService = login;
            
            GoToDashboard= new Interaction<Unit, Unit>();
            Confirm = new Interaction<Unit, bool>();
            
            var canLogin = this.WhenAnyValue(x => x.UserName, x => x.Password, LoginInputValidator.Validate);
            LoginCommand = ReactiveCommand.CreateFromTask(async () => { await Login();  }, canLogin);
        }

        private async Task<IObservable<bool>> Login()
        {
            var lg = await _loginService.Login(_userName, _password);
            //TODO: refactor, druga funkcja Login a ta to TryLogin
            if (lg)
            {
                var confirmation = await Confirm.Handle(Unit.Default);

                if (confirmation)
                {
                    await GoToDashboard.Handle(Unit.Default);
                }
            }
            
            return Observable.Return(lg);
        }
    }

    public static class LoginInputValidator
    {
        private const int MinimumPasswordLength = 5;
        public static bool Validate(string username, string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length > MinimumPasswordLength && !string.IsNullOrEmpty(username);
        }
    }
}