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
                string query = "call hotel.GetRequests();";
                List<Dictionary<string, string>> UR = Database.Select(query, Request.RequestKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Request ur = new Request(Int32.Parse(item["id"]),
                                                item["product_name"],
                                                Int32.Parse(item["product_count"]),
                                                item["provider_name"],
                                                item["statement"]);
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

        public static void CreateRequest(int _prod, int _count, int _provider)
        {
            
            try
            {
                string query = $"INSERT INTO `hotel`.`requests_to_providers` (`product_id`, `product_count`, `provider_id`, `state_id`) VALUES('{_prod}', '{_count}', '{_provider}', '1');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CloseRequest(int request_id, int product_id, int _count)
        {
            try
            {
                string query = $"call hotel.Close_A_Request({request_id}, {product_id}, {_count});";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteRequest(int _id)
        {            
            try
            {
                string query = $"DELETE FROM `hotel`.`requests_to_providers` WHERE(`id` = '{_id}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
