namespace Shared.ViewModels
{
    public static class LoginInputValidator
    {
        private const int MinimumPasswordLength = 0;
        public static bool Validate(string username, string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length > MinimumPasswordLength && !string.IsNullOrEmpty(username);
        }
    }
}