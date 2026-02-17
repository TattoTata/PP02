using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ПП._2
{
    public class AuthService
    {
        private readonly Dictionary<string, (string Password, string Role)> _users;

        public AuthService()
        {
            _users = new Dictionary<string, (string, string)>
        {
            { "admin", ("123", "директор") },
            { "manager", ("111", "менеджер") }
        };
        }

        public string Login(string login, string password)
        {
            if (_users.ContainsKey(login) && _users[login].Password == password)
                return _users[login].Role;

            return null;
        }

        public bool Register(string login, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email))
                return false;

            if (_users.ContainsKey(login))
                return false;

            _users.Add(login, (password, "менеджер"));
            return true;
        }
    }
}
