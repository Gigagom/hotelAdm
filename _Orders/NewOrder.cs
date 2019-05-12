using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class NewOrder
    {
        public static Client Client { get; set; }
        public static List<Apartment> Aparts = new List<Apartment>();
        public static int DaysCount { get; set; }
        public static double Price { get; set; }
        public static string StartDay { get; set; }
        public static string Date { get; set; }

        public static void Clear()
        {
            Client = null;
            Aparts.Clear();
        }

        public static void SetPrice()
        {
            double tmp = 0;
            foreach(Apartment item in Aparts)
            {
                tmp += item.price;
            }
            Price = tmp * DaysCount;
        }
        public static string GetApartList()
        {
            string tmp = null;
            foreach (Apartment item in Aparts)
            {
                tmp += $"{item.id}, ";
            }
            tmp = tmp.TrimEnd(new char[] { ',', ' ' });
            return tmp;
        }
        public static void Save()
        {
            try
            {
                int _id = 0;
                string query = $"call hotel.NewOrder({DaysCount},{Client.Id},{Price},'{StartDay}');";
                List<Dictionary<string, string>> UR = Database.Select(query, new string[] { "_last" });
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        _id =  Int32.Parse(item["_last"]);
                    }
                }
                query = null;
                foreach(Apartment a in Aparts)
                {
                    query += $"INSERT INTO `hotel`.`orders_to_apart_connect` (`order_id`, `apart_id`) VALUES ({_id}, {a.id}); ";
                }
                Database.Insert(query);

                query = null;
                query = $"call hotel.RemoveProducts({Aparts.Count*DaysCount});";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
