using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PianoForte.Model;
using PianoForte.Manager;

namespace PianoForte.View
{
    public partial class ClassroomDateViewer : Form
    {
        private const int DATE_RANGE_OFFSET_L = 0;
        private const int DATE_RANGE_OFFSET_R = 28;

        private Form caller;
        private Enrollment enrollment = null;
        private DateTime minClassroomDate = DateTime.Today;
        private DateTime maxClassroomDate = DateTime.Today;

        public ClassroomDateViewer()
        {
            InitializeComponent();
        }

        public Enrollment showFormDialog(Form caller, Enrollment enrollment)
        {
            this.caller = caller;
            this.enrollment = enrollment;

            //this.initialize();
            this.ShowDialog();

            return this.enrollment;
        }

        private void initialize()
        {
            if (this.enrollment != null)
            {
                this.initMinClassroomDate();
                this.initMaxClassroomDate();

                this.TextBox_CourseName.Text = this.enrollment.Course.Name;
                this.TextBox_CourseLevel.Text = this.enrollment.Course.Level;

                int classroomListCount = this.enrollment.ClassroomList.Count();

                for (int i = 0; i < classroomListCount; i++)
                {
                    Classroom classroom = this.enrollment.ClassroomList[i];
                    Teacher teacher = TeacherManager.findTeacher(classroom.TeacherId);
                    List<ClassroomDetail> classroomDetailList = this.enrollment.ClassroomIdClassroomDetailListDictionary[classroom.Id];

                    if (i == 0)
                    {
                        this.TextBox_DayOfWeek1.Text = classroom.ClassDayOfWeek;
                        this.TextBox_ClassroomTime1.Text = classroom.ClassTime;
                        this.TextBox_TeacherName1.Text = teacher.Firstname + " (" + teacher.Nickname + ")";
                    }
                    else
                    {
                        this.TextBox_DayOfWeek2.Text = classroom.ClassDayOfWeek;
                        this.TextBox_ClassroomTime2.Text = classroom.ClassTime;
                        this.TextBox_TeacherName2.Text = teacher.Firstname + " (" + teacher.Nickname + ")";
                    }



                    for (int j = classroomDetailList.Count() - 1; j >= 0; j--)
                    {
                        ClassroomDetail classroomDetail = classroomDetailList[j];
                        int index = this.getDatePickerNumber(classroomListCount, i, j);

                        switch (index)
                        {
                            case 1:
                                this.DateTimePicker_ClassroomDate01.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate01.Show();
                                break;
                            case 2:
                                this.DateTimePicker_ClassroomDate02.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate02.Show();
                                break;
                            case 3:
                                this.DateTimePicker_ClassroomDate03.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate03.Show();
                                break;
                            case 4:
                                this.DateTimePicker_ClassroomDate04.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate04.Show();
                                break;
                            case 5:
                                this.DateTimePicker_ClassroomDate05.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate05.Show();
                                break;
                            case 6:
                                this.DateTimePicker_ClassroomDate06.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate06.Show();
                                break;
                            case 7:
                                this.DateTimePicker_ClassroomDate07.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate07.Show();
                                break;
                            case 8:
                                this.DateTimePicker_ClassroomDate08.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate08.Show();
                                break;
                            case 9:
                                this.DateTimePicker_ClassroomDate09.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate09.Show();
                                break;
                            case 10:
                                this.DateTimePicker_ClassroomDate10.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate10.Show();
                                break;
                            case 11:
                                this.DateTimePicker_ClassroomDate11.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate11.Show();
                                break;
                            case 12:
                                this.DateTimePicker_ClassroomDate12.Value = classroomDetail.ClassroomDate;
                                this.DateTimePicker_ClassroomDate12.Show();
                                break;
                        }
                    }
                }
            }

            this.updateDateRangeForDateTimePicker_ClassroomDate01();
            this.updateDateRangeForDateTimePicker_ClassroomDate02();
            this.updateDateRangeForDateTimePicker_ClassroomDate03();
            this.updateDateRangeForDateTimePicker_ClassroomDate04();
            this.updateDateRangeForDateTimePicker_ClassroomDate05();
            this.updateDateRangeForDateTimePicker_ClassroomDate06();
            this.updateDateRangeForDateTimePicker_ClassroomDate07();
            this.updateDateRangeForDateTimePicker_ClassroomDate08();
            this.updateDateRangeForDateTimePicker_ClassroomDate09();
            this.updateDateRangeForDateTimePicker_ClassroomDate10();
            this.updateDateRangeForDateTimePicker_ClassroomDate11();
            this.updateDateRangeForDateTimePicker_ClassroomDate12();

            this.updateTextBox_StartDate(this.minClassroomDate);
            this.updateTextBox_NextPayment(this.maxClassroomDate);
        }

