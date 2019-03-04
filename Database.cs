using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace hotelAdm
{
    class Database
    {
        static MySqlConnection connection;
        static string server;
        static string database;
        static string user;
        static string password;

        static public void NewDB()
        {
            Database.server = ConfigChange.ReadSetting("DBServer");
            Database.database = ConfigChange.ReadSetting("DBName");
            Database.user = ConfigChange.ReadSetting("DBUser");
            Database.password = ConfigChange.ReadSetting("DBPassword");
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }
        static public void SetToTextboxes(TextBox Serv, TextBox DB, TextBox Us, TextBox Pass)
        {
            Serv.Text = Database.server;
            DB.Text = Database.database;
            Us.Text = Database.user;
            Pass.Text = Database.password;
        }

        static public void UpdateDB()
        {

        }
    }
}
