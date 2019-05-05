using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class RequestState
    {
        public static string[] RequestStateKeys = new string[] { "id", "statement" };
        public int Id { get; set; }
        public string State { get; set; }

        public RequestState(int _id, string _state)
        {
            Id = _id;
            State = _state;
        }
    }
}
