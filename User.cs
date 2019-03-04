using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class User
    {
        static string login;
        static string password;
        static string Name;
        static string Position;
        static public void NewUser()
        {
            User.login = "Admin";
            User.password = "Admin";
            User.Name = "Admin";
            User.Position = "Admin";
        }
        static public void UpdateUser(string login, string password,  string Name, string Position)
        {
            User.login = login;
            User.password = password;
            User.Name = Name;
            User.Position = Position;
        }
        public string GetName()
        {
            return User.Name;
        }
        static public void SetLabels(Label LName, Label LPosition)
        {
            LName.Content = User.Name;
            LPosition.Content = User.Position;
        }

        static public void UpdateUser()
        {

        }
    }
}
