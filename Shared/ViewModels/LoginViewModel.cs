using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.Util;
using PPCAndroid.Shared.Service;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private ILogin _loginService;
        
        private string _userName;
        public string UserName
        {
            get
            {
                Log.WriteLine(LogPriority.Info,"login","zmienilem username");
                return _userName;
            }
            
            //notify when property user name changes
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }


        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }
 
        public ReactiveCommand<Unit,Unit> LoginCommand { get; private set; }
        
        public LoginViewModel(ILogin login)
        {
            _loginService = login;
            
            var canLogin = this.WhenAnyValue(x => x.UserName, x => x.Password, LoginInputValidator.Validate);
            LoginCommand = ReactiveCommand.CreateFromTask(async () => { await Login(); }, canLogin);
        }

        private async Task<IObservable<bool>> Login()
        {
            Log.WriteLine(LogPriority.Info, "login", "tworze taska");
            var lg = await _loginService.Login(_userName, _password);

            return Observable.Return(lg);
                
            if (lg)
            {
                Log.WriteLine(LogPriority.Info, "login", "zmieniam ekran");
                //TODO: przejście na inny ekran
            }
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