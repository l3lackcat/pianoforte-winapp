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
    class StudentService
    {
        public static void create()
        {
            List<Student> studentList = StudentManager.findAllStudent();

            if (studentList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\student.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                StudentService.createHeader(excelWorkSheet);
                StudentService.createContent(excelWorkSheet, studentList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "MS11_StudentId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS11_StudentCode");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS11_FirstName");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS11_LastName");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "MS11_NickName");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "MS11_Tel1");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "MS11_Tel2");
            HelperService.addCellHeader(excelWorkSheet, "H1", "H1", 15, "MS11_Tel3");
            HelperService.addCellHeader(excelWorkSheet, "I1", "I1", 15, "MS11_BirthDate");
            HelperService.addCellHeader(excelWorkSheet, "J1", "J1", 15, "MS11_Email");
            HelperService.addCellHeader(excelWorkSheet, "K1", "K1", 15, "MS11_Address1");
            HelperService.addCellHeader(excelWorkSheet, "L1", "L1", 15, "MS11_Address2");
            HelperService.addCellHeader(excelWorkSheet, "M1", "M1", 15, "MS11_Postcode");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Student> studentList)
        {
            for (int i = 0; i < studentList.Count; i++)
            {
                Student student = studentList[i];
                int studentId = student.Id;

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, studentId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, studentId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Firstname);
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Lastname);
                HelperService.addCellData(excelWorkSheet, "E" + (i + 2).ToString(), "E" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Nickname);
                HelperService.addCellData(excelWorkSheet, "F" + (i + 2).ToString(), "F" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Phone1);
                HelperService.addCellData(excelWorkSheet, "G" + (i + 2).ToString(), "G" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Phone2);
                HelperService.addCellData(excelWorkSheet, "H" + (i + 2).ToString(), "H" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Phone3);
                HelperService.addCellData(excelWorkSheet, "I" + (i + 2).ToString(), "I" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Birthday.ToShortDateString());
                HelperService.addCellData(excelWorkSheet, "J" + (i + 2).ToString(), "J" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Email);
                HelperService.addCellData(excelWorkSheet, "K" + (i + 2).ToString(), "K" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Address.Address1);
                HelperService.addCellData(excelWorkSheet, "L" + (i + 2).ToString(), "L" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Address.Address2);
                HelperService.addCellData(excelWorkSheet, "M" + (i + 2).ToString(), "M" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, student.Address.PostCode.ToString());
            }
        }
    }
}
