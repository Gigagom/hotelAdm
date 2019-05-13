using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hotelAdm
{
    class ActionLogCollection
    {
        static public List<ActionLog> ActionLogList = new List<ActionLog>();

        public static void TakeActionLog(DataGrid DG)
        {
            ActionLogList.Clear();
            string query = "SELECT * FROM hotel.action_log;";
            List<Dictionary<string, string>> UR = Database.Select(query, ActionLog.ActionLogKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    ActionLog ur = new ActionLog(Int32.Parse(item["id"]),
                                  item["time"],
                                  item["action_text"],
                                  item["table"],
                                  item["id_entity"]);
                    ActionLogList.Add(ur);
                }
            }
            SetActionLogData(DG);
        }

        private static void SetActionLogData(DataGrid DG)
        {
            DG.Items.Clear();
            foreach (ActionLog ur in ActionLogList)
            {
                DG.Items.Add(ur);
            }
        }
    }
}
