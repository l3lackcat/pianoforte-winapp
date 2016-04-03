using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

using Excel = Microsoft.Office.Interop.Excel;

using PianoForte.Model;
using PianoForte.Manager;
using System.Globalization;

namespace PianoForte.MigrationService
{
    class SaleDetailService
    {
        public static List<Schedule> create()
        {
            List<Schedule> scheduleList = null;
            List<Payment> paymentList = PaymentManager.findAllPayment();

            if (paymentList.Count > 0)
            {
                string fileName = PianoForte.Constant.Constant.STARTUP_PATH + "\\MigratedData\\sale_detail.xls";

                Excel.Application excelApplication;
                Excel.Workbook excelWorkBook;
                Excel.Worksheet excelWorkSheet;
                object missingValue = System.Reflection.Missing.Value;

                excelApplication = new Excel.ApplicationClass();
                excelWorkBook = excelApplication.Workbooks.Add(missingValue);

                excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.get_Item(1);
                SaleDetailService.createHeader(excelWorkSheet);
                scheduleList = SaleDetailService.createContent(excelWorkSheet, paymentList);

                excelWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);
                excelWorkBook.Close(true, missingValue, missingValue);
                excelApplication.Quit();

                HelperService.releaseObject(excelWorkSheet);
                HelperService.releaseObject(excelWorkBook);
                HelperService.releaseObject(excelApplication);
            }

            return scheduleList;
        }

        private static void createHeader(Excel.Worksheet excelWorkSheet)
        {
            HelperService.addCellHeader(excelWorkSheet, "A1", "A1", 15, "SA01_SaleId");
            HelperService.addCellHeader(excelWorkSheet, "B1", "B1", 15, "SA02_Seq");
            HelperService.addCellHeader(excelWorkSheet, "C1", "C1", 15, "MS07_ItemId");
            HelperService.addCellHeader(excelWorkSheet, "D1", "D1", 15, "MS03_SubjectId");
            HelperService.addCellHeader(excelWorkSheet, "E1", "E1", 15, "SA02_Description");
            HelperService.addCellHeader(excelWorkSheet, "F1", "F1", 15, "SA02_Qty");
            HelperService.addCellHeader(excelWorkSheet, "G1", "G1", 15, "SA02_Discount");
            HelperService.addCellHeader(excelWorkSheet, "H1", "H1", 15, "SA02_UnitPrice");
            HelperService.addCellHeader(excelWorkSheet, "I1", "I1", 15, "SA02_StartDate");
            HelperService.addCellHeader(excelWorkSheet, "J1", "J1", 15, "SA02_SaleType");
            HelperService.addCellHeader(excelWorkSheet, "K1", "K1", 15, "SA02_Frequency");
            HelperService.addCellHeader(excelWorkSheet, "L1", "L1", 15, "SA02_DayOfWeek1");
            HelperService.addCellHeader(excelWorkSheet, "M1", "M1", 15, "SA02_DayOfWeek2");
            HelperService.addCellHeader(excelWorkSheet, "N1", "N1", 15, "MS04_Teacher1Id");
            HelperService.addCellHeader(excelWorkSheet, "O1", "O1", 15, "MS04_Teacher2Id");
            HelperService.addCellHeader(excelWorkSheet, "P1", "P1", 15, "SA02_StartTime1");
            HelperService.addCellHeader(excelWorkSheet, "Q1", "Q1", 15, "SA02_StartTime2");
            HelperService.addCellHeader(excelWorkSheet, "R1", "R1", 15, "SA02_EndTime1");
            HelperService.addCellHeader(excelWorkSheet, "S1", "S1", 15, "SA02_EndTime2");
            HelperService.addCellHeader(excelWorkSheet, "T1", "T1", 15, "SA02_ClassQty");
        }

