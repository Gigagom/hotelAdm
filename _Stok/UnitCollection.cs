using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class UnitCollection
    {
        static public List<Unit> UnitList = new List<Unit>();

        public static void TakeUnits()
        {
            UnitList.Clear();
            string query = "SELECT * FROM hotel.units;";
            List<Dictionary<string, string>> UR = Database.Select(query, Unit.UnitKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    Unit ur = new Unit(Int32.Parse(item["id"]),
                                  item["Unit_name"]);
                    UnitList.Add(ur);
                }
            }
        }

        public static void UnitsToBox(System.Windows.Controls.ComboBox CB)
        {
            CB.Items.Clear();
            foreach (Unit ut in UnitList)
            {
                CB.Items.Add(ut.Name);
            }
        }

        public static int NameToId(string _name)
        {
            Unit tmp = UnitList.Find(x => x.Name == _name);
            return tmp.Id;
        }
    }
}