        private void initMinClassroomDate()
        {
            int classroomListCount = this.enrollment.ClassroomList.Count();

            for (int i = 0; i < classroomListCount; i++)
            {
                Classroom classroom = this.enrollment.ClassroomList[i];
                DateTime minDate = ClassroomDetailManager.getMinClassroomDate(this.enrollment.ClassroomIdClassroomDetailListDictionary[classroom.Id]);

                if (i == 0)
                {
                    this.minClassroomDate = minDate;
                }
                else
                {
                    if (this.minClassroomDate > minDate)
                    {
                        this.minClassroomDate = minDate;
                    }
                }
            }
        }

        private void initMaxClassroomDate()
        {
            int classroomListCount = this.enrollment.ClassroomList.Count();

            for (int i = 0; i < classroomListCount; i++)
            {
                Classroom classroom = this.enrollment.ClassroomList[i];
                DateTime maxDate = ClassroomDetailManager.getMaxClassroomDate(this.enrollment.ClassroomIdClassroomDetailListDictionary[classroom.Id]);

                if (i == 0)
                {
                    this.maxClassroomDate = maxDate;
                }
                else
                {
                    if (this.maxClassroomDate < maxDate)
                    {
                        this.maxClassroomDate = maxDate;
                    }
                }
            }
        }

        private int getClassroomIndex(int numberOfClassroom, int datePickerNumber)
        {
            int classroomIndex = -1;

            if (numberOfClassroom == 1)
            {
                classroomIndex = 0;
            }
            else if (numberOfClassroom == 2)
            {
                if ((datePickerNumber % 2) != 0)
                {
                    classroomIndex = 0;
                }
                else
                {
                    classroomIndex = 1;
                }
            }

            return classroomIndex;
        }

        private int getDatePickerNumber(int numberOfClassroom, int classroomIndex, int classroomDetailIndex)
        {
            int datePickerNumber = (classroomDetailIndex * numberOfClassroom) + classroomIndex + 1;

            return datePickerNumber;
        }

        private int getClassroomDetailIndex(int numberOfClassroom, int classroomIndex, int datePickerIndex)
        {
            int classroomDetailIndex = (datePickerIndex - 1 - classroomIndex) / numberOfClassroom;

            return classroomDetailIndex;
        }

        private void updateClassroomDetailDate(int datePickerNumber, DateTime date)
        {
            int classroomListCount = this.enrollment.ClassroomList.Count;
            int classroomIndex = this.getClassroomIndex(classroomListCount, datePickerNumber);
            Classroom classroom = this.enrollment.ClassroomList[classroomIndex];

            if (classroom != null)
            {
                int classroomDetailIndex = this.getClassroomDetailIndex(classroomListCount, classroomIndex, datePickerNumber);
                this.enrollment.ClassroomIdClassroomDetailListDictionary[classroom.Id][classroomDetailIndex].ClassroomDate = date;

                if (classroomDetailIndex == 0)
                {
                    this.enrollment.ClassroomList[classroomIndex].ClassDayOfWeek = ConvertManager.toShortDayOfWeek_EN(date.DayOfWeek.ToString()); ;
                    this.enrollment.ClassroomList[classroomIndex].StartDate = date;
                }
            }
        }

        private void updateTextBox_StartDate(DateTime date)
        {
            this.TextBox_StartDate.Text = date.ToShortDateString();
        }

