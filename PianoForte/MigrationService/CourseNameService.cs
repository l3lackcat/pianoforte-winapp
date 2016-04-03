using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.MigrationService
{
    class CourseNameService
    {
        public static Dictionary<string, int> create()
        {
            Dictionary<string, int> courseNameAndIdDictionary = createCourseNameAndIdDictionary();

            if (courseNameAndIdDictionary.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\course-name.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                CourseNameService.createHeader(excelWorkSheet);
                CourseNameService.createContent(excelWorkSheet, courseNameAndIdDictionary);

                excelWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);
                excelWorkBook.Close(true, missingValue, missingValue);
                excelApplication.Quit();

                HelperService.releaseObject(excelWorkSheet);
                HelperService.releaseObject(excelWorkBook);
                HelperService.releaseObject(excelApplication);
            }

            return courseNameAndIdDictionary;
        }

        private static void createHeader(Excel.Worksheet excelWorkSheet)
        {
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "MS02_ClassId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "MS01_CourseId");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS02_ClassName");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS02_Status");
        }

        private static void createContent(Excel.Worksheet excelWorkSheet, Dictionary<string, int> courseNameAndIdDictionary)
        {
            int index = 0;

            foreach (KeyValuePair<string, int> item in courseNameAndIdDictionary)
            {
                int newCourseId = item.Value;
                int courseCategoryId = 0;
                string courseName = item.Key;
                string status = Course.CourseStatus.INACTIVE.ToString();
                List<Course> courseList = CourseManager.findAllCourseByCourseName(courseName);

                for (int j = 0; j < courseList.Count; j++)
                {
                    Course course = courseList[j];

                    courseCategoryId = course.CourseCategoryId;

                    if (courseList[j].Status == Course.CourseStatus.ACTIVE.ToString())
                    {
                        status = Course.CourseStatus.ACTIVE.ToString();
                        break;
                    }
                }

                HelperService.addCellData(excelWorkSheet, "A" + (index + 2).ToString(), "A" + (index + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, newCourseId.ToString());
                HelperService.addCellData(excelWorkSheet, "B" + (index + 2).ToString(), "B" + (index + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseCategoryId.ToString());
                HelperService.addCellData(excelWorkSheet, "C" + (index + 2).ToString(), "C" + (index + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, courseName);
                HelperService.addCellData(excelWorkSheet, "D" + (index + 2).ToString(), "D" + (index + 2).ToString(), Excel.XlHAlign.xlHAlignLeft, status);

                index++;
            }
        }

        private static Dictionary<string, int> createCourseNameAndIdDictionary()
        {
            Dictionary<string, int> courseNameAndIdDictionary = new Dictionary<string, int>();
            List<string> courseNameList = CourseManager.findAllCourseName();

            for (int i = 0; i < courseNameList.Count; i++)
            {
                courseNameAndIdDictionary.Add(courseNameList[i], i + 1);
            }

            return courseNameAndIdDictionary;
        }
    }
}
