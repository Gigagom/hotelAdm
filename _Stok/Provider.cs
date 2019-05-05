using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Provider
    {
        public static string[] ProviderKeys = new string[] { "id", "provider_name", "provider_tel" };
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Provider(int _id, string _name, string _phone)
        {
            Id = _id;
            Name = _name;
            Phone = _phone;
        }
    }
}
