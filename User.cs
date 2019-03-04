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
        string login;
        string password;
        string Name;
        string Position;
        public User()
        {
            this.login = "Admin";
            this.password = "Admin";
            this.Name = "Admin";
            this.Position = "Admin";
        }
        public User(string login, string password,  string Name, string Position)
        {
            this.login = login;
            this.password = password;
            this.Name = Name;
            this.Position = Position;
        }
        public string GetName()
        {
            return this.Name;
        }
        public void SetLabels(Label LName, Label LPosition)
        {
            LName.Content = this.Name;
            LPosition.Content = this.Position;
        }

        public void UpdateUser()
        {

        }
    }
}
