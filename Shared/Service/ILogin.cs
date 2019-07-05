using System.Threading.Tasks;

namespace PPCAndroid.Shared.Service
{
    public interface ILogin
    {
        Task<bool> Login(string username, string password);
    }
}