using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Apartment
    {
        public static string[] ApartsKeysToSelect = new string[] { "id",
                                                                   "count_of_rooms",
                                                                   "room_type",
                                                                   "cleaning_time",
                                                                   "price",
                                                                   "is_busy" };

        public int id { get; set; }
        public int countOfRooms { get; set; }
        public string roomType { get; set; }
        public string cleaningTime { get; set; }
        public double price { get; set; }
        public byte isBusy { get; set; }
    }
}
