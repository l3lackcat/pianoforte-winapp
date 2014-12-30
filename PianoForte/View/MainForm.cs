﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PianoForte.Constant;
using PianoForte.Manager;
using PianoForte.Model;
using PianoForte.Dao;

namespace PianoForte.View
{
    public partial class MainForm : Form
    {
        //Logged User
        private User user;

        //Current Form
        public static Form currentForm;

        //Login Form
        private static LoginForm loginForm;

        //Form on top menu        
        public static PaymentForm paymentForm;
        public static StudentMainForm studentMainForm;
        public static TeacherMainForm teacherMainForm;
        public static CourseMainForm courseMainForm;
        public static BookMainForm bookMainForm;
        public static CdMainForm cdMainForm;
        public static PaymentMainForm paymentMainForm;
        public static DailyPaymentReportForm dailyPaymentReportForm;
        public static CheckInMainForm checkInMainForm;
        public static PaymentForm2 paymentForm2;

        public MainForm()
        {
            //LogManager.writeLog("Start MainForm.InitializeComponent()");
            InitializeComponent();
            //LogManager.writeLog("Finish MainForm.InitializeComponent()");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //LogManager.writeLog("Start MainForm.MainForm_Load()");
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(6, 6);

            //LogManager.writeLog("Start DatabaseManager.readDatabaseConfiguration()");
            DatabaseManager.readDatabaseConfiguration();
            //LogManager.writeLog("Finish DatabaseManager.readDatabaseConfiguration()");

            //LogManager.writeLog("Start DateTimeManager.initialTimeList()");
            DateTimeManager.initialTimeList();
            //LogManager.writeLog("Finish DateTimeManager.initialTimeList()");

            Constant.Constant.STARTUP_PATH = Application.StartupPath;

            currentForm = null;

            user = new User();

            //LogManager.writeLog("Start loading LoginForm");
            loginForm = new LoginForm();
            loginForm.load(this);
            this.initialForm(loginForm);
            //LogManager.writeLog("Finish loading LoginForm");

            //LogManager.writeLog("Start loading PaymentForm");
            //paymentForm = new PaymentForm();
            //paymentForm.load(this);
            //this.initialForm(paymentForm);
            //LogManager.writeLog("Finish loading PaymentForm");

            //LogManager.writeLog("Start loading StudentMainForm");
            studentMainForm = new StudentMainForm();
            studentMainForm.load(this);
            this.initialForm(studentMainForm);
            //LogManager.writeLog("Finish loading StudentMainForm");

            //LogManager.writeLog("Start loading TeacherMainForm");
            teacherMainForm = new TeacherMainForm();
            teacherMainForm.load(this);
            this.initialForm(teacherMainForm);
            //LogManager.writeLog("Finish loading TeacherMainForm");

            //LogManager.writeLog("Start loading CourseMainForm");
            courseMainForm = new CourseMainForm();
            courseMainForm.load(this);
            this.initialForm(courseMainForm);
            //LogManager.writeLog("Finish loading CourseMainForm");

            //LogManager.writeLog("Start loading BookMainForm");
            bookMainForm = new BookMainForm();
            bookMainForm.load(this);
            this.initialForm(bookMainForm);
            //LogManager.writeLog("Finish loading BookMainForm");

            //LogManager.writeLog("Start loading CdMainForm");
            cdMainForm = new CdMainForm();
            cdMainForm.load(this);
            this.initialForm(cdMainForm);
            //LogManager.writeLog("Finish loading CdMainForm");

            //LogManager.writeLog("Start loading PaymentMainForm");
            paymentMainForm = new PaymentMainForm();
            paymentMainForm.load(this);
            this.initialForm(paymentMainForm);
            //LogManager.writeLog("Finish loading PaymentMainForm");

            //LogManager.writeLog("Start loading DailyPaymentReportForm");
            dailyPaymentReportForm = new DailyPaymentReportForm();
            dailyPaymentReportForm.load(this);
            this.initialForm(dailyPaymentReportForm);
            //LogManager.writeLog("Finish loading DailyPaymentReportForm");

            //LogManager.writeLog("Start loading CheckInMainForm");
            //checkInMainForm = new CheckInMainForm();
            //checkInMainForm.load(this);
            //this.initialForm(checkInMainForm);
            //LogManager.writeLog("Finish loading CheckInMainForm");

            paymentForm2 = new PaymentForm2();
            paymentForm2.load(this);
            this.initialForm(paymentForm2);

            this.switchForm(loginForm);

            //LogManager.writeLog("Finish MainForm.MainForm_Load()");
        }

