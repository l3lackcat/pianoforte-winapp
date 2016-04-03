using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.MigrationService
{
    class TeachingCourseService
    {
        public static void create(Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary)
        {
            List<Teacher> teacherList = TeacherManager.findAllTeacher();

            if (teacherList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\teaching-course.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                TeachingCourseService.createHeader(excelWorkSheet);
                TeachingCourseService.createContent(excelWorkSheet, teacherList, courseLevelIdAndcourseNameIdDictionary);

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
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "MS04_TeacherId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS02_ClassId");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, List<Teacher> teacherList, Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary)
        {
            for (int i = 0; i < teacherList.Count; i++)
            {
                Teacher teacher = teacherList[i];

                if (teacher.Status == Teacher.TeacherStatus.ACTIVE.ToString())
                {
                    List<int> courseNameIdList = getCourseNameIdList(teacher.Id, courseLevelIdAndcourseNameIdDictionary);

                    for (int j = 0; j < courseNameIdList.Count; j++)
                    {
                        HelperService.addCellData(excelWorkSheet, "A" + (i + 2).ToString(), "A" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, teacher.Id.ToString());
                        HelperService.addCellData(excelWorkSheet, "B" + (i + 2).ToString(), "B" + (i + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseNameIdList[j].ToString());
                    }
                }
            }
        }

        private static List<int> getCourseNameIdList(int teacherId, Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary)
        {
            List<int> courseNameIdList = new List<int>();
            List<int> courseLevelIdList = TeachingCourseManager.getCourseIdByTeacherId(teacherId);

            for (int i = 0; i < courseLevelIdList.Count; i++)
            {
                int courseLevelId = courseLevelIdList[i];
                int courseNameId = courseLevelIdAndcourseNameIdDictionary[courseLevelId];

                if (courseNameIdList.Contains(courseNameId) == false)
                {
                    courseNameIdList.Add(courseNameId);
                }
            }

            return courseNameIdList;
        }
    }
}
