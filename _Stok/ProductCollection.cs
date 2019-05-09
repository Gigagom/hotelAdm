using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotelAdm
{
    class ProductCollection
    {
        static public List<Product> ProductsList = new List<Product>();

        public static void TakeProducts()
        {
            try
            {
                ProductsList.Clear();
                string query = "call hotel.GetProducts();";
                List<Dictionary<string, string>> UR = Database.Select(query, Product.ProductKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Product ur = new Product(Int32.Parse(item["id_prod"]),
                                                item["product_name"],
                                                Int32.Parse(item["product_count"]),
                                                item["Unit_name"]);
                        ProductsList.Add(ur);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.Source + ex.TargetSite);
            }

        }

        public static void ProductsToDG(System.Windows.Controls.DataGrid DG)
        {
            DG.Items.Clear();
            foreach (Product p in ProductsList)
            {
                DG.Items.Add(p);
            }
        }

        public static void ProductsToCB(System.Windows.Controls.ComboBox CB)
        {
            CB.Items.Clear();
            foreach (Product p in ProductsList)
            {
                CB.Items.Add(p.Name);
            }
        }

        public static int NameToId(string _name)
        {
            Product tmp = ProductsList.Find(x => x.Name == _name);
            return tmp.Id;
        }

        public static void CreateProduct(string _name, int _count, int _units)
        {
            try
            {
                string query = $"INSERT INTO `hotel`.`stok` (`product_name`, `product_count`, `unit_id`) VALUES ('{_name}', '{_count}', '{_units}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateProduct(int _id,string _name, int _count, int _units)
        {
            try
            {
                string query = $"UPDATE `hotel`.`stok` SET `product_name` = '{_name}', `product_count` = '{_count}', `unit_id` = '{_units}' WHERE (`id_prod` = '{_id}');";
                Database.Insert(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteProduct(int _id)
        {
            try
            {                
                string query = $"DELETE FROM `hotel`.`stok` WHERE(`id_prod` = '{_id}');";
                Database.Delete(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
