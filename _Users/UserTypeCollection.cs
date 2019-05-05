using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace hotelAdm
{
    class UserTypeCollection
    {
        static public List<UserType> UserTypeList = new List<UserType>();

        public static void TakeUserType()
        {
            UserTypeList.Clear();
            string query = "SELECT * FROM hotel.user_type;";
            List<Dictionary<string, string>> UR = Database.Select(query, UserType.UserTypeKeys);
            if (UR.Count != 0)
            {
                foreach (var item in UR)
                {
                    UserType ur = new UserType(Int32.Parse(item["id"]),
                                  item["type"]);
                    UserTypeList.Add(ur);
                }
            }
        }

        public static void UserTypeToBox(System.Windows.Controls.ComboBox CB)
        {
            CB.Items.Clear();
            foreach(UserType ut in UserTypeList)
            {
                CB.Items.Add(ut.Name);
            }
        }

        public static int NameToId(string _name)
        {
            UserType tmp = UserTypeList.Find(x=> x.Name == _name);
            return tmp.Id;
        }
    }
}
