using System.Reactive;
using System.Reactive.Linq;
using Model;
using ReactiveUI;

namespace Shared.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly LoginModel _loginModel;
        
        public string Username
        {
            get => _loginModel.Username;
            set => _loginModel.Username = value;
        }

        public string Password
        {
            get => _loginModel.Password;
            set => _loginModel.Password = value;
        }

        public LoginViewModel(LoginModel loginModel)
        {
            _loginModel = loginModel;
            CheckCommand = ReactiveCommand.Create(CheckData);

            _checkResult = CheckCommand
                .ToProperty(this, x => x.CheckResult);
        }

        //??? co to robi
        private readonly ObservableAsPropertyHelper<bool> _checkResult;

        public bool CheckResult => _checkResult.Value;
        
        public ReactiveCommand<Unit, bool> CheckCommand { get; }
        
        
        private bool CheckData()
        {
            return _loginModel.CheckData();
        }
    }
}