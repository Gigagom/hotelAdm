using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Client
    {
        public static string[] ClientKeys = new string[] { "id", "guest_name", "passport_num", "guest_tel" };
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Passport_number { get; set; }
        public string Phone { get; set; }
        public Client(int _id, string _name, string _pass, string _phone)
        {
            Id = _id;
            Name = _name;
            Passport_number = _pass;
            Phone = _phone;
        }
    }
}
