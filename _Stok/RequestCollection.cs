using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class RequestCollection
    {
        static public List<Request> RequestsList = new List<Request>();
        public static void TakeRequests()
        {
            try
            {
                RequestsList.Clear();
                string query = "SELECT * FROM hotel.requests_to_providers;";
                List<Dictionary<string, string>> UR = Database.Select(query, Request.RequestKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Request ur = new Request(Int32.Parse(item["id"]),
                                                Int32.Parse(item["product_id"]),
                                                Int32.Parse(item["product_count"]),
                                                Int32.Parse(item["provider_id"]),
                                                Int32.Parse(item["state_id"]));
                        RequestsList.Add(ur);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }

        }

        public static void RequestSsToDG(System.Windows.Controls.DataGrid DG)
        {
            DG.Items.Clear();
            foreach (Request p in RequestsList)
            {
                DG.Items.Add(p);
            }
        }
    }
}
