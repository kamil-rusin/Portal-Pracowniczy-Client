namespace Model
{
    public class LoginModel
    {
        private string _user = "admin";
        private string _pswd = "admin";
        
        public string Username { get; set; }

        public string Password { get; set; }

        public bool CheckData()
        {
            if (Username != _user) return false;
            return Password == _pswd;
        }
    }
}