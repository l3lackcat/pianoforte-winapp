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
    class TeacherService
    {
        public static void create()
        {
            List<Teacher> teacherList = TeacherManager.findAllTeacher();

            if (teacherList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\teacher.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                TeacherService.createHeader(excelWorkSheet);
                TeacherService.createContent(excelWorkSheet, teacherList);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "old_teacherId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS04_TeacherId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "SC06_BranchId");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS04_TeacherCode");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "MS04_NickName");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "MS04_FirstName");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "MS04_LastName");
            HelperService.addCellHeader(excelWorkSheet, "H1", "H1", 15, "MS04_Email");
            HelperService.addCellHeader(excelWorkSheet, "I1", "I1", 15, "MS04_Tel1");
            HelperService.addCellHeader(excelWorkSheet, "J1", "J1", 15, "MS04_Tel2");
            HelperService.addCellHeader(excelWorkSheet, "K1", "K1", 15, "MS04_Tel3");
            HelperService.addCellHeader(excelWorkSheet, "L1", "L1", 15, "MS04_Tel4");
            HelperService.addCellHeader(excelWorkSheet, "M1", "M1", 15, "MS04_AccCode");
            HelperService.addCellHeader(excelWorkSheet, "N1", "N1", 15, "MS04_Status");
            HelperService.addCellHeader(excelWorkSheet, "O1", "O1", 15, "MS04_IsSpecial");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Teacher> teacherList)
        {
            for (int i = 0; i < teacherList.Count; i++)
            {
                Teacher teacher = teacherList[i];
                int oldTeacherId = teacher.Id;
                int newTeacherId = i + 1;
                string isSpecial = "N";

                if (teacher.Settings == Teacher.TeacherSettings.TEACHES_45_MIN)
                {
                    isSpecial = "Y";
                }

                TeacherService.updateRelatedTable(oldTeacherId, newTeacherId);

                HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldTeacherId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newTeacherId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (i + 2).ToString(), "C" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "D" + (i + 2).ToString(), "D" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, oldTeacherId.ToString());
                HelperService.addCellData(excelWorkSheet, "E" + (i + 2).ToString(), "E" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Nickname);
                HelperService.addCellData(excelWorkSheet, "F" + (i + 2).ToString(), "F" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Firstname);
                HelperService.addCellData(excelWorkSheet, "G" + (i + 2).ToString(), "G" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Lastname);
                HelperService.addCellData(excelWorkSheet, "H" + (i + 2).ToString(), "H" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Email);
                HelperService.addCellData(excelWorkSheet, "I" + (i + 2).ToString(), "I" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Phone1);
                HelperService.addCellData(excelWorkSheet, "J" + (i + 2).ToString(), "J" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Phone2);
                HelperService.addCellData(excelWorkSheet, "K" + (i + 2).ToString(), "K" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Phone3);
                HelperService.addCellData(excelWorkSheet, "L" + (i + 2).ToString(), "L" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "M" + (i + 2).ToString(), "M" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                HelperService.addCellData(excelWorkSheet, "N" + (i + 2).ToString(), "N" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Status.ToString());
                HelperService.addCellData(excelWorkSheet, "O" + (i + 2).ToString(), "O" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, isSpecial);
            }
        }

        private static bool updateRelatedTable(int oldTeacherId, int newTeacherId)
        {
            bool returnFlag = false;
            List<string> sqlList = new List<string>();

            sqlList.Add("UPDATE teacher SET teacherId=" + newTeacherId + " WHERE teacherId=" + oldTeacherId);
            sqlList.Add("UPDATE classroom SET teacherId=" + newTeacherId + " WHERE teacherId=" + oldTeacherId);
            sqlList.Add("UPDATE classroom_detail SET teacherId=" + newTeacherId + " WHERE teacherId=" + oldTeacherId);
            sqlList.Add("UPDATE teaching_course SET teacherId=" + newTeacherId + " WHERE teacherId=" + oldTeacherId);

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
