using System.Reactive;
using System.Reactive.Linq;
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
            get => _userName;
            //notify when property user name changes
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }


        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        ObservableAsPropertyHelper<bool> _validLogin;
        public bool ValidLogin
        {
            get { return _validLogin?.Value ?? false; }
        }
 
        public ReactiveCommand<Unit,Unit> LoginCommand { get; private set; }
        
        //wycialem hostscreen z argumentow
        public LoginViewModel(ILogin login) : base()
        {
            _loginService = login;

            this.WhenAnyValue(x => x.UserName, x => x.Password,
                    (username, password) =>
                        (
                            //Validate the password
                            !string.IsNullOrEmpty(password) && password.Length > 5
                        )
                        &&
                        (
                            //Validate the username
                            !string.IsNullOrEmpty(username)
                        ))
                .ToProperty(this, v => v.ValidLogin, out _validLogin);

            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {

                var lg = await login.Login(_userName, _password);
                if (lg)
                {
                    //TODO: przejście na inny ekran
                }
            }, this.WhenAnyValue(x => x.ValidLogin, x => x.ValidLogin, (validLogin, valid) => ValidLogin && valid));

        }
    }
}