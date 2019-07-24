using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;

namespace PPCAndroid.Shared.Service
{
    public class LoginService: ILogin
    {
        readonly Dictionary<string, string> _userCredentials;

        public LoginService()
        {
            _userCredentials = new Dictionary<string, string>
            {
                {"admin1", "admin1"}, 
                {"quest", "iamquest"}, 
                {"wojciech", "wojciech"},
                {"a", "a"}
            };
        }

        public Task<bool> Login(string username, string password)
        {
            return !_userCredentials.TryGetValue(username, out var maybePassword) ? Task.FromResult(false) : Task.FromResult(password == maybePassword);
        }
    }
}