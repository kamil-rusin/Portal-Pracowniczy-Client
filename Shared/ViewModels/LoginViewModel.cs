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
        private Interaction<Unit, bool> _confirm;
        private Interaction<Unit, bool> _goToDashboard;
        public Interaction<Unit, bool> Confirm => this._confirm;
        public Interaction<Unit, bool> GoToDashboard => this._goToDashboard;
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
            
            this._goToDashboard = new Interaction<Unit, bool>();
            this._confirm = new Interaction<Unit, bool>();
            
            var canLogin = this.WhenAnyValue(x => x.UserName, x => x.Password, LoginInputValidator.Validate);
            LoginCommand = ReactiveCommand.CreateFromTask(async () => { await Login();  }, canLogin);
        }

        private async Task<IObservable<bool>> Login()
        {
            var lg = await _loginService.Login(_userName, _password);

            if (lg)
            {
                var confirmation = await _confirm.Handle(new Unit());

                if (confirmation)
                {
                    //TODO: przejście na inny ekran, tu jest błąd przy rejestracji interakcji  
                     _goToDashboard.Handle(new Unit());
                }
            }
            //TODO: should it return something?
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