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
                string query = "SELECT * FROM hotel.stok;";
                List<Dictionary<string, string>> UR = Database.Select(query, Product.ProductKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        Product ur = new Product(Int32.Parse(item["id"]),
                                                item["product_name"],
                                                Int32.Parse(item["product_count"]),
                                                Int32.Parse(item["unit_id"]));
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
    }
}
