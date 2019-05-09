using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Request
    {
        public static string[] RequestKeys = new string[] { "id", "product_name", "product_count", "provider_name", "statement" };
        public int Id { get; set; }
        public string ProdName { get; set; }
        public int Count { get; set; }
        public string ProviderName { get; set; }
        public string State { get; set; }
        public Request(int _id, string _prod, int _count, string _provider, string _state)
        {
            Id = _id;
            ProdName = _prod;
            Count = _count;
            ProviderName = _provider;
            State = _state;
        }
    }
}
