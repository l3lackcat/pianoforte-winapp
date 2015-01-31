using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PianoForte.Manager;
using PianoForte.Model;

namespace PianoForte.View
{
    public partial class ClassroomDetailForm : Form
    {
        public ClassroomDetailForm()
        {
            InitializeComponent();
        }

        public void showFormDialog(ClassroomDetail classroomDetail, string courseName, string courseLevel)
        {
            Teacher teacher = TeacherManager.findTeacher(classroomDetail.TeacherId);

            if (teacher != null)
            {
                this.Label_ClassroomDate.Text = classroomDetail.ClassroomDate.ToShortDateString();
                this.Label_ClassroomTime.Text = classroomDetail.ClassroomTime;
                this.Label_CourseName.Text = courseName + " - " + courseLevel;
                this.Label_TeacherName.Text = teacher.Firstname + " " + teacher.Lastname + " (" + teacher.Nickname + ")";
            }
            
            this.ShowDialog(); 
        }
    }
}
