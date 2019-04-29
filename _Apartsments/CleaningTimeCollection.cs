using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class CleaningTimeCollection
    {
        static public List<CleaningTime> CleaningTimeCollectionList = new List<CleaningTime>();

        public static void TakeTime()
        {
            CleaningTimeCollectionList.Clear();
            string query = "SELECT * FROM hotel.cleaning_time;";
            List<Dictionary<string, string>> UR = Database.Select(query, CleaningTime.CleaningTimeKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    CleaningTime ur = new CleaningTime(Int32.Parse(item["id"]),
                                  item["cleaning_time"]);
                    CleaningTimeCollectionList.Add(ur);
                }
            }
        }

        public static void TimeToBox(ComboBox CB)
        {
            CB.Items.Clear();
            foreach (CleaningTime p in CleaningTimeCollectionList)
            {
                CB.Items.Add(p.Name);
            }
        }

        public static int NameToId(string _name)
        {
            CleaningTime tmp = CleaningTimeCollectionList.Find(x => x.Name == _name);
            return tmp.Id;
        }
    }
}
