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
    }
}
