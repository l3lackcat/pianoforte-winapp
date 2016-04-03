using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PianoForte.Model;
using PianoForte.MigrationService;

namespace PianoForte.View
{
    public partial class HiddenForm2 : Form
    {
        public HiddenForm2()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            CourseCategoryService.create();
            Dictionary<string, int> courseNameAndIdDictionary = CourseNameService.create();
            Dictionary<int, int> courseLevelIdAndcourseNameIdDictionary = CourseLevelService.create(courseNameAndIdDictionary);
            CoursePriceService.create();

            ProductService.create();
            ProductPriceService.create();

            TeacherService.create();
            TeachingCourseService.create(courseLevelIdAndcourseNameIdDictionary);

            StudentService.create();

            SaleService.create();
            List<Schedule> scheduleList = SaleDetailService.create();
            ScheduleService.create(scheduleList);
        }
    }
}