        private static List<Schedule> createContent(Excel.Worksheet excelWorkSheet, List<Payment> paymentList)
        {
            List<Schedule> scheduleList = new List<Schedule>();
            int SA02_Seq = 0;
            DateTime referenceDate = DateTime.ParseExact("15/11/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            for (int i = 0; i < paymentList.Count; i++)
            {
                Payment payment = paymentList[i];
                User receiver = UserManager.findUser(payment.ReceiverId);
                int paymentId = payment.Id;
                int saleId = i + 1;
                List<PaymentDetail> paymentDetailList = PaymentDetailManager.findAllPaymentDetail(paymentId);

                for (int j = 0; j < paymentDetailList.Count; j++)
                {
                    SA02_Seq++;

                    int rowIndex = SA02_Seq + 1;

                    PaymentDetail paymentDetail = paymentDetailList[j];
                    string SA02_SaleType = "I";

                    if (paymentDetail.Product.Type == Product.ProductType.COURSE.ToString())
                    {
                        SA02_SaleType = "C";
                    }
                    else if (paymentDetail.Product.Type == Product.ProductType.OTHER.ToString())
                    {
                        SA02_SaleType = "O";
                    }

                    HelperService.addCellData(excelWorkSheet, "A" + rowIndex.ToString(), "A" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, saleId.ToString());
                    HelperService.addCellData(excelWorkSheet, "B" + rowIndex.ToString(), "B" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, SA02_Seq.ToString());

                    HelperService.addCellData(excelWorkSheet, "E" + rowIndex.ToString(), "E" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Product.Name);
                    HelperService.addCellData(excelWorkSheet, "F" + rowIndex.ToString(), "F" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Quantity.ToString());
                    HelperService.addCellData(excelWorkSheet, "G" + rowIndex.ToString(), "G" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Discount.ToString());
                    HelperService.addCellData(excelWorkSheet, "H" + rowIndex.ToString(), "H" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Product.Price.ToString());

                    HelperService.addCellData(excelWorkSheet, "J" + rowIndex.ToString(), "J" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, SA02_SaleType);
                    
                    if (SA02_SaleType == "C")
                    {
                        HelperService.addCellData(excelWorkSheet, "C" + rowIndex.ToString(), "C" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                        HelperService.addCellData(excelWorkSheet, "D" + rowIndex.ToString(), "D" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Product.Id.ToString());

                        List<Enrollment> enrollmentList = EnrollmentManager.findAllEnrollmentByPaymentId(paymentId);
                        
                        if (enrollmentList.Count > 0)
                        {
                            Enrollment enrollment = enrollmentList[0];
                            List<Classroom> classroomList = ClassroomManager.findAllClassroom(enrollment.Id);
                            int numberOfClassroon = classroomList.Count;

                            if (numberOfClassroon > 0)
                            {
                                int SA02_ClassQty = 0;
                                Classroom classroom1 = classroomList[0];
                                DateTime startDate = classroom1.StartDate;
                                TimeSpan startTime1 = TimeSpan.Parse(classroom1.ClassTime.Replace(".", ":"));
                                TimeSpan endTime1 = startTime1.Add(TimeSpan.FromMinutes(classroom1.ClassDuration));
                                int numberOfRepeat1 = ClassroomDetailManager.findAllClassroomDetailByClassroomId(classroom1.Id).Count;
                                
                                Schedule schedule1 = new Schedule();
                                Schedule schedule2 = null;

                                schedule1.SaleId = saleId;
                                schedule1.SaleDetailId = SA02_Seq;
                                schedule1.TeacherId = classroom1.TeacherId;
                                schedule1.NumberOfRepeat = numberOfRepeat1;
                                schedule1.StartTime = classroom1.StartDate.Add(startTime1);
                                schedule1.EndTime = classroom1.StartDate.Add(endTime1);

                                SA02_ClassQty += numberOfRepeat1;

                                HelperService.addCellData(excelWorkSheet, "K" + rowIndex.ToString(), "K" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, numberOfClassroon.ToString());

                                HelperService.addCellData(excelWorkSheet, "L" + rowIndex.ToString(), "L" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, ConvertManager.toNumDayOfWeek(classroom1.ClassDayOfWeek).ToString());
                                HelperService.addCellData(excelWorkSheet, "N" + rowIndex.ToString(), "N" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, classroom1.TeacherId.ToString());
                                HelperService.addCellData(excelWorkSheet, "P" + rowIndex.ToString(), "P" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, startTime1.ToString());
                                HelperService.addCellData(excelWorkSheet, "R" + rowIndex.ToString(), "R" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, endTime1.ToString());

                                if (numberOfClassroon == 2)
                                {
                                    Classroom classroom2 = classroomList[1];
                                    TimeSpan startTime2 = TimeSpan.Parse(classroom2.ClassTime.Replace(".", ":"));
                                    TimeSpan endTime2 = startTime1.Add(TimeSpan.FromMinutes(classroom2.ClassDuration));
                                    int numberOfRepeat2 = ClassroomDetailManager.findAllClassroomDetailByClassroomId(classroom2.Id).Count;

                                    schedule2 = new Schedule();
                                    schedule2.SaleId = saleId;
                                    schedule2.SaleDetailId = SA02_Seq;
                                    schedule2.TeacherId = classroom2.TeacherId;
                                    schedule2.NumberOfRepeat = numberOfRepeat2;
                                    schedule2.StartTime = classroom2.StartDate.Add(startTime2);
                                    schedule2.EndTime = classroom2.StartDate.Add(endTime2);

                                    SA02_ClassQty += numberOfRepeat2;

                                    if (startDate > classroom2.StartDate)
                                    {
                                        startDate = classroom2.StartDate;
                                    }

                                    HelperService.addCellData(excelWorkSheet, "M" + rowIndex.ToString(), "M" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, ConvertManager.toNumDayOfWeek(classroom2.ClassDayOfWeek).ToString());
                                    HelperService.addCellData(excelWorkSheet, "O" + rowIndex.ToString(), "O" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, classroom2.TeacherId.ToString());
                                    HelperService.addCellData(excelWorkSheet, "Q" + rowIndex.ToString(), "Q" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, startTime2.ToString());
                                    HelperService.addCellData(excelWorkSheet, "S" + rowIndex.ToString(), "S" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, endTime2.ToString());
                                }

                                HelperService.addCellData(excelWorkSheet, "I" + rowIndex.ToString(), "I" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, startDate.ToShortDateString());

                                HelperService.addCellData(excelWorkSheet, "T" + rowIndex.ToString(), "T" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, SA02_ClassQty.ToString());

                                if (startDate >= referenceDate)
                                {
                                    scheduleList.Add(schedule1);

                                    if (schedule2 != null)
                                    {
                                        scheduleList.Add(schedule2);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        HelperService.addCellData(excelWorkSheet, "C" + rowIndex.ToString(), "C" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, paymentDetail.Product.Id.ToString());
                        HelperService.addCellData(excelWorkSheet, "D" + rowIndex.ToString(), "D" + rowIndex.ToString(), Excel.XlHAlign.xlHAlignLeft, "");
                    }
                }
            }

            return scheduleList;
        }
    }
}
