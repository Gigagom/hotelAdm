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
    }
}
