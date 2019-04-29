using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class UserType
    {
        public static string[] UserTypeKeys = new string[] { "id", "type" };
        public int Id { get; set; }
        public string Name { get; set; }

        public UserType(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
