using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

using PianoForte.Model;

using System.Data.OleDb;

namespace PianoForte.Manager
{
    public class SystemUpdateManager
    {
        private static bool executeUpdateCommand(string sql)
        {
            bool returnFlag = false;

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

            return returnFlag;
        }

        private static bool updateClassroomDetail()
        {
            string tableName = ClassroomDetail.tableName;
            string classroomDateColumn = ClassroomDetail.columnClassroomDate;
            string classroomTimeColumn = ClassroomDetail.columnClassroomTime;
            string classroomDurationColumn = ClassroomDetail.columnClassroomDuration;
            string currStatusColumn = ClassroomDetail.columnCurrentStatus;
            string prevStatusColumn = ClassroomDetail.columnPreviousStatus;

            string sql = "UPDATE " + tableName + " SET " + prevStatusColumn + " = " + currStatusColumn + ", " + currStatusColumn + " = 'CHECKED_IN' " +
                         "WHERE " + currStatusColumn + " = 'WAITING' " +
                         "AND (" + classroomDateColumn + " < CURDATE() " +
                         "OR (" + classroomDateColumn + " = CURDATE() " +
                         "AND ADDTIME(STR_TO_DATE(" + classroomTimeColumn + ", '%H:%i:%s'), " + classroomDurationColumn + " * 100) < CURTIME()))";

            return SystemUpdateManager.executeUpdateCommand(sql);
        }

        private static void updateStudent()
        {
            List<Student> tempStudentList = StudentManager.findAllStudent(Student.StudentStatus.ACTIVE);

            foreach (Student s in tempStudentList)
            {
                bool hasActiveClassroom = false;
                List<Enrollment> tempEnrollmentList = EnrollmentManager.findAllEnrollmentByStudentId(s.Id);

                foreach (Enrollment en in tempEnrollmentList)
                {
                    List<Classroom> tempClassroomList = ClassroomManager.findAllClassroom(en.Id);

                    foreach (Classroom c in tempClassroomList)
                    {
                        List<ClassroomDetail> tempClassroomDetailList = ClassroomDetailManager.findAllActiveClassroomDetailByClassroomId(c.Id);

                        if (tempClassroomDetailList.Count > 0)
                        {
                            hasActiveClassroom = true;
                            break;
                        }
                    }

                    if (hasActiveClassroom == true)
                    {
                        break;
                    }
                }

                if (hasActiveClassroom == false)
                {
                    s.Status = Student.StudentStatus.INACTIVE.ToString();
                    StudentManager.updateStudent(s);
                }
            }
        }

        public static void update()
        {
            SystemUpdateManager.updateClassroomDetail();
            SystemUpdateManager.updateStudent();
        }
    }
}
