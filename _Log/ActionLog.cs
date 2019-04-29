using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class ActionLog
    {
        public static string[] ActionLogKeys = new string[] { "id", "time", "action_text" };
        public int Id { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }

        public ActionLog(int _id, string _time, string _text)
        {
            Id = _id;
            Time = _time;
            Text = _text;
        }
    }
}
