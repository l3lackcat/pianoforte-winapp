using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.MigrationService
{
    class ProductPriceService
    {
        public static void create()
        {
            List<Book> bookList = BookManager.findAllBook();
            List<Cd> cdList = CdManager.findAllCd();

            if ((bookList.Count > 0) || (cdList.Count > 0))
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\product-price.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                ProductPriceService.createHeader(excelWorkSheet);
                ProductPriceService.createContent(excelWorkSheet, bookList, cdList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "MS07_ItemId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "SC06_BranchId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS10_SaleTotal");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Book> bookList, List<Cd> cdList)
        {
            int i = 0;

            for (int j = 0; j < bookList.Count; j++)
            {
                Book book = bookList[j];
                int newBookId = i + 1;

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newBookId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, book.Price.ToString());

                i++;
            }

            for (int j = 0; j < cdList.Count; j++)
            {
                Cd cd = cdList[j];
                int newCdId = i + 1;

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newCdId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, cd.Price.ToString());

                i++;
            }
        }
    }
}
