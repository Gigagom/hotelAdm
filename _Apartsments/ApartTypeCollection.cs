using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class ApartTypeCollection
    {
        static public List<ApartType> ApartTypeList = new List<ApartType>();

        public static void TakeApart()
        {
            ApartTypeList.Clear();
            string query = "SELECT * FROM hotel.room_types;";
            List<Dictionary<string, string>> UR = Database.Select(query, ApartType.ApartTypeKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    ApartType ur = new ApartType(Int32.Parse(item["id"]),
                                  item["room_type"]);
                    ApartTypeList.Add(ur);
                }
            }
        }

        public static void ApartToBox(ComboBox CB)
        {
            CB.Items.Clear();
            foreach (ApartType p in ApartTypeList)
            {
                CB.Items.Add(p.Name);
            }
        }

        public static int NameToId(string _name)
        {
            ApartType tmp = ApartTypeList.Find(x => x.Name == _name);
            return tmp.Id;
        }
    }
}
