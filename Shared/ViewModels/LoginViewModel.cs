using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.Util;
using PPCAndroid;
using PPCAndroid.Shared.Service;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly Interaction<Unit, bool> _confirm;
        public Interaction<Unit, bool> Confirm => this._confirm;
        
        private ILogin _loginService;
        
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
        

        //TODO: Kamil, tu te pierdoły, dodatkowy properties bo coś próbowałem kombinować
        private readonly ObservableAsPropertyHelper<IEnumerable<WifiNetwork>> _wifiList;
        public IEnumerable<WifiNetwork> WifiList => _wifiList.Value;

        /*private IEnumerable<WifiNetwork> _wifiNetworks;
        public IEnumerable<WifiNetwork> WifiNetworks
        {
            get { return _wifiNetworks; }
            set => this.RaiseAndSetIfChanged(ref _wifiNetworks,value);
        }*/


        public ReactiveCommand<Unit,Unit> LoginCommand { get; private set; }
        
        public LoginViewModel(ILogin login, IObservable<IEnumerable<WifiNetwork>> wifiNetworksObs)
        {
            _loginService = login;
            this._confirm = new Interaction<Unit, bool>();
            
            /*_wifiList = this
                .WhenAnyValue(x => x.WifiNetworks)
                .ToProperty(this, x => x.WifiList);*/
            //this.WhenAnyValue(x => wifiNetworksObs).ToProperty<LoginViewModel,IEnumerable<WifiNetwork>>(this, x => x.WifiList, out _wifiList);
            //TODO: Kamil, no cóż xd
            wifiNetworksObs.Subscribe(Observer.Create<IEnumerable<WifiNetwork>>(n =>
            {
             //TODO: napisać nexta  
             
            }));
            
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
                    //TODO: przejście na inny ekran  
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