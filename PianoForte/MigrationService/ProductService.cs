using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.MigrationService
{
    class ProductService
    {
        public static void create()
        {
            List<Book> bookList = BookManager.findAllBook();
            List<Cd> cdList = CdManager.findAllCd();

            if ((bookList.Count > 0) || (cdList.Count > 0))
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\product.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                ProductService.createHeader(excelWorkSheet);
                ProductService.createContent(excelWorkSheet, bookList, cdList);

                excelWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);
                excelWorkBook.Close(true, missingValue, missingValue);
                excelApplication.Quit();

                HelperService.releaseObject(excelWorkSheet);
                HelperService.releaseObject(excelWorkBook);
                HelperService.releaseObject(excelApplication);
            }
        }

        private static void createHeader(Excel.Worksheet excelWorkSheet)
        {
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "old_productId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS07_ItemId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS06_ItemTypeId");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS07_ItemCode");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "MS07_ItemName");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "MS07_Barcode");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "MS07_Status");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Book> bookList, List<Cd> cdList)
        {
            int i = 0;

            for (int j = 0; j < bookList.Count; j++)
            {
                Book book = bookList[j];
                int oldBookId = book.Id;
                int newBookId = i + 1;

                ProductService.updateRelatedTable(oldBookId, newBookId);

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldBookId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newBookId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, Product.ProductType.BOOK.ToString());
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldBookId.ToString());
                HelperService.addCellData(excelWorkSheet, "E" + (i + 2).ToString(), "E" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, book.Name);
                HelperService.addCellData(excelWorkSheet, "F" + (i + 2).ToString(), "F" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "'" + book.Barcode);
                HelperService.addCellData(excelWorkSheet, "G" + (i + 2).ToString(), "G" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, book.Status.ToString());

                i++;
            }

            for (int j = 0; j < cdList.Count; j++)
            {
                Cd cd = cdList[j];
                int oldCdId = cd.Id;
                int newCdId = i + 1;

                ProductService.updateRelatedTable(oldCdId, newCdId);

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldCdId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newCdId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, Product.ProductType.CD.ToString());
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldCdId.ToString());
                HelperService.addCellData(excelWorkSheet, "E" + (i + 2).ToString(), "E" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, cd.Name);
                HelperService.addCellData(excelWorkSheet, "F" + (i + 2).ToString(), "F" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, cd.Barcode);
                HelperService.addCellData(excelWorkSheet, "G" + (i + 2).ToString(), "G" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, cd.Status.ToString());

                i++;
            }
        }

        private static bool updateRelatedTable(int oldProductId, int newProductId)
        {
            bool returnFlag = false;
            List<string> sqlList = new List<string>();

            sqlList.Add("UPDATE book SET bookId=" + newProductId + " WHERE bookId=" + oldProductId);
            sqlList.Add("UPDATE cd SET cdId=" + newProductId + " WHERE cdId=" + oldProductId);
            sqlList.Add("UPDATE payment_detail SET productId=" + newProductId + " WHERE productId=" + oldProductId);

            for (int i = 0; i < sqlList.Count; i++)
            {
                string sql = sqlList[i];
                MySqlConnection conn = null;

                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(sql, conn);

                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow != -1)
                        {
                            returnFlag = true;
                        }
                    }
                }
                catch (System.InvalidOperationException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (MySqlException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (System.SystemException e)
                {
                    LogManager.writeLog(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return returnFlag;
        }
    }
}
