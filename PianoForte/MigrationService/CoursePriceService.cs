using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.MigrationService
{
    class CoursePriceService
    {
        public static void create()
        {
            List<Course> courseList = CourseManager.findAllCourse();

            if (courseList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\course-price.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                CoursePriceService.createHeader(excelWorkSheet);
                CoursePriceService.createContent(excelWorkSheet, courseList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "MS03_SubjectId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "SC06_BranchId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS03_Tuition");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS03_Price");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Course> courseList)
        {
            for (int i = 0; i < courseList.Count; i++)
            {
                Course course = courseList[i];

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.Id.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.Price.ToString());
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
            }
        }
    }
}
