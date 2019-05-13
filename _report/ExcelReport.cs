using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;

namespace hotelAdm
{
    class ExcelReport
    {
        private static string[] ReportKeys = new string[] { "id", "guest_name", "passport_num", "price", "start_day", "count_of_days", "date" };
        public static void Create()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    throw new Exception("Excel не установлен!");
                }
                Workbook xlWorkBook = xlApp.Workbooks.Add();
                Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                ///
                xlWorkSheet.Columns[1].ColumnWidth = 5;
                xlWorkSheet.Columns[2].ColumnWidth = 40;
                xlWorkSheet.Columns[3].ColumnWidth = 20;
                xlWorkSheet.Columns[4].ColumnWidth = 15;
                xlWorkSheet.Columns[5].ColumnWidth = 20;
                xlWorkSheet.Columns[6].ColumnWidth = 20;
                xlWorkSheet.Columns[7].ColumnWidth = 20;

                xlWorkSheet.Cells[1, 1].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, 7]].Merge();

                xlWorkSheet.Cells[1, 1] = "Отчет по заказам за последний месяц";
                xlWorkSheet.Cells[2, 1] = "№";
                xlWorkSheet.Cells[2, 2] = "ФИО гостя";
                xlWorkSheet.Cells[2, 3] = "№ пасспорта гостя";
                xlWorkSheet.Cells[2, 4] = "Цена";
                xlWorkSheet.Cells[2, 5] = "День начала";
                xlWorkSheet.Cells[2, 6] = "Количество дней";
                xlWorkSheet.Cells[2, 7] = "Дата регистрации";

                int Row = 3;
                double sum = 0;

                string query = "call hotel.GetReport();";
                List<Dictionary<string, string>> UR = Database.Select(query, ReportKeys);
                if (UR.Count != 0)
                {
                    foreach (var item in UR)
                    {
                        xlWorkSheet.Cells[Row, 1] = item["id"];
                        xlWorkSheet.Cells[Row, 2] = item["guest_name"];
                        xlWorkSheet.Cells[Row, 3] = item["passport_num"];
                        xlWorkSheet.Cells[Row, 4] = item["price"];
                        xlWorkSheet.Cells[Row, 5] = item["start_day"];
                        xlWorkSheet.Cells[Row, 6] = item["count_of_days"];
                        xlWorkSheet.Cells[Row, 7] = item["date"];
                        sum += Convert.ToDouble(item["price"]);

                        Row++;
                    }
                }
                
                xlWorkSheet.Range[xlWorkSheet.Cells[Row+1, 1], xlWorkSheet.Cells[Row+1, 3]].Merge();
                xlWorkSheet.Cells[Row+1, 1] = "Сумма заказов:";
                xlWorkSheet.Cells[Row+1, 4] = sum.ToString();
                ///
                Range cell1 = xlWorkSheet.Cells[1, 1];
                Range cell2 = xlWorkSheet.Cells[Row-1, 7];
                Range range = xlWorkSheet.get_Range(cell1, cell2);
                range.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
                ///
                string day = DateTime.Today.ToString("dd-MM-yyyy");
                string time = DateTime.Now.ToString("HH-mm-ss");
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                xlWorkBook.SaveAs($@"{dir}Отчет({day} {time}).xlsx");
                xlWorkBook.Close();
                xlApp = null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
