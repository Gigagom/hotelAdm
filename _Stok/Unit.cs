using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Unit
    {
        public static string[] UnitKeys = new string[] { "id", "Unit_name" };
        public int Id { get; set; }
        public string Name { get; set; }

        public Unit(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
