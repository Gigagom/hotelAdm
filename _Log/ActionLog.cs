using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class ActionLog
    {
        public static string[] ActionLogKeys = new string[] { "id", "time", "action_text", "table", "id_entity" };
        public int Id { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }
        public string Table { get; set; }
        public string IdInTable { get; set; }

        public ActionLog(int _id, string _time, string _text, string _table, string _idin)
        {
            Id = _id;
            Time = _time;
            Text = _text;
            Table = _table;
            IdInTable = _idin;
        }
    }
}