        public void loginSuccess(User user)
        {
            this.systemUpdatetimer.Enabled = true;

            this.user = user;
            dailyPaymentReportForm.init(this.user.Role);
            this.switchForm(paymentForm2);

            this.systemUpdatetimer.Start();
        }

        public User getUser()
        {
            return this.user;
        }

        private void ToolStripButton_Home_Click(object sender, EventArgs e)
        {
            this.switchForm(paymentForm2);
        }

        private void ToolStripButton_RegisterStudent_Click(object sender, EventArgs e)
        {
            StudentRegisterForm studentRegisterForm = new StudentRegisterForm();
            Student student = studentRegisterForm.showFormDialog();
            if (student != null)
            {
                MainForm.paymentForm2.setStudent(student);
                this.switchForm(MainForm.paymentForm2);
            }
        }        

        private void ToolStripButton_Student_Click(object sender, EventArgs e)
        {
            studentMainForm.reset();
            this.switchForm(studentMainForm);
        }

        private void ToolStripButton_Teacher_Click(object sender, EventArgs e)
        {
            teacherMainForm.reset();
            this.switchForm(teacherMainForm);
        }

        private void ToolStripButton_Course_Click(object sender, EventArgs e)
        {
            courseMainForm.reset();          
            this.switchForm(courseMainForm);
        }

        private void ToolStripButton_Book_Click(object sender, EventArgs e)
        {
            bookMainForm.reset();
            this.switchForm(bookMainForm);
        }

        private void ToolStripButton_CD_Click(object sender, EventArgs e)
        {
            cdMainForm.reset();
            this.switchForm(cdMainForm);
        }

        private void ToolStripButton_Payment_Click(object sender, EventArgs e)
        {
            paymentMainForm.reset();
            this.switchForm(paymentMainForm);
        }

        private void ToolStripButton_DailyPaymentReport_Click(object sender, EventArgs e)
        {
            dailyPaymentReportForm.reset();
            this.switchForm(dailyPaymentReportForm);
        }

        private void ToolStripButton_AddUser_Click(object sender, EventArgs e)
        {
            UserRegisterForm userRegisterForm = new UserRegisterForm();
            userRegisterForm.showFormDialog();
        }

        private void toolStripButton_CheckIn_Click(object sender, EventArgs e)
        {
            //checkInMainForm.init();
            //checkInMainForm.reload();
            //this.switchForm(checkInMainForm);
            this.switchForm(paymentForm2);            
        } 

        private void ToolStripButton_Logout_Click(object sender, EventArgs e)
        {
            this.systemUpdatetimer.Enabled = false;

            this.user = null;
            loginForm.reset();
            this.switchForm(loginForm);

            this.systemUpdatetimer.Stop();
        }

        private void initialForm(Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            this.Panel_Body.Controls.Add(form);
        }

        public void switchForm(Form form)
        {
            if (currentForm != null)
            {                
                currentForm.Hide();
            }

            if (form is LoginForm)
            {
                this.enableTopMenu(false);
            }
            else
            {
                this.enableTopMenu(true);
            }

            currentForm = form;            
            currentForm.Show();

            if (currentForm is PaymentForm2)
            {
                paymentForm2.updateFocusedTextBox();
            }
        }  
      
        public void switchForm(PaymentForm2 paymentForm2, Student student)
        {
            if (currentForm != null)
            {
                currentForm.Hide();
            }

            paymentForm2.setStudent(student);
            currentForm = paymentForm2;
            currentForm.Show();
        } 
       
        private void enableTopMenu(bool isLoggedIn)
        {
            this.ToolStripButton_Home.Enabled = isLoggedIn;            
            this.ToolStripButton_RegisterStudent.Enabled = isLoggedIn;
            this.ToolStripButton_Student.Enabled = isLoggedIn;
            this.ToolStripButton_Teacher.Enabled = isLoggedIn;
            this.ToolStripButton_Course.Enabled = isLoggedIn;
            this.ToolStripButton_Book.Enabled = isLoggedIn;
            this.ToolStripButton_CD.Enabled = isLoggedIn;
            this.ToolStripButton_Payment.Enabled = isLoggedIn;
            this.ToolStripButton_DailyPaymentReport.Enabled = isLoggedIn;
            //this.toolStripButton_CheckIn.Enabled = isLoggedIn;
          
            if (isLoggedIn)
            {
                if (this.user.Role == (int)User.UserRole.ADMIN)
                {
                    this.ToolStripButton_AddUser.Enabled = true;
                }
                else
                {
                    this.ToolStripButton_AddUser.Enabled = false;
                }
            }            
            else
            {
                this.ToolStripButton_AddUser.Enabled = false;
            }

            this.ToolStripButton_Logout.Enabled = isLoggedIn;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.PreCloseWorker.RunWorkerAsync();
            ProgressBarManager.showProgressBar(true);
        }

