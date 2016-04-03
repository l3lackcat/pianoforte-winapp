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
    class CourseLevelService
    {
        public static Dictionary<int, int> create(Dictionary<string, int> courseNameAndIdDictionary)
        {
            Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary = new Dictionary<int, int>();
            List<Course> courseList = CourseManager.findAllCourse();

            if (courseList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\course-level.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                CourseLevelService.createHeader(excelWorkSheet);
                courseLevelIdAndcourseNameIdDictionary = CourseLevelService.createContent(excelWorkSheet, courseList, courseNameAndIdDictionary);

                excelWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);
                excelWorkBook.Close(true, missingValue, missingValue);
                excelApplication.Quit();

                HelperService.releaseObject(excelWorkSheet);
                HelperService.releaseObject(excelWorkBook);
                HelperService.releaseObject(excelApplication);
            }

            return courseLevelIdAndcourseNameIdDictionary;
        }

        private static void createHeader(Excel.Worksheet excelWorkSheet)
        {
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "old_courseId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS03_SubjectId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS02_ClassId");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS03_SubjectName");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "MS03_SubjectCode");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "MS03_Level");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "MS03_Qty");
            HelperService.addCellHeader(excelWorkSheet, "H1", "H1", 15, "MS03_Period");
            HelperService.addCellHeader(excelWorkSheet, "I1", "I1", 15, "MS03_SubjectType");
            HelperService.addCellHeader(excelWorkSheet, "J1", "J1", 15, "MS03_Status");
        }

        private static Dictionary<int, int> createContent(Excel.Worksheet excelWorkSheet, List<Course> courseList, Dictionary<string, int> courseNameAndIdDictionary)
        {
            Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary = new Dictionary<int, int>();

            for (int i = 0; i < courseList.Count; i++)
            {
                Course course = courseList[i];
                int oldCourseId = course.Id;
                int newCourseId = i + 1;
                int courseNameId = courseNameAndIdDictionary[course.Name];
                string subjectType = "S";

                if (course.StudentPerClassroom > 1)
                {
                    subjectType = "G";
                }

                courseLevelIdAndcourseNameIdDictionary.Add(newCourseId, courseNameId);
                CourseLevelService.updateRelatedTable(oldCourseId, newCourseId);

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldCourseId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newCourseId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseNameId.ToString());
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.Level);
                HelperService.addCellData(excelWorkSheet, "E" + (i + 2).ToString(), "E" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldCourseId.ToString());
                HelperService.addCellData(excelWorkSheet, "F" + (i + 2).ToString(), "F" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "G" + (i + 2).ToString(), "G" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.NumberOfClassroom.ToString());
                HelperService.addCellData(excelWorkSheet, "H" + (i + 2).ToString(), "H" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.ClassroomDuration.ToString());
                HelperService.addCellData(excelWorkSheet, "I" + (i + 2).ToString(), "I" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, subjectType);
                HelperService.addCellData(excelWorkSheet, "J" + (i + 2).ToString(), "J" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, course.Status.ToString());
            }

            return courseLevelIdAndcourseNameIdDictionary;
        }

        private static bool updateRelatedTable(int oldCourseId, int newCourseId)
        {
            bool returnFlag = false;
            List<string> sqlList = new List<string>();

            sqlList.Add("UPDATE course SET courseId=" + newCourseId + " WHERE courseId=" + oldCourseId);
            sqlList.Add("UPDATE enrollment SET courseId=" + newCourseId + " WHERE courseId=" + oldCourseId);
            sqlList.Add("UPDATE payment_detail SET productId=" + newCourseId + " WHERE productId=" + oldCourseId);
            sqlList.Add("UPDATE teaching_course SET courseId=" + newCourseId + " WHERE courseId=" + oldCourseId);

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
