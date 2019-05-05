using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class Request
    {
        public static string[] RequestKeys = new string[] { "id", "product_id", "product_count", "provider_id", "state_id" };
        public int Id { get; set; }
        public int ProdId { get; set; }
        public int Count { get; set; }
        public int ProviderId { get; set; }
        public int StateId { get; set; }
        public Request(int _id, int _prodId, int _count, int _providerId, int _stateId)
        {
            Id = _id;
            ProdId = _prodId;
            Count = _count;
            ProviderId = _providerId;
            StateId = _stateId;
        }
    }
}
