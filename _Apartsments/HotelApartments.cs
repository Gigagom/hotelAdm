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
            dg.Items.Clear();
            foreach (KeyValuePair<int, Apartment> a in Apartsments)
            {
                dg.Items.Add(a.Value);
            }
        }

        public static void CreateApartsment(int _rooms, string _type, string _time, double _cost)
        {
            try
            {
                int _type_id = ApartTypeCollection.NameToId(_type);
                int _time_id = CleaningTimeCollection.NameToId(_time);
                string query = $"INSERT INTO `hotel`.`hotel_apart` (`count_of_rooms`, `id_room_type`, `cleaning_time_id`, `price`) VALUES ('{_rooms}', '{_type_id}', '{_time_id}', '{_cost}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateApartsment(int _id,int _rooms, string _type, string _time, double _cost)
        {
            try
            {
                int _type_id = ApartTypeCollection.NameToId(_type);
                int _time_id = CleaningTimeCollection.NameToId(_time);
                string query = $"UPDATE `hotel`.`hotel_apart` SET `count_of_rooms` = '{_rooms}', `id_room_type` = '{_type_id}', `cleaning_time_id` = '{_time_id}', `price` = '{_cost}' WHERE (`id` = '{_id}')";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public static string GetApartsByOrder(int _id)
        {
            try
            {
                string result = null;
                string query = $"SELECT apart_id FROM hotel.orders_to_apart_connect where order_id = {_id} order by apart_id;";
                List<Dictionary<string, string>> UR = Database.Select(query, new string[] { "apart_id" });
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        result += $"{item["apart_id"]}, ";
                    }
                }
                result = result.TrimEnd(new char[] { ',', ' '});
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }
        }

        public static Apartment GetApartById(int _id)
        {
            Apartment tmp = Apartsments[_id];
            return tmp;
        }
    }
}
