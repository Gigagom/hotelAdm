using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class User
    {
        public static string[] UsersKeysForSelects = new string[] { "id",
                                                                    "login",
                                                                    "password",
                                                                    "FIO",
                                                                    "type",
                                                                    "position_name"};
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string FIO { get; set; }
        public string type { get; set; }
        public string position_name { get; set; }

        public User(int Id, string Login, string Password, string Fio, string Type, string Position)
        {
            id = Id;
            login = Login;
            password = Password;
            FIO = Fio;
            type = Type;
            position_name = Position;
        }
    }
}
