using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Order
    {
        public static string[] OrderKeys = new string[] { "id", "count_of_days", "guest_id", "price", "start_day", "date" };
        public int Id { get; set; }
        public string DaysCount { get; set; }
        public int GuestID { get; set; }
        public double Price { get; set; }
        public string StartDay { get; set; }
        public string Date { get; set; }
        public Order(int _id, string _daysCount, int _guestID, double _price, string _startDay, string _date)
        {
            Id = _id;
            DaysCount = _daysCount;
            GuestID = _guestID;
            Price = _price;
            StartDay = _startDay;
            Date = _date;
        }
    }
}