        private void updateTextBox_NextPayment(DateTime date)
        {
            this.TextBox_NextPayment.Text = date.ToShortDateString();
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate01()
        {
            if (this.DateTimePicker_ClassroomDate01.Visible == true)
            {
                this.DateTimePicker_ClassroomDate01.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate01.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate02()
        {
            if (this.DateTimePicker_ClassroomDate02.Visible == true)
            {
                this.DateTimePicker_ClassroomDate02.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate02.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate03()
        {
            if (this.DateTimePicker_ClassroomDate03.Visible == true)
            {
                this.DateTimePicker_ClassroomDate03.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate03.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate04()
        {
            if (this.DateTimePicker_ClassroomDate04.Visible == true)
            {
                this.DateTimePicker_ClassroomDate04.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate04.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate05()
        {
            if (this.DateTimePicker_ClassroomDate05.Visible == true)
            {
                this.DateTimePicker_ClassroomDate05.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate05.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate06()
        {
            if (this.DateTimePicker_ClassroomDate06.Visible == true)
            {
                this.DateTimePicker_ClassroomDate06.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate06.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate07()
        {
            if (this.DateTimePicker_ClassroomDate07.Visible == true)
            {
                this.DateTimePicker_ClassroomDate07.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate07.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate08()
        {
            if (this.DateTimePicker_ClassroomDate08.Visible == true)
            {
                this.DateTimePicker_ClassroomDate08.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate08.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate09()
        {
            if (this.DateTimePicker_ClassroomDate09.Visible == true)
            {
                this.DateTimePicker_ClassroomDate09.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate09.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate10()
        {
            if (this.DateTimePicker_ClassroomDate10.Visible == true)
            {
                this.DateTimePicker_ClassroomDate10.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate10.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate11()
        {
            if (this.DateTimePicker_ClassroomDate11.Visible == true)
            {
                this.DateTimePicker_ClassroomDate11.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate11.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void updateDateRangeForDateTimePicker_ClassroomDate12()
        {
            if (this.DateTimePicker_ClassroomDate12.Visible == true)
            {
                this.DateTimePicker_ClassroomDate12.MinDate = this.minClassroomDate.AddDays(DATE_RANGE_OFFSET_L);
                this.DateTimePicker_ClassroomDate12.MaxDate = this.maxClassroomDate.AddDays(DATE_RANGE_OFFSET_R);
            }
        }

        private void ClassroomDateViewer_Load(object sender, EventArgs e)
        {
            this.initialize();
        }

        private void DateTimePicker_ClassroomDate01_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate01.Value;

            this.updateClassroomDetailDate(1, newDate);
            this.updateTextBox_StartDate(newDate);

            if (this.DateTimePicker_ClassroomDate02.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate02_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate02.Value;

            this.updateClassroomDetailDate(2, newDate);

            if (this.DateTimePicker_ClassroomDate03.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate03_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate03.Value;

            this.updateClassroomDetailDate(3, newDate);

            if (this.DateTimePicker_ClassroomDate04.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate04_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate04.Value;

            this.updateClassroomDetailDate(4, newDate);

            if (this.DateTimePicker_ClassroomDate05.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate05_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate05.Value;

            this.updateClassroomDetailDate(5, newDate);

            if (this.DateTimePicker_ClassroomDate06.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate06_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate06.Value;

            this.updateClassroomDetailDate(6, newDate);

            if (this.DateTimePicker_ClassroomDate07.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate07_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate07.Value;

            this.updateClassroomDetailDate(7, newDate);

            if (this.DateTimePicker_ClassroomDate08.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate08_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate08.Value;

            this.updateClassroomDetailDate(8, newDate);

            if (this.DateTimePicker_ClassroomDate09.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate09_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate09.Value;

            this.updateClassroomDetailDate(9, newDate);

            if (this.DateTimePicker_ClassroomDate10.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate10_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate10.Value;

            this.updateClassroomDetailDate(10, newDate);

            if (this.DateTimePicker_ClassroomDate11.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate11_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate11.Value;

            this.updateClassroomDetailDate(11, newDate);

            if (this.DateTimePicker_ClassroomDate12.Visible == false)
            {
                this.updateTextBox_NextPayment(newDate);
            }
        }

        private void DateTimePicker_ClassroomDate12_ValueChanged(object sender, EventArgs e)
        {
            DateTime newDate = this.DateTimePicker_ClassroomDate12.Value;

            this.updateClassroomDetailDate(12, newDate);
            this.updateTextBox_NextPayment(newDate);
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            this.enrollment = null;
            this.Close();
        }

        private void Button_Finish_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
