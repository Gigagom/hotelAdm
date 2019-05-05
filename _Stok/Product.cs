using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Product
    {
        public static string[] ProductKeys = new string[] { "id", "product_name", "product_count", "unit_id" };
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int UnitId { get; set; }
        public Product(int _id, string _name, int _count, int _unitId)
        {
            Id = _id;
            Name = _name;
            Count = _count;
            UnitId = _unitId;
        }
    }
}
