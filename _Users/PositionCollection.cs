using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class PositionCollection
    {
        static public List<Position> PositionList = new List<Position>();

        public static void TakePositions()
        {
            PositionList.Clear();
            string query = "SELECT * FROM hotel.positions;";
            List<Dictionary<string, string>> UR = Database.Select(query, Position.PositionKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    Position ur = new Position(Int32.Parse(item["id"]),
                                  item["position_name"]);
                    PositionList.Add(ur);
                }
            }
        }

        public static void PositionsToBox(ComboBox CB)
        {
            CB.Items.Clear();
            foreach (Position p in PositionList)
            {
                CB.Items.Add(p.Name);
            }
        }

        public static int NameToId(string _name)
        {
            Position tmp = PositionList.Find(x => x.Name == _name);
            return tmp.Id;
        }
    }
}
