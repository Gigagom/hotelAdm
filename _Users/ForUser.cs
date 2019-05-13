using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace hotelAdm
{
    class Users
    {
        public static List<User> UsersList = new List<User>();
        
        public static void ClearUsersList()
        {
            UsersList.Clear();
        }

        public static void TakeAllUsers(DataGrid DG)
        {
            UsersList.Clear();
            string query = "call hotel.Select_All_Users();";
            List<Dictionary<string, string>> UR = Database.Select(query, User.UsersKeysForSelects);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    User ur = new User(Int32.Parse(item["id"]),
                                  item["login"],
                                  item["password"],
                                  item["FIO"],
                                  item["type"],
                                  item["position_name"]);
                    UsersList.Add(ur);
                }
            }
            SetUsersData(DG);
        }

        private static void SetUsersData(DataGrid DG)
        {
            DG.Items.Clear();
            foreach (User ur in UsersList)
            {
                DG.Items.Add(ur);
            }
        }

        public static void CreateUser(string _login, string _password, string _fio, string _user_type_text, string _position_text)
        {
            try
            {
                int _type_id = UserTypeCollection.NameToId(_user_type_text);
                int _position_id = PositionCollection.NameToId(_position_text);
                string query = $"call hotel.new_user('{_login}', '{_password}', '{_fio}', '{_type_id}','{_position_id}');";
                Database.Insert(query);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateUser(int _id,string _login, string _password, string _fio, string _user_type_text, string _position_text)
        {
            try
            {
                int _type_id = UserTypeCollection.NameToId(_user_type_text);
                int _position_id = PositionCollection.NameToId(_position_text);
                string query = $"call hotel.update_user('{_id}','{_login}', '{_password}', '{_fio}', '{_type_id}','{_position_id}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteUser(int id)
        {
            string query = $"DELETE FROM `hotel`.`users` WHERE (`id` = '{id.ToString()}');";
            try
            {
                Database.Delete(query);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    class CurrrentUser
    {
        public static int id { get; set; }
        public static string login { get; set; }
        public static string password { get; set; }
        public static string Name { get; set; }
        public static string Type { get; set; }
        public static string Position { get; set; }
        static public void NewUser()
        {
            CurrrentUser.id = 1;
            CurrrentUser.login = "Admin";
            CurrrentUser.password = "Admin";
            CurrrentUser.Name = "Admin";
            CurrrentUser.Type = "Admin";
            CurrrentUser.Position = "Admin";
        }
        static public void NewUser(int Id, string Login, string Password, string Fio, string Type, string Position)
        {
            CurrrentUser.id = Id;
            CurrrentUser.login = Login;
            CurrrentUser.password = Password;
            CurrrentUser.Name = Fio;
            CurrrentUser.Type = Type;
            CurrrentUser.Position = Position;
        }
        static public void UpdateCurrentUser(string login, string password,  string Name, string Position)
        {
            CurrrentUser.login = login;
            CurrrentUser.password = password;
            CurrrentUser.Name = Name;
            CurrrentUser.Position = Position;
        }
        public string GetName()
        {
            return CurrrentUser.Name;
        }
        static public void SetLabels(Label LName, Label LPosition)
        {
            LName.Content = CurrrentUser.Name;
            LPosition.Content = CurrrentUser.Position;
        }

        static public bool Authorization(string login, string password)
        {
            try
            {
                string query = "call hotel.Autorization_data('" + login + "','" + password + "');";
                List<Dictionary<string, string>> UR = Database.Select(query, User.UsersKeysForSelects);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        CurrrentUser.id = Int32.Parse(item["id"]);
                        CurrrentUser.login = item["login"];
                        CurrrentUser.password = item["password"];
                        CurrrentUser.Name = item["FIO"];
                        CurrrentUser.Type = item["type"];
                        CurrrentUser.Position = item["position_name"];                        
                    }
                    LogAuthorization();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }         
        }

        static private void LogAuthorization()
        {
            try
            {
                string query = $"INSERT INTO `hotel`.`action_log` (`time`, `action_text`, `table`, `id_entity`) VALUES (now(), 'Авторизован пользователь', 'users', {id});";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
