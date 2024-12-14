using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApp;

namespace UserManagementApp
{
    public interface IUser
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Role { get; set; }

        void SayHello(User user);
    }
}

