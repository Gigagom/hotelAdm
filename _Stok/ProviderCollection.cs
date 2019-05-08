using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class ProviderCollection
    {
        static public List<Provider> ProviderList = new List<Provider>();
        public static void TakeProviders()
        {
            try
            {
                ProviderList.Clear();
                string query = "SELECT * FROM hotel.providers;";
                List<Dictionary<string, string>> UR = Database.Select(query, Provider.ProviderKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Provider ur = new Provider(Int32.Parse(item["id"]),
                                                item["provider_name"],
                                                item["provider_tel"]);
                        ProviderList.Add(ur);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }

        }
        public static void ProvidersToDG(System.Windows.Controls.DataGrid DG)
        {
            DG.Items.Clear();
            foreach (Provider p in ProviderList)
            {
                DG.Items.Add(p);
            }
        }

        public static void CreateProvider(string _name, string _tel)
        {
            try
            {
                string query = $"INSERT INTO `hotel`.`providers` (`provider_name`, `provider_tel`) VALUES ('{_name}', '{_tel}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateProvider(int _id, string _name, string _tel)
        {
            try
            {
                string query = $"UPDATE `hotel`.`providers` SET `provider_name` = '{_name}', `provider_tel` = '{_tel}' WHERE (`id` = '{_id}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteProvider(int _id)
        {
            try
            {
                string query = $"DELETE FROM `hotel`.`providers` WHERE (`id` = '{_id}');";
                Database.Delete(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
