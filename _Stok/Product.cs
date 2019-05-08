using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Product
    {
        public static string[] ProductKeys = new string[] { "id_prod", "product_name", "product_count", "Unit_name" };
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Unit { get; set; }
        public Product(int _id, string _name, int _count, string _unit)
        {
            Id = _id;
            Name = _name;
            Count = _count;
            Unit = _unit;
        }
    }
}
