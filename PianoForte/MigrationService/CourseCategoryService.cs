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
    class CourseCategoryService
    {
        public static void create()
        {
            List<CourseCategory> courseCategoryList = CourseCategoryManager.findAllCourseCategory();

            if (courseCategoryList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\course-category.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                CourseCategoryService.createHeader(excelWorkSheet);
                CourseCategoryService.createContent(excelWorkSheet, courseCategoryList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "old_CourseCategoryId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS01_CourseId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS01_CourseName");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS01_Status");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<CourseCategory> courseCategoryList)
        {
            for (int i = 0; i < courseCategoryList.Count; i++)
            {
                CourseCategory courseCategory = courseCategoryList[i];
                int oldCourseCategoryId = courseCategory.Id;
                int newCourseCategoryId = i + 1;

                CourseCategoryService.updateRelatedTable(oldCourseCategoryId, newCourseCategoryId);

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldCourseCategoryId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newCourseCategoryId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseCategory.Name);
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseCategory.Status.ToString());
            }
        }

        private static bool updateRelatedTable(int oldCourseCategoryId, int newCourseCategoryId)
        {
            bool returnFlag = false;
            List<string> sqlList = new List<string>();

            sqlList.Add("UPDATE course SET courseCategoryId=" + newCourseCategoryId + " WHERE courseCategoryId=" + oldCourseCategoryId);

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
