using System.Collections.Generic;
using System.Threading.Tasks;

namespace PPCAndroid.Shared.Service
{
    public class LoginService: ILogin
    {
        Dictionary<string, string> _userCredentials;

        public LoginService()
        {
            _userCredentials = new Dictionary<string, string>();
            _userCredentials.Add("admin", "admin");
            _userCredentials.Add("mietek", "mietek");
            _userCredentials.Add("marek", "marek");
        }

        public async Task<bool> Login(string username, string password)
        {
            if(_userCredentials.ContainsKey(username))
            {
                return _userCredentials[username] == password;
            }

            return false;
        }
    }
}