        private void PreCloseWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.PreCloseWorker.ReportProgress(0, ProgressBarManager.ProgressBarState.BACKUP_DATABASE);
            DatabaseManager.backup();
        }

        private void PreCloseWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarManager.ProgressBarState progressBarState = (ProgressBarManager.ProgressBarState)e.UserState;
            ProgressBarManager.updateProgressBar(e.ProgressPercentage, progressBarState); 
        }

        private void PreCloseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarManager.showProgressBar(false);
        }

        //private void updateClassroomDetailWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    List<ClassroomDetail> tempClassroomDetailList = ClassroomDetailManager.findAllActiveClassroomDetailByEndDate(DateTime.Today);

        //    foreach (ClassroomDetail cd in tempClassroomDetailList)
        //    {
        //        bool isPass = false;
        //        DateTime today = DateTime.Today;

        //        if (cd.ClassroomDate < today)
        //        {
        //            isPass = true;
        //        }
        //        else
        //        {
        //            TimeSpan time = TimeSpan.Parse(cd.ClassroomTime.Replace(".", ":"));
        //            DateTime classroomEndTime = DateTime.Today.Add(time).AddMinutes(cd.ClassroomDuration);

        //            if (classroomEndTime <= DateTime.Now)
        //            {
        //                isPass = true;
        //            }
        //        }

        //        if (isPass == true)
        //        {
        //            cd.PreviousStatus = cd.CurrentStatus;
        //            cd.CurrentStatus = ClassroomDetail.ClassroomStatus.CHECKED_IN.ToString();

        //            ClassroomDetailManager.updateClassroomDetail(cd);
        //        }
        //    }
        //}

        //private void updateClassroomDetailWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    // Do Nothing
        //}

        //private void updateClassroomDetailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (this.updateStudentStatusWorker.IsBusy == false)
        //    {
        //        //this.updateStudentStatusWorker.RunWorkerAsync();
        //    }
        //}

        //private void updateStudentStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    List<Student> tempStudentList = StudentManager.findAllStudent(Student.StudentStatus.ACTIVE);

        //    foreach (Student s in tempStudentList)
        //    {
        //        bool hasActiveClassroom = false;
        //        List<Enrollment> tempEnrollmentList = EnrollmentManager.findAllEnrollmentByStudentId(s.Id);

        //        foreach (Enrollment en in tempEnrollmentList)
        //        {
        //            List<Classroom> tempClassroomList = ClassroomManager.findAllClassroom(en.Id);

        //            foreach (Classroom c in tempClassroomList)
        //            {
        //                List<ClassroomDetail> tempClassroomDetailList = ClassroomDetailManager.findAllActiveClassroomDetailByClassroomId(c.Id);

        //                if (tempClassroomDetailList.Count > 0)
        //                {
        //                    hasActiveClassroom = true;
        //                    break;
        //                }
        //            }

        //            if (hasActiveClassroom == true)
        //            {
        //                break;
        //            }
        //        }

        //        if (hasActiveClassroom == false)
        //        {
        //            s.Status = Student.StudentStatus.INACTIVE.ToString();
        //            StudentManager.updateStudent(s);
        //        }
        //    }
        //}

        //private void updateStudentStatusWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //}

        //private void updateStudentStatusWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}

        private void systemUpdateTimer_Tick(object sender, EventArgs e)
        {
            if ((DatabaseManager.SYSTEM_UPDATE == true) && (this.user != null) && (this.systemUpdateWorker.IsBusy == false))
            {
                this.systemUpdateWorker.RunWorkerAsync();
            }
        }

        private void systemUpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SystemUpdateManager.update();
        }

        private void systemUpdateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Do Nothing
        }

        private void systemUpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Do Nothing
        }
    }
}
