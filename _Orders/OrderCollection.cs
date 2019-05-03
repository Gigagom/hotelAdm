using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class OrderCollection
    {
        static public List<Order> OrderList = new List<Order>();
        public static void TakeOrders()
        {
            try
            {
                OrderList.Clear();
                string query = "SELECT * FROM hotel.orders;";
                List<Dictionary<string, string>> UR = Database.Select(query, Order.OrderKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Order ur = new Order(Int32.Parse(item["id"]),
                                             item["count_of_days"],
                                             Int32.Parse(item["guest_id"]),
                                             Convert.ToDouble(item["price"]),
                                             item["start_day"],
                                             item["date"]);
                        OrderList.Add(ur);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }

        }
        public static void OrdersToDG(DataGrid DG)
        {
            DG.Items.Clear();
            foreach (Order p in OrderList)
            {
                DG.Items.Add(p);
            }
        }
    }
}
