using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class ClientCollection
    {
        static public List<Client> ClientsList = new List<Client>();
        public static void TakeClients()
        {
            try
            {
                ClientsList.Clear();
                string query = "SELECT * FROM hotel.guests;";
                List<Dictionary<string, string>> UR = Database.Select(query, Client.ClientKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Client ur = new Client(Int32.Parse(item["id"]),
                                                item["guest_name"],
                                                item["passport_num"],
                                                item["guest_tel"]);
                        ClientsList.Add(ur);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message+ex.Source+ex.TargetSite);
            }
            
        }
        public static void AddClient(string _name, string _pass, string _tel)
        {
            try
            {
                string query = $"INSERT INTO `hotel`.`guests` (`guest_name`, `passport_num`, `guest_tel`) VALUES ('{_name}', '{_pass}', '{_tel}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string TakePassById(int _id)
        {
            try
            {
                string query = $"SELECT passport_num FROM hotel.guests where id = {_id};";
                List<Dictionary<string, string>> UR = Database.Select(query, new string[] { "passport_num" });
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        return item["passport_num"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }
            return null;
        }
        public static Client TakeClientByPass(string _pass)
        {
            try
            {
                Client tmp = ClientsList.Find(x => x.Passport_number == _pass);
                return tmp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }
            return null;
        }
        public static void ClientsToDG(DataGrid DG)
        {
            DG.Items.Clear();
            foreach (Client p in ClientsList)
            {
                DG.Items.Add(p);
            }
        }
    }
}
