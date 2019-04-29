using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Position
    {
        public static string[] PositionKeys = new string[] { "id", "position_name" };
        public int Id { get; set; }
        public string Name { get; set; }

        public Position(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
