using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace hotelAdm
{
    class HotelApartments
    {
        public static Dictionary<int, Apartment> Apartsments = new Dictionary<int, Apartment>();
        public static int ApartmentsCount;

        public static void TakeApartments()
        {
            Apartsments.Clear();
            try
            {
                string query = "call hotel.GetAparts();";
                List<Dictionary<string, string>> Aparts = Database.Select(query, Apartment.ApartsKeysToSelect);
                if (Aparts.Count != 0)
                {
                    foreach (var item in Aparts)
                    {
                        Apartment tmp = new Apartment();
                        tmp.id = Int32.Parse(item["id"]);
                        tmp.countOfRooms = Int32.Parse(item["count_of_rooms"]);
                        tmp.roomType = item["room_type"];
                        tmp.cleaningTime = item["cleaning_time"];
                        tmp.price = Double.Parse(item["price"]);
                        tmp.isBusy = Byte.Parse(item["is_busy"]);
                        Apartsments.Add(tmp.id,tmp);                        
                    }                        
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ApartmentsCount = Apartsments.Count();
            }
        }

        public static void SetApartmentsToGrid(DataGrid dg)
        {
            foreach(KeyValuePair<int, Apartment> a in Apartsments)
            {
                dg.Items.Add(a.Value);
            }
        }

        public static void DeleteApartsment(int id)
        {
            string query = $"DELETE FROM `hotel`.`hotel_apart` WHERE (`id` = '{id.ToString()}');";
            try
            {
                Database.Delete(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
