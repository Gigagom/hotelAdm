using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class CleaningTime
    {
        public static string[] CleaningTimeKeys = new string[] { "id", "cleaning_time" };
        public int Id { get; set; }
        public string Name { get; set; }

        public CleaningTime(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
