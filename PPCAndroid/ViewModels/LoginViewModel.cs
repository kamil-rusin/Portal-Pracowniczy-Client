using System.Text.RegularExpressions;
using PPCAndroid.Services;
using ReactiveUI;


namespace PPCAndroid.ViewModels
{
    public class LoginViewModel : ViewModelBase
   {
       ILogin _loginService;

       private string _userName;
       public string UserName
       {
           get => _userName;
           //Notify when property user name changes
           set => this.RaiseAndSetIfChanged(ref _userName, value);
       }

       private string _password;
       public string Password
       {
           get => _password;
           set => this.RaiseAndSetIfChanged(ref _password, value);
       }

       /// <summary>
       /// This is an Oaph Observable propperty helper, 
       /// Which is used to determine whether a subsequent action
       /// Could be performed or not depending on its value
       /// This condition is calculated every time its value changes.
       /// </summary>
       ObservableAsPropertyHelper<bool> _validLogin;
       public bool ValidLogin
       {
           get { return _validLogin?.Value ?? false; }
       }
       
       public ReactiveCommand LoginCommand { get; private set; }
       
       public LoginViewModel(ILogin login, IScreen hostScreen = null) : base(hostScreen)
       {
           _loginService = login;

           ///When there is a change in either the password or email, 
           ///This is been fired and if it is validated according to 
           ///What is shown below, the ValidLogin Property is been updated.
           this.WhenAnyValue(x => x.UserName, x => x.Password,
               (email, password) =>
               (
                   ///Validate the password
                   !string.IsNullOrEmpty(password) && password.Length > 5
               )
               &&
               (
                   ///Validate teh email.
                   !string.IsNullOrEmpty(email)
                           &&
                    Regex.Matches(email, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$").Count == 1
               ))
               .ToProperty(this, v => v.ValidLogin, out _validLogin);

           ///We add the logic first, describing what the login command will perfom when ran
           ///And we add a CamExecute clause which states that the command will be able to execute only 
           ///If Email and Password are valid, that is, when ValidLogin has a value of true.
           LoginCommand = ReactiveCommand.CreateFromTask(async () =>
           {

               var lg = await login.Login(_userName, _password);
               if (lg)
               {
                   HostScreen.Router
                               .Navigate
                               .Execute(new ItemsViewModel())
                               .Subscribe();
               }
           }, this.WhenAnyValue(x => x.ValidLogin, x => x.ValidLogin, (validLogin, valid) => ValidLogin && valid));

       }
   }
}