using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class RequestStateCollection
    {
        static public List<RequestState> RequestStateList = new List<RequestState>();
        public static void TakeRequestState()
        {
            RequestStateList.Clear();
            string query = "SELECT * FROM hotel.request_state;";
            List<Dictionary<string, string>> UR = Database.Select(query, RequestState.RequestStateKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    RequestState ur = new RequestState(Int32.Parse(item["id"]),
                                                        item["statement"]);
                    RequestStateList.Add(ur);
                }
            }
        }
        public static void UserTypeToBox(System.Windows.Controls.ComboBox CB)
        {
            CB.Items.Clear();
            foreach (RequestState ut in RequestStateList)
            {
                CB.Items.Add(ut.State);
            }
        }

        public static int NameToId(string _name)
        {
            RequestState tmp = RequestStateList.Find(x => x.State == _name);
            return tmp.Id;
        }
    }
}
