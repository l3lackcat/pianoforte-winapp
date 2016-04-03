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
    class SaleService
    {
        public static void create()
        {
            List<Payment> paymentList = PaymentManager.findAllPayment();

            if (paymentList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\sale.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                SaleService.createHeader(excelWorkSheet);
                SaleService.createContent(excelWorkSheet, paymentList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "SA01_SaleId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS11_StudentId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "SA01_SaleNo");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "SA01_SaleDate");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "SA01_CreateUser");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "SA01_ReceiptNo");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "SA01_ReceiveUser");
            HelperService.addCellHeader(excelWorkSheet, "H1", "H1", 15, "SA01_ReceiveDate");
            HelperService.addCellHeader(excelWorkSheet, "I1", "I1", 15, "SA01_Status");
            HelperService.addCellHeader(excelWorkSheet, "J1", "J1", 15, "SA01_CancelDate");

            HelperService.addCellHeader(excelWorkSheet, "K1", "K1", 15, "SA01_SaleId");
            HelperService.addCellHeader(excelWorkSheet, "L1", "L1", 15, "SA03_Seq");
            HelperService.addCellHeader(excelWorkSheet, "M1", "M1", 15, "SA03_PaymentType");
            HelperService.addCellHeader(excelWorkSheet, "N1", "N1", 15, "SA03_RefID");
            HelperService.addCellHeader(excelWorkSheet, "O1", "O1", 15, "SA03_Amount");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Payment> paymentList)
        {
            for (int i = 0; i < paymentList.Count; i++)
            {
                Payment payment = paymentList[i];
                User receiver = UserManager.findUser(payment.ReceiverId);
                int paymentId = payment.Id;
                int saleId = i + 1;
                int rowIndex = i + 2;

                HelperService.addCellData(excelWorkSheet, "A" + rowIndex.ToString(), "A" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, saleId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + rowIndex.ToString(), "B" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.StudentId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + rowIndex.ToString(), "C" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.Id.ToString());
                HelperService.addCellData(excelWorkSheet, "D" + rowIndex.ToString(), "D" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.PaymentDate.ToShortDateString());
                HelperService.addCellData(excelWorkSheet, "E" + rowIndex.ToString(), "E" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, receiver.Username);
                HelperService.addCellData(excelWorkSheet, "F" + rowIndex.ToString(), "F" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.Id.ToString());
                HelperService.addCellData(excelWorkSheet, "G" + rowIndex.ToString(), "G" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, receiver.Username);
                HelperService.addCellData(excelWorkSheet, "H" + rowIndex.ToString(), "H" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.PaymentDate.ToShortDateString());
                HelperService.addCellData(excelWorkSheet, "I" + rowIndex.ToString(), "I" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.Status);
                HelperService.addCellData(excelWorkSheet, "J" + rowIndex.ToString(), "J" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, "");

                string paymentType = "M";

                if (payment.CreditCardNumber == "")
                {
                    paymentType = "C";
                }

                HelperService.addCellData(excelWorkSheet, "K" + rowIndex.ToString(), "K" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, saleId.ToString());
                HelperService.addCellData(excelWorkSheet, "L" + rowIndex.ToString(), "L" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, saleId.ToString());
                HelperService.addCellData(excelWorkSheet, "M" + rowIndex.ToString(), "M" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentType);
                HelperService.addCellData(excelWorkSheet, "N" + rowIndex.ToString(), "N" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.CreditCardNumber);
                HelperService.addCellData(excelWorkSheet, "O" + rowIndex.ToString(), "O" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, payment.TotalPrice.ToString());
            }
        }
    }
}
