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
    class ScheduleService
    {
        public static void create(List<Schedule> scheduleList)
        {
            if (scheduleList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\schedule.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                ScheduleService.createHeader(excelWorkSheet);
                ScheduleService.createContent(excelWorkSheet, scheduleList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "SA04_RunId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "SA01_SaleId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "SA02_Seq");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS04_TeacherId");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "SA04_Type");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "SA04_StartTime");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "SA04_EndTime");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Schedule> scheduleList)
        {
            int SA04_RunId = 0;
            for (int i = 0; i < scheduleList.Count; i++)
            {
                Schedule schedule = scheduleList[i];

                for (int j = 0; j < schedule.NumberOfRepeat; j++)
                {
                    SA04_RunId++;

                    int rowIndex = SA04_RunId + 1;

                    HelperService.addCellData(excelWorkSheet, "A" + rowIndex.ToString(), "A" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, SA04_RunId.ToString());
                    HelperService.addCellData(excelWorkSheet, "B" + rowIndex.ToString(), "B" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, schedule.SaleId.ToString());
                    HelperService.addCellData(excelWorkSheet, "C" + rowIndex.ToString(), "C" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, schedule.SaleDetailId.ToString());
                    HelperService.addCellData(excelWorkSheet, "D" + rowIndex.ToString(), "D" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, schedule.TeacherId.ToString());
                    HelperService.addCellData(excelWorkSheet, "E" + rowIndex.ToString(), "E" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, "N");
                    HelperService.addCellData(excelWorkSheet, "F" + rowIndex.ToString(), "F" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, schedule.StartTime.AddDays(j * 7).ToString("dd-MMM-yyyy HH':'mm':'ss"));
                    HelperService.addCellData(excelWorkSheet, "G" + rowIndex.ToString(), "G" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, schedule.EndTime.AddDays(j * 7).ToString("dd-MMM-yyyy HH':'mm':'ss"));
                }
            }
        }
    }
}
