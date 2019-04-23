using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace hotelAdm
{
    class Database
    {
        static MySqlConnection connection;
        static string connectionString;
        static string server;
        static string port;
        static string databaseName;
        static string user;
        static string password;

        static public void SetDBSettings()
        {
            server = ConfigChange.ReadSetting("DBServer");
            port = ConfigChange.ReadSetting("DBPort");
            databaseName = ConfigChange.ReadSetting("DBName");
            user = ConfigChange.ReadSetting("DBUser");
            password = ConfigChange.ReadSetting("DBPassword");
            connectionString = "datasource="+ server + 
                               ";port="+ port +
                               ";username="+ user + 
                               ";password="+password;
            connection = new MySqlConnection(connectionString);
        }
        static public void SetToTextboxes(TextBox Serv, TextBox Port, TextBox DB, TextBox Us, TextBox Pass)
        {
            Serv.Text = Database.server;
            Port.Text = Database.port;
            DB.Text = Database.databaseName;
            Us.Text = Database.user;
            Pass.Text = Database.password;
        }

        static public bool CheckConnection()
        {
            bool result = false;
            try
            {
                connection.Open();
                result = true;
                connection.Close();
            }
            catch       
            {
                result = false;
            }
            return result;
         }

        //Открытие подключения к БД
        //Две наиболее распростаненные ошибки
        //0: Невозможно подключиться к серверу.
        //1045: Некорректный логин или пароль.
        static private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                string message = "";
                switch (ex.Number)
                {
                    case 0:
                        message = "Невозможно подключиться к серверу. Свяжитесь с администратором.";
                        break;
                    case 1045:
                        message = "Неверный логин или пароль для доступа к БД. Свяжитесь с администратором.";
                        break;
                    case 1042:
                        message = "Невозможно подключиться к серверу. Проверьте подключение или свяжитесь с администратором.";
                        break;
                    default:
                        message = ex.Message+" №:"+ex.Number.ToString();
                        break;
                }
                throw new Exception(message);
            }
        }

        static private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        static public void Insert(string query)
        {
            try
            {
                if (Database.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();                    
                }
            }
            finally
            {
                Database.CloseConnection();
            }
           
        }
        static public void InsertSomeQueryes(string[] queryes)
        {
            try
            {
                if (Database.OpenConnection())
                {
                    string bigQuery = "";
                    foreach (var query in queryes)
                    {
                        bigQuery += query;
                    }
                    MySqlCommand cmd = new MySqlCommand(bigQuery, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                Database.CloseConnection();
            }            
        }

        static public void Update(string query)
        {
            try
            {
                if (Database.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                Database.CloseConnection();
            }            
        }

        static public void Delete(string query)
        {
            try
            {
                if (Database.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                Database.CloseConnection();
            }
        }
        static public List<Dictionary<string,string>> Select(string query, string[] keys)
        {
            List <Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            try
            {
                if (Database.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int keyId = 0;
                        Dictionary<string, string> temp = new Dictionary<string, string>();
                        foreach (string key in keys)
                        {
                            temp.Add(keys[keyId], dataReader[key].ToString());
                            keyId++;
                        }
                        list.Add(temp);
                    }
                    dataReader.Close();
                    Database.CloseConnection();
                    return list;
                }
                else
                {
                    return list;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
