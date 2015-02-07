﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PianoForte.Manager;
using PianoForte.Model;
using PianoForte.Constant;
using System.Globalization;

namespace PianoForte.View
{
    public partial class PaymentForm2 : Form
    {
        private MainForm mainForm;

        private Student student;
        private OtherCost firstRegister;
        private Enrollment enrollment;
        private List<PaymentDetail> paymentDetailList;
        private List<SavedPayment> studentSavedPaymentList;
        private int selectedStudentSavedPaymentIndex = 0;
        private Dictionary<int, SavedPayment> unpaidSavedPaymentDictionary;
        private int unpaidSavedPaymentId = 0;

        public PaymentForm2()
        {
            InitializeComponent();
        }

        public void load(MainForm mainForm)
        {
            this.mainForm = mainForm;

            this.student = null;            
            this.enrollment = null;
            this.paymentDetailList = new List<PaymentDetail>();
            this.studentSavedPaymentList = new List<SavedPayment>();
            this.unpaidSavedPaymentDictionary = new Dictionary<int, SavedPayment>();

            this.firstRegister = OtherCostManager.findOtherCost(4000001);

            this.initTextBoxPaymentDate();
            this.updateUnpaidSavedPaymentDataGridView();
            this.initPaymentDetailSummaryDataGridView();
        }

        public void reload()
        {
            //Do Nothing
        }

        public void reset()
        {
            //Do Nothing
        }

        public int getProductQuantity(int productId)
        {
            int quantity = 0;

            int numberOfPaymentDetailList = this.paymentDetailList.Count;
            for (int i = 0; i < numberOfPaymentDetailList; i++)
            {
                PaymentDetail paymentDetail = this.paymentDetailList[i];
                if (paymentDetail.Product.Id == productId)
                {
                    quantity = paymentDetail.Quantity;
                    break;
                }
            }

            return quantity;
        }

        public void setStudent(Student student)
        {
            if (student != null)
            {
                this.student = student;

                this.TextBox_StudentId.Text = student.Id.ToString();
                this.TextBox_StudentNickname.Text = student.Nickname;
                this.TextBox_StudentFullName.Text = student.Firstname + " " + student.Lastname;
                this.TextBox_StudentPhoneNumber.Text = student.Phone2;

                this.TextBox_Barcode.Enabled = true;
                this.Button_SearchBarcode.Enabled = true;
                this.Button_SelectBook.Enabled = true;
                this.Button_SelectCD.Enabled = true;
                this.Button_AddOther.Enabled = true;

                if (student.Id != 1)
                {
                    this.Button_SelectCourse.Enabled = true;
                }
                
                this.TextBox_Barcode.Focus();

                this.searchUnpaidPayment(student.Id);
            }

            this.updateButtonPay();
            this.updateButtonSave();
            this.updateButtonCancelUnpaidPayment();
        }

        public void updateFocusedTextBox()
        {
            if (this.student == null)
            {
                this.TextBox_StudentId.Focus();
            }
            else
            {
                this.TextBox_Barcode.Focus();
            }
        }

        private bool updateEnrollment(Enrollment enrollment)
        {
            Product product = null;
            double discount = 0;

            if (enrollment != null)
            {
                this.enrollment = enrollment;

                string productName = enrollment.Course.Name;
                if (enrollment.Course.Level != "")
                {
                    productName += " - ";
                    productName += enrollment.Course.Level;
                }

                product = new Product();
                product.Id = enrollment.Course.Id;
                product.Type = Product.ProductType.COURSE.ToString();
                product.Name = productName;
                product.Price = enrollment.TotalPrice;

                discount = enrollment.Discount;
            }

            if (product != null)
            {
                PaymentDetail paymentDetail = new PaymentDetail();
                paymentDetail.Product = product;
                paymentDetail.Quantity = 1;
                paymentDetail.Discount = discount;

                this.addPaymentDetail(paymentDetail);
            }

            return true;
        }

        public void addFirstRegisterToPaymentDetail()
        {
            if (this.student != null)
            {
                if (this.student.Status == Student.StudentStatus.INACTIVE.ToString())
                {
                    DateTime? lastDateOfClass = this.student.LastDateOfClass;
                    if ((lastDateOfClass == null) || (lastDateOfClass < DateTime.Today.AddDays(-90)))
                    {
                        Product product = new Product();
                        product.Id = this.firstRegister.Id;
                        product.Type = Product.ProductType.OTHER.ToString();
                        product.Name = this.firstRegister.Name;
                        product.Price = this.firstRegister.Price;

                        PaymentDetail paymentDetail = new PaymentDetail();
                        paymentDetail.Product = product;
                        paymentDetail.Quantity = 1;

                        this.addPaymentDetail(paymentDetail);
                    }
                }
            }
        }

        public bool addPaymentDetail(PaymentDetail paymentDetail)
        {
            bool isSuccess = false;

            int index = this.getPaymentDetailListIndex(paymentDetail.Product.Id);
            if (index >= 0)
            {
                if (paymentDetail.Product.Id != 4000001)
                {
                    this.paymentDetailList[index].Product = paymentDetail.Product;
                    this.paymentDetailList[index].Quantity += paymentDetail.Quantity;
                }

                isSuccess = true;
            }
            else
            {
                int numberOfPaymentDetail = this.paymentDetailList.Count;
                if (numberOfPaymentDetail < 12)
                {
                    this.paymentDetailList.Add(paymentDetail);
                    isSuccess = true;
                }
                else
                {
                    MessageBox.Show(Constant.Constant.OVER_12_ITEMS);
                }
            }

            if (isSuccess)
            {
                if (paymentDetail.Product.Type == Product.ProductType.COURSE.ToString())
                {
                    this.Button_SelectCourse.Enabled = false;

                    this.addFirstRegisterToPaymentDetail();

                    if (this.selectedStudentSavedPaymentIndex != 0)
                    {
                        this.enrollment = EnrollmentManager.findEnrollmentBySavedPaymentId(paymentDetail.SavedPaymentId);
                    }
                }
                else if (paymentDetail.Product.Type == Product.ProductType.OTHER.ToString())
                {
                    if ((paymentDetail.Product.Id == 4000001) && (this.CheckBox_AddFirstRegisterCost.Checked == false))
                    {
                        this.CheckBox_AddFirstRegisterCost.Checked = true;
                    }
                }

                this.sortPaymentDetailList();
                this.updatePaymentDetailSummaryDataGridView();
                this.updateTextBoxGrandTotal();
            }

            this.updateButtonPay();
            this.updateButtonSave();

            return isSuccess;
        }

        private void clearPaymentDetail()
        {
            foreach (PaymentDetail paymentDetail in this.paymentDetailList)
            {
                if (paymentDetail.Product.Type == Product.ProductType.COURSE.ToString())
                {
                    this.enrollment = null;
                    this.Button_SelectCourse.Enabled = true;
                }
            }

            this.paymentDetailList.Clear();

            this.sortPaymentDetailList();
            this.updatePaymentDetailSummaryDataGridView();
            this.updateTextBoxGrandTotal();
            this.updateButtonPay();
            this.updateButtonSave();
        }

        private bool removePaymentDetail(int productId)
        {
            bool isSuccess = false;
            PaymentDetail paymentDetail = null;

            int index = this.getPaymentDetailListIndex(productId);
            if (index >= 0)
            {
                paymentDetail = this.paymentDetailList[index];

                this.paymentDetailList.Remove(this.paymentDetailList[index]);
                isSuccess = true;
            }

            if (isSuccess)
            {
                if (paymentDetail != null)
                {
                    if (paymentDetail.Product.Type == Product.ProductType.COURSE.ToString())
                    {
                        this.enrollment = null;
                        this.Button_SelectCourse.Enabled = true;

                        this.removePaymentDetail(this.firstRegister.Id);
                    }
                }

                this.sortPaymentDetailList();
                this.updatePaymentDetailSummaryDataGridView();
                this.updateTextBoxGrandTotal();
            }

            this.updateButtonPay();
            this.updateButtonSave();

            return isSuccess;
        }

        private void reset(bool isResetAll)
        {
            this.student = null;
            this.enrollment = null;
            this.paymentDetailList.Clear();
            this.studentSavedPaymentList.Clear();
            this.selectedStudentSavedPaymentIndex = 0;
            this.unpaidSavedPaymentDictionary.Clear();
            this.unpaidSavedPaymentId = 0;

            this.TextBox_StudentId.Text = "";
            this.TextBox_StudentId.Focus();

            this.ComboBox_Unpaid_Payment.Visible = false;
            this.ComboBox_Unpaid_Payment.Items.Clear();

            this.CheckBox_AddFirstRegisterCost.Checked = false;

            this.TextBox_StudentNickname.Text = "";
            this.TextBox_StudentFullName.Text = "";
            this.TextBox_StudentPhoneNumber.Text = "";
            this.TextBox_PaymentDate.Text = DateTime.Today.ToShortDateString();

            this.TextBox_Barcode.Enabled = false;
            this.TextBox_Barcode.Text = "";
            this.Button_SearchBarcode.Enabled = false;

            this.Button_SelectCourse.Enabled = false;
            this.Button_SelectBook.Enabled = false;
            this.Button_SelectCD.Enabled = false;
            this.Button_AddOther.Enabled = false;
            this.CheckBox_AddFirstRegisterCost.Enabled = false;

            this.TextBox_GrandTotalText.Text = "";
            this.TextBox_GrandTotal.Text = "0.00";

            this.TextBox_CreditCardNumber1.Text = "";
            this.TextBox_CreditCardNumber2.Text = "";
            this.TextBox_CreditCardNumber3.Text = "";
            this.TextBox_CreditCardNumber4.Text = "";

            this.RadioButton_Cash.Checked = true;

            this.Button_Pay.Enabled = false;
            this.Button_Save.Enabled = false;
            this.Button_Cancel_UnpaidPayment.Enabled = false;

            this.updateUnpaidSavedPaymentDataGridView();
            this.updatePaymentDetailSummaryDataGridView();
        }

        private void initTextBoxPaymentDate()
        {
            this.TextBox_PaymentDate.Text = DateTime.Today.ToShortDateString();
        }

        private void initPaymentDetailSummaryDataGridView()
        {
            for (int i = 1; i <= 12; i++)
            {
                int n = this.DataGridView_PaymentDetail_Summary.Rows.Add();
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["No"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["ItemName"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["Quantity"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["Discount"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["Price"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["TotalPrice"].Value = "";               
            }
        }

        private void initUnpaidSavedPaymentDataGridView()
        {
            for (int i = 1; i <= 12; i++)
            {
                int n = this.DataGridView_UnpaidSavedPayment.Rows.Add();
                this.DataGridView_UnpaidSavedPayment.Rows[n].Cells["Col_No"].Value = "";
                this.DataGridView_PaymentDetail_Summary.Rows[n].Cells["Col_Name"].Value = "";
            }
        }

        private void updatePaymentDetailSummaryDataGridView()
        {
            int numberOfPaymentDetailList = this.paymentDetailList.Count;

            for (int i = 0; i < 12; i++)
            {
                if (i < numberOfPaymentDetailList)
                {
                    PaymentDetail paymentDetail = this.paymentDetailList[i];
                    Product product = paymentDetail.Product;

                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["No"].Value = i + 1;
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["ItemName"].Value = product.Name;
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Quantity"].Value = paymentDetail.Quantity;
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Discount"].Value = paymentDetail.Discount;
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Price"].Value = product.Price;
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["TotalPrice"].Value = (product.Price * paymentDetail.Quantity) - paymentDetail.Discount;

                    if ((paymentDetail.Id == 0) && (product.Id != this.firstRegister.Id))
                    {
                        ((DataGridViewImageCell)this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["DeleteButton"]).Value = PianoForte.Properties.Resources.Delete;
                    }
                    else
                    {
                        ((DataGridViewImageCell)this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["DeleteButton"]).Value = PianoForte.Properties.Resources.Empty;
                    }
                }
                else
                {
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["No"].Value = "";
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["ItemName"].Value = "";
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Quantity"].Value = "";
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Discount"].Value = "";
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["Price"].Value = "";
                    this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["TotalPrice"].Value = "";

                    ((DataGridViewImageCell)this.DataGridView_PaymentDetail_Summary.Rows[i].Cells["DeleteButton"]).Value = PianoForte.Properties.Resources.Empty;
                }
            }
        }

        private void updateUnpaidSavedPaymentDataGridView()
        {
            List<SavedPayment> unpaidSavedPaymentDictionary = SavedPaymentManager.findAllSavedPayment(SavedPayment.SavedPaymentStatus.NOT_PAID);

            this.unpaidSavedPaymentDictionary.Clear();
            this.DataGridView_UnpaidSavedPayment.Rows.Clear();

            foreach (SavedPayment sp in unpaidSavedPaymentDictionary)
            {
                Student s = StudentManager.findStudent(sp.StudentId);

                if (s != null)
                {
                    int n = this.DataGridView_UnpaidSavedPayment.Rows.Add();

                    this.DataGridView_UnpaidSavedPayment.Rows[n].Cells["Col_No"].Value = n + 1;
                    this.DataGridView_UnpaidSavedPayment.Rows[n].Cells["Col_Name"].Value = s.Firstname + " (" + s.Nickname + ")";

                    this.unpaidSavedPaymentDictionary.Add(n, sp);
                }
            }
        }

        private void updateTextBoxGrandTotal()
        {
            double grandTotalPrice = 0;

            int numberOfPaymentDetailList = this.paymentDetailList.Count;

            for (int i = 0; i < numberOfPaymentDetailList; i++)
            {
                PaymentDetail paymentDetail = this.paymentDetailList[i];
                Product product = paymentDetail.Product;

                grandTotalPrice += ((product.Price * paymentDetail.Quantity) - paymentDetail.Discount);
            }

            this.TextBox_GrandTotal.Text = grandTotalPrice.ToString("N", new CultureInfo("en-US"));

            if (grandTotalPrice > 0)
            {
                this.TextBox_GrandTotalText.Text = ConvertManager.toBahtText(grandTotalPrice);
            }
            else
            {
                this.TextBox_GrandTotalText.Text = "";
            }
        }

        private void searchStudent(int studentId)
        {
            this.reset(true);
            this.setStudent(StudentManager.findStudent(studentId));
        }

        private void searchStudent(int studentId, int savedPaymentId)
        {
            this.reset(true);
            this.unpaidSavedPaymentId = savedPaymentId;
            this.setStudent(StudentManager.findStudent(studentId));
        }

        private void searchUnpaidPayment(int studentId)
        {
            this.studentSavedPaymentList = SavedPaymentManager.findAllSavedPaymentByStudentId(studentId, SavedPayment.SavedPaymentStatus.NOT_PAID);
            int numberOfSavedPayment = this.studentSavedPaymentList.Count;
            int selectedIndex = 0;

            if (numberOfSavedPayment > 0)
            {
                
                this.ComboBox_Unpaid_Payment.Visible = true;
                this.ComboBox_Unpaid_Payment.Items.Clear();
                this.ComboBox_Unpaid_Payment.Items.Add("ทำรายการใหม่");
                this.ComboBox_Unpaid_Payment.SelectedIndex = 0;

                for (int i = 0; i < numberOfSavedPayment; i++)
                {
                    this.ComboBox_Unpaid_Payment.Items.Add("ค้างชำระ " + (i+1));

                    if (this.unpaidSavedPaymentId == this.studentSavedPaymentList[i].Id)
                    {
                        selectedIndex = i + 1;
                    }
                }

                if (this.unpaidSavedPaymentId == 0) {
                    DialogResult result = MessageBox.Show("มีรายการค้างชำระ " + numberOfSavedPayment + " รายการ ต้องการเปิดดูหรือไม่?", "มีรายการค้างชำระ", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        this.ComboBox_Unpaid_Payment.SelectedIndex = numberOfSavedPayment;
                    }
                    else
                    {
                        this.ComboBox_Unpaid_Payment.SelectedIndex = 0;
                    }
                } else {
                    this.ComboBox_Unpaid_Payment.SelectedIndex = selectedIndex;
                }
            }
            else
            {
                this.ComboBox_Unpaid_Payment.Visible = false;
                this.ComboBox_Unpaid_Payment.Items.Clear();
            }
        }

        private void searchProduct(string barcode)
        {
            Product product = null;
            bool isCourse = false;
            int courseIndex = -1;

            Book book = BookManager.findBookByBarcode(barcode);
            if (book != null)
            {
                product = new Product();
                product.Id = book.Id;
                product.Type = Product.ProductType.BOOK.ToString();
                product.Name = book.Name;
                product.Price = book.Price;
            }
            else
            {
                Cd cd = CdManager.findCdByBarcode(barcode);
                if (cd != null)
                {
                    product = new Product();
                    product.Id = cd.Id;
                    product.Type = Product.ProductType.CD.ToString();
                    product.Name = cd.Name;
                    product.Price = cd.Price;
                }
                else
                {
                    if (ValidateManager.isNumber(barcode) == true)
                    {
                        OtherCost otherProduct = OtherCostManager.findOtherCost(Convert.ToInt32(barcode));
                        if (otherProduct != null)
                        {
                            product = new Product();
                            product.Id = otherProduct.Id;
                            product.Type = Product.ProductType.OTHER.ToString();
                            product.Name = otherProduct.Name;
                            product.Price = otherProduct.Price;
                        }
                        else
                        {
                            Course course = CourseManager.findCourse(Convert.ToInt32(barcode));
                            if (course != null)
                            {
                                isCourse = true;
                                courseIndex = this.getCourseIndexInPaymentDetailList();
                                if (courseIndex == -1)
                                {
                                    EnrollmentPopUp enrollmentPopUp = new EnrollmentPopUp();
                                    Enrollment enrollment = enrollmentPopUp.showFormDialog(this, course);
                                    if (enrollment != null)
                                    {
                                        this.updateEnrollment(enrollment);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("ไม่สามารถเพิ่มวิชาเรียนมากกว่า 1 วิชาได้");
                                }
                            }
                        }
                    }
                }
            }

            if (product != null)
            {
                if (product.Type != Product.ProductType.COURSE.ToString())
                {
                    PaymentDetail paymentDetail = new PaymentDetail();
                    paymentDetail.Product = product;
                    paymentDetail.Quantity = 1;

                    this.addPaymentDetail(paymentDetail);
                }
            }
            else
            {
                if (isCourse == false)
                {
                    if (courseIndex == -1)
                    {
                        MessageBox.Show(DatabaseConstant.DATA_NOT_FOUND);
                    }
                }
            }

            this.TextBox_Barcode.Text = "";
            this.TextBox_Barcode.Focus();
        }        

        private int getCourseIndexInPaymentDetailList()
        {
            int courseIndex = -1;
            int numberOfPaymentDetailList = this.paymentDetailList.Count;

            for (int i = 0; i < numberOfPaymentDetailList; i++)
            {
                if (this.paymentDetailList[i].Product.Type == Product.ProductType.COURSE.ToString())
                {
                    courseIndex = i;
                    break;
                }
            }

            return courseIndex;
        }

        private int getPaymentDetailListIndex(int productId)
        {
            int index = -1;
            int numberOfPaymentDetailList = this.paymentDetailList.Count;

            for (int i = 0; i < numberOfPaymentDetailList; i++)
            {
                if (this.paymentDetailList[i].Product.Id == productId)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private void sortPaymentDetailList()
        {
            int numberOfPaymentDetailList = this.paymentDetailList.Count;
            for (int i = 0; i < numberOfPaymentDetailList; i++)
            {
                for (int j = i + 1; j < numberOfPaymentDetailList; j++)
                {
                    if (this.paymentDetailList[j].Product.Id < this.paymentDetailList[i].Product.Id)
                    {
                        PaymentDetail tempPaymentDetail = this.paymentDetailList[i];
                        this.paymentDetailList[i] = this.paymentDetailList[j];
                        this.paymentDetailList[j] = tempPaymentDetail;

                        break;
                    }
                }
            }
        }

        private void updateButtonPay()
        {
            bool isEnableButtonPay = false;

            if ((this.student != null) && (this.paymentDetailList.Count > 0))
            {
                if (this.RadioButton_CreditCard.Checked == true)
                {
                    string creditCardNumber = this.getCreditCardNumber();
                    
                    if (ValidateManager.validateCreditCardNumber(creditCardNumber) == true)
                    {
                        isEnableButtonPay = true;
                    }
                }
                else
                {
                    isEnableButtonPay = true;
                }
            }

            if (isEnableButtonPay == true)
            {
                this.Button_Pay.Enabled = true;
            }
            else
            {
                this.Button_Pay.Enabled = false;
            }
        }

        private void updateButtonSave()
        {
            bool isEnableButtonSave = false;

            if (this.selectedStudentSavedPaymentIndex == 0)
            {
                if (this.student != null)
                {
                    if (this.paymentDetailList.Count > 0)
                    {
                        if (this.enrollment == null)
                        {
                            isEnableButtonSave = true;
                        }
                        else
                        {
                            List<Classroom> classroomList = this.enrollment.ClassroomList;
                            foreach (Classroom c in classroomList)
                            {
                                List<ClassroomDetail> classroomDetailList = this.enrollment.ClassroomIdClassroomDetailListDictionary[c.Id];

                                foreach (ClassroomDetail cd in classroomDetailList)
                                {
                                    if (cd.ClassroomDate < DateTime.Today)
                                    {
                                        isEnableButtonSave = true;
                                        break;
                                    }
                                }

                                if (isEnableButtonSave == true)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (PaymentDetail paymentDetail in this.paymentDetailList)
                {
                    if (paymentDetail.Id == 0)
                    {
                        isEnableButtonSave = true;
                        break;
                    }
                }
            }

            if (isEnableButtonSave == true)
            {
                this.Button_Save.Enabled = true;
            }
            else
            {
                this.Button_Save.Enabled = false;
            }
        }

        private void updateButtonCancelUnpaidPayment()
        {
            bool isEnableButtonCancel = false;

            if (this.selectedStudentSavedPaymentIndex > 0)
            {
                isEnableButtonCancel = true;
            }

            if (isEnableButtonCancel == true)
            {
                this.Button_Cancel_UnpaidPayment.Enabled = true;
            }
            else
            {
                this.Button_Cancel_UnpaidPayment.Enabled = false;
            }
        }

        private string getCreditCardNumber()
        {
            string creditCardNumber = "";
            string creditCardNumber1 = this.TextBox_CreditCardNumber1.Text;
            string creditCardNumber2 = this.TextBox_CreditCardNumber2.Text;
            string creditCardNumber3 = this.TextBox_CreditCardNumber3.Text;
            string creditCardNumber4 = this.TextBox_CreditCardNumber4.Text;

            if ((creditCardNumber1 != "") && (creditCardNumber2 != "") && (creditCardNumber3 != "") && (creditCardNumber4 != ""))
            {
                creditCardNumber += creditCardNumber1;
                creditCardNumber += creditCardNumber2;
                creditCardNumber += creditCardNumber3;
                creditCardNumber += creditCardNumber4;
            }

            return creditCardNumber;
        }

        private double getGrandTotalPrice()
        {            
            string grandTotalText = this.TextBox_GrandTotal.Text.Replace(",", "");

            return Convert.ToDouble(grandTotalText);
        }

        private void processPayment(string creditCardNumber, double grandTotalPrice)
        {
            int receiverId = this.mainForm.getUser().Id;
            Payment newPayment = PaymentManager.processPayment(this.student.Id, receiverId, creditCardNumber, grandTotalPrice, Payment.PaymentStatus.PAID);

            if (newPayment != null)
            {
                if (this.selectedStudentSavedPaymentIndex > 0)
                {
                    SavedPayment savedPayment = this.studentSavedPaymentList[this.selectedStudentSavedPaymentIndex - 1];

                    if (savedPayment != null)
                    {
                        bool isUpdateComplete = false;

                        savedPayment.PaymentId = newPayment.Id;
                        savedPayment.Status = SavedPayment.SavedPaymentStatus.PAID.ToString();
                        isUpdateComplete = SavedPaymentManager.updateSavedPayment(savedPayment);

                        if (isUpdateComplete == true)
                        {
                            if (this.enrollment != null)
                            {
                                this.enrollment.PaymentId = newPayment.Id;
                                this.enrollment.Status = Enrollment.EnrollmentStatus.PAID.ToString();

                                isUpdateComplete = EnrollmentManager.updateEnrollment(this.enrollment);
                            }

                            if (isUpdateComplete == true)
                            {
                                foreach (PaymentDetail paymentDetail in this.paymentDetailList)
                                {
                                    paymentDetail.PaymentId = newPayment.Id;

                                    isUpdateComplete = PaymentDetailManager.updatePaymentDetail(paymentDetail);

                                    if (isUpdateComplete == false)
                                    {
                                        break;
                                    }
                                }
                            }
                            
                            if (isUpdateComplete == true)
                            {
                                this.printReceipt(newPayment.Id);
                                MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_SUCCESS);
                                this.reset(true);
                            }
                            else
                            {
                                MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_FAIL);
                            }
                        }
                        else
                        {
                            MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_FAIL);
                        }
                    }
                }
                else
                {
                    if (this.processPaymentDetail(newPayment.Id, 0, Payment.PaymentStatus.PAID))
                    {
                        this.printReceipt(newPayment.Id);
                        MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_SUCCESS);
                        this.reset(true);
                    }
                    else
                    {
                        MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_FAIL);
                    }
                }
            }
            else
            {
                MessageBox.Show(PianoForte.Constant.Constant.PAYMENT_FAIL);
            }
        }

        private bool processPaymentDetail(int paymentId, int savedPaymentId, Payment.PaymentStatus paymentStatus)
        {
            bool isAddComplete = false;

            for (int i = 0; i < this.paymentDetailList.Count; i++)
            {
                PaymentDetail paymentDetail = this.paymentDetailList[i];
                if ((paymentDetail != null) && (paymentDetail.Id == 0))
                {
                    paymentDetail.PaymentId = paymentId;
                    paymentDetail.SavedPaymentId = savedPaymentId;

                    isAddComplete = PaymentDetailManager.insertPaymentDetail(paymentDetail);
                    if (isAddComplete)
                    {
                        string productType = paymentDetail.Product.Type;
                        if (productType == Product.ProductType.COURSE.ToString())
                        {
                            if (this.enrollment != null)
                            {
                                this.enrollment.PaymentId = paymentId;
                                this.enrollment.SavedPaymentId = savedPaymentId;
                                this.enrollment.Student = this.student;

                                if (paymentStatus == Payment.PaymentStatus.PAID)
                                {
                                    this.enrollment.Status = Enrollment.EnrollmentStatus.PAID.ToString();
                                }
                                else if (paymentStatus == Payment.PaymentStatus.NOT_PAID)
                                {
                                    this.enrollment.Status = Enrollment.EnrollmentStatus.NOT_PAID.ToString();
                                }
                                
                                if ((EnrollmentManager.processEnrollment(this.enrollment) == true) && (this.student != null))
                                {
                                    foreach (Classroom c in this.enrollment.ClassroomList)
                                    {
                                        foreach (ClassroomDetail cd in this.enrollment.ClassroomIdClassroomDetailListDictionary[c.Id])
                                        {
                                            if ((this.student.LastDateOfClass == null) ||
                                                (this.student.LastDateOfClass < cd.ClassroomDate))
                                            {
                                                this.student.LastDateOfClass = cd.ClassroomDate;
                                            }
                                        }
                                    }

                                    StudentManager.updateStudent(this.student);
                                }
                            }

                            if (this.student != null)
                            {
                                if (this.student.Status != Student.StudentStatus.ACTIVE.ToString())
                                {
                                    this.student.Status = Student.StudentStatus.ACTIVE.ToString();
                                    StudentManager.updateStudent(this.student);
                                }                                
                            }
                        }
                        else if (productType == Product.ProductType.BOOK.ToString())
                        {
                            Book tempBook = BookManager.findBook(paymentDetail.Product.Id);
                            if (tempBook != null)
                            {
                                tempBook.Quantity = tempBook.Quantity - paymentDetail.Quantity;
                                if (tempBook.Quantity == 0)
                                {
                                    tempBook.Status = Book.BookStatus.EMPTY.ToString();
                                }

                                BookManager.updateBook(tempBook);
                            }
                        }
                        else if (productType == Product.ProductType.CD.ToString())
                        {
                            Cd tempCd = CdManager.findCd(paymentDetail.Product.Id);
                            if (tempCd != null)
                            {
                                tempCd.Quantity = tempCd.Quantity - paymentDetail.Quantity;
                                if (tempCd.Quantity == 0)
                                {
                                    tempCd.Status = Cd.CdStatus.EMPTY.ToString();
                                }

                                CdManager.updateCd(tempCd);
                            }
                        }
                        else if (productType == Product.ProductType.OTHER.ToString())
                        {
                            // Do Nothing
                        }
                    }
                }
            }

            return isAddComplete;
        }

        private void printReceipt(int paymentId)
        {
            if (!ReceiptManager.printReceipt(paymentId))
            {
                MessageBox.Show(PianoForte.Constant.Constant.PRINTER_NOT_FOUND);
            }
        }

        private void savePayment()
        {
            int receiverId = this.mainForm.getUser().Id;
            double grandTotalPrice = this.getGrandTotalPrice();
            SavedPayment savedPayment = null;

            if (this.selectedStudentSavedPaymentIndex > 0)
            {
                savedPayment = this.studentSavedPaymentList[this.selectedStudentSavedPaymentIndex - 1];
            }

            if (savedPayment == null)
            {
                savedPayment = SavedPaymentManager.processSavedPayment(this.student.Id, receiverId, grandTotalPrice, SavedPayment.SavedPaymentStatus.NOT_PAID);   
            }
            else
            {
                savedPayment.ReceiverId = receiverId;
                savedPayment.TotalPrice = grandTotalPrice;
                savedPayment.SavedDate = DateTime.Today;

                if (SavedPaymentManager.updateSavedPayment(savedPayment) == false)
                {
                    savedPayment = null;
                }
            }

            if (savedPayment != null)
            {
                if (this.processPaymentDetail(0, savedPayment.Id, Payment.PaymentStatus.NOT_PAID))
                {
                    MessageBox.Show(PianoForte.Constant.Constant.SAVE_PAYMENT_SUCCESS);
                    this.reset(true);
                }
                else
                {
                    MessageBox.Show(PianoForte.Constant.Constant.SAVE_PAYMENT_FAIL);
                }
            }
            else
            {
                MessageBox.Show(PianoForte.Constant.Constant.SAVE_PAYMENT_FAIL);
            }
        }

        private void TextBox_StudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidateManager.isPressNumber(e))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void TextBox_StudentId_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string text = this.TextBox_StudentId.Text;
                
                if (text != "")
                {
                    int studentId = Convert.ToInt32(text);
                    this.searchStudent(studentId);
                }                
            }
        }

        private void Button_SerachStudent_Click(object sender, EventArgs e)
        {
            string text = this.TextBox_StudentId.Text;

            if (text != "")
            {
                int studentId = Convert.ToInt32(text);
                this.searchStudent(studentId);
            }
        }

        private void TextBox_Barcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string barcode = this.TextBox_Barcode.Text;

                if (barcode != "")
                {
                    this.searchProduct(barcode);
                }
            }
        }

        private void Button_SearchBarcode_Click(object sender, EventArgs e)
        {
            string barcode = this.TextBox_Barcode.Text;

            if (barcode != "")
            {
                this.searchProduct(barcode);
            }
        }

        private void DataGridView_PaymentDetail_Summary_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            int numberOfPaymentDetailList = this.paymentDetailList.Count;
            if (numberOfPaymentDetailList > 0)
            {
                int rowIndex = e.RowIndex;
                if ((rowIndex >= 0) && (rowIndex < numberOfPaymentDetailList))
                {
                    if (e.ColumnIndex == 6)
                    {
                        PaymentDetail paymentDetail = this.paymentDetailList[rowIndex];

                        if (paymentDetail != null)
                        {
                            if (paymentDetail.Id == 0)
                            {
                                if (paymentDetail.Product.Id != this.firstRegister.Id)
                                {
                                    this.Cursor = Cursors.Hand;
                                }
                            }
                        }
                    }
                }
            }           
        }

        private void DataGridView_PaymentDetail_Summary_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void DataGridView_PaymentDetail_Summary_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int numberOfPaymentDetailList = this.paymentDetailList.Count;
            if (numberOfPaymentDetailList > 0)
            {
                int rowIndex = e.RowIndex;
                if ((rowIndex >= 0) && (rowIndex < numberOfPaymentDetailList))
                {
                    if (e.ColumnIndex == 6)
                    {
                        PaymentDetail paymentDetail = this.paymentDetailList[rowIndex];
                        if (paymentDetail != null)
                        {
                            if (paymentDetail.Id == 0)
                            {
                                int productId = paymentDetail.Product.Id;

                                if (productId != this.firstRegister.Id)
                                {
                                    if (ConfirmDialogBox.show("คุณต้องการลบรายการนี้?"))
                                    {
                                        this.removePaymentDetail(productId);
                                    }
                                }

                                this.Cursor = Cursors.Arrow;
                            }
                        }
                        
                    }
                }
            }
        }

        private void DataGridView_PaymentDetail_Summary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int numberOfPaymentDetailList = this.paymentDetailList.Count;
            if (numberOfPaymentDetailList > 0)
            {
                int rowIndex = e.RowIndex;
                if ((rowIndex >= 0) && (rowIndex < numberOfPaymentDetailList))
                {
                    PaymentDetail paymentDetail = this.paymentDetailList[rowIndex];
                    if (paymentDetail.Id == 0)
                    {
                        if (paymentDetail.Product.Type == Product.ProductType.COURSE.ToString())
                        {
                            int tempCourseId = this.enrollment.Course.Id;
                            EnrollmentPopUp enrollmentPopup = new EnrollmentPopUp();
                            Enrollment enrollment = enrollmentPopup.showFormDialog(this, this.enrollment);

                            if (enrollment != null)
                            {
                                this.removePaymentDetail(tempCourseId);
                                this.updateEnrollment(enrollment);
                            }
                        }

                        this.Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        private void DataGridView_UnpaidSavedPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            SavedPayment sp = this.unpaidSavedPaymentDictionary[rowIndex];

            if (sp != null)
            {
                this.selectedStudentSavedPaymentIndex = 0;
                this.searchStudent(sp.StudentId, sp.Id);
            }
        }

        private void RadioButton_Cash_CheckedChanged(object sender, EventArgs e)
        {
            this.updateButtonPay();

            this.TextBox_CreditCardNumber1.Text = "";
            this.TextBox_CreditCardNumber2.Text = "";
            this.TextBox_CreditCardNumber3.Text = "";
            this.TextBox_CreditCardNumber4.Text = "";

            this.TextBox_CreditCardNumber1.Enabled = false;
            this.TextBox_CreditCardNumber2.Enabled = false;
            this.TextBox_CreditCardNumber3.Enabled = false;
            this.TextBox_CreditCardNumber4.Enabled = false;
        }

        private void RadioButton_CreditCard_CheckedChanged(object sender, EventArgs e)
        {
            this.updateButtonPay();

            this.TextBox_CreditCardNumber1.Enabled = true;
            this.TextBox_CreditCardNumber2.Enabled = true;
            this.TextBox_CreditCardNumber3.Enabled = true;
            this.TextBox_CreditCardNumber4.Enabled = true;

            this.TextBox_CreditCardNumber1.Focus();
        }

        private void TextBox_CreditCardNumber1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidateManager.isPressNumber(e))
            {
                e.SuppressKeyPress = true;
            }
        }         

        private void TextBox_CreditCardNumber1_TextChanged(object sender, EventArgs e)
        {
            int textLength = this.TextBox_CreditCardNumber1.Text.Length;

            if (textLength == 4)
            {
                this.TextBox_CreditCardNumber2.Focus();
            }

            this.updateButtonPay();
        }

        private void TextBox_CreditCardNumber2_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidateManager.isPressNumber(e))
            {
                e.SuppressKeyPress = true;
            }
        }        

        private void TextBox_CreditCardNumber2_TextChanged(object sender, EventArgs e)
        {
            int textLength = this.TextBox_CreditCardNumber2.Text.Length;

            if (textLength == 4)
            {
                this.TextBox_CreditCardNumber3.Focus();
            }

            this.updateButtonPay();
        }

        private void TextBox_CreditCardNumber3_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidateManager.isPressNumber(e))
            {
                e.SuppressKeyPress = true;
            }
        }        

        private void TextBox_CreditCardNumber3_TextChanged(object sender, EventArgs e)
        {
            int textLength = this.TextBox_CreditCardNumber3.Text.Length;

            if (textLength == 4)
            {
                this.TextBox_CreditCardNumber4.Focus();
            }

            this.updateButtonPay();
        }

        private void TextBox_CreditCardNumber4_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidateManager.isPressNumber(e))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void TextBox_CreditCardNumber4_TextChanged(object sender, EventArgs e)
        {
            int textLength = this.TextBox_CreditCardNumber4.Text.Length;

            if (textLength == 4)
            {
                this.Button_Pay.Focus();
            }

            this.updateButtonPay();
        }

        private void Button_Pay_Click(object sender, EventArgs e)
        {
            double grandTotalPrice = getGrandTotalPrice();

            if (this.RadioButton_CreditCard.Checked)
            {
                string creditCardNumber = this.getCreditCardNumber();

                if (ValidateManager.validateCreditCardNumber(creditCardNumber))
                {
                    this.processPayment(creditCardNumber, grandTotalPrice);
                }
                else
                {
                    MessageBox.Show(Constant.Constant.INVALID_CREDITCARD_NUMBER);
                }
            }
            else
            {
                string temp = InputDialogBox.show("Cash");
                if (ValidateManager.isNumber(temp))
                {                    
                    double receiveMoney = Convert.ToDouble(temp);
                    if (receiveMoney >= grandTotalPrice)
                    {
                        double change = receiveMoney - grandTotalPrice;
                        MessageBox.Show("Change is " + change.ToString());
                        this.processPayment("", grandTotalPrice);
                    }
                    else
                    {
                        MessageBox.Show("จำนวนเงินไม่ครบ");
                    }
                }
                else
                {
                    if (temp != "")
                    {
                        MessageBox.Show("กรุณากรอกจำนวนเงินให้ถูกต้อง");
                    }
                }
            }
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            this.reset(true);
            //this.updateDataGridViewPaymentDetailSummary();
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            this.savePayment();
        }

        private void Button_SelectCourse_Click(object sender, EventArgs e)
        {
            Enrollment enrollment = null;

            EnrollmentPopUp enrollmentPopUp = new EnrollmentPopUp();
            enrollment = enrollmentPopUp.showFormDialog(this);

            if (enrollment != null)
            {
                this.updateEnrollment(enrollment);
                //ClassroomDateViewer classroomDateViewer = new ClassroomDateViewer();
                //enrollment = classroomDateViewer.showFormDialog(this, enrollment);

                //if (enrollment != null)
                //{
                //    this.updateEnrollment(enrollment);
                //}
            }
        }

        private void Button_SelectBook_Click(object sender, EventArgs e)
        {
            SearchBookPopUp searchBookPopUp = new SearchBookPopUp();
            searchBookPopUp.showFormDialog(this);
        }

        private void Button_SelectCD_Click(object sender, EventArgs e)
        {
            SearchCDPopUp searchCDPopUp = new SearchCDPopUp();
            searchCDPopUp.showFormDialog(this);
        }

        private void Button_AddOther_Click(object sender, EventArgs e)
        {
            AddOtherProductPopUp addOtherProductPopUp = new AddOtherProductPopUp();

            OtherCost otherProduct = addOtherProductPopUp.showFormDialog();
            if (otherProduct != null)
            {
                Product product = new Product();
                product.Id = otherProduct.Id;
                product.Type = Product.ProductType.OTHER.ToString();
                product.Name = otherProduct.Name;
                product.Price = otherProduct.Price;

                PaymentDetail paymentDetail = new PaymentDetail();
                paymentDetail.Product = product;
                paymentDetail.Quantity = 1;

                this.addPaymentDetail(paymentDetail);
            }
        }

        private void CheckBox_AddFirstRegisterCost_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.CheckBox_AddFirstRegisterCost.Checked)
            //{
            //    if (this.firstRegister == null)
            //    {
            //        this.firstRegister = OtherCostManager.findOtherCost(4000001);
            //    }

            //    if (this.firstRegister != null)
            //    {
            //        Product product = new Product();
            //        product.Id = this.firstRegister.Id;
            //        product.Type = Product.ProductType.OTHER.ToString();
            //        product.Name = this.firstRegister.Name;
            //        product.Price = this.firstRegister.Price;

            //        PaymentDetail paymentDetail = new PaymentDetail();
            //        paymentDetail.Product = product;
            //        paymentDetail.Quantity = 1;

            //        this.addPaymentDetail(paymentDetail);
            //    }
            //}
            //else
            //{
            //    this.removePaymentDetail(this.firstRegister.Id);
            //}
        }

        private void ComboBox_Unpaid_Payment_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedStudentSavedPaymentIndex = this.ComboBox_Unpaid_Payment.SelectedIndex;

            if (this.paymentDetailList.Count > 0)
            {
                this.clearPaymentDetail();
                this.TextBox_StudentId.Focus();
            }

            if (this.selectedStudentSavedPaymentIndex > 0)
            {
                this.applyUnpaidPaymentView(true);

                SavedPayment savedPayment = this.studentSavedPaymentList[this.selectedStudentSavedPaymentIndex - 1];

                if (savedPayment != null)
                {
                    List<PaymentDetail> tempPaymentDetailList = PaymentDetailManager.findAllPaymentDetailBySavedPaymentId(savedPayment.Id);

                    foreach (PaymentDetail paymentDetail in tempPaymentDetailList)
                    {
                        this.addPaymentDetail(paymentDetail);
                    }
                }
            }
            else
            {
                this.applyUnpaidPaymentView(false);
            }
        }

        private void applyUnpaidPaymentView(bool isUnpaidPaymentView)
        {
            //this.TextBox_Barcode.Enabled = !isUnpaidPaymentView;
            //this.Button_SearchBarcode.Enabled = !isUnpaidPaymentView;

            //this.Button_SelectCourse.Enabled = !isUnpaidPaymentView;
            //this.Button_SelectBook.Enabled = !isUnpaidPaymentView;
            //this.Button_SelectCD.Enabled = !isUnpaidPaymentView;
            //this.Button_AddOther.Enabled = !isUnpaidPaymentView;

            //this.CheckBox_AddFirstRegisterCost.Checked = false;
            //this.CheckBox_AddFirstRegisterCost.Enabled = !isUnpaidPaymentView;

            this.RadioButton_Cash.Checked = true;

            if (isUnpaidPaymentView == true)
            {
                this.RadioButton_Cash.Focus();
                this.Button_Cancel_UnpaidPayment.Enabled = true;
            }
            else
            {
                this.TextBox_Barcode.Focus();
                this.Button_Cancel_UnpaidPayment.Enabled = false;
            }
        }

        private void Button_ReloadUnpaidPaymentList_Click(object sender, EventArgs e)
        {
            this.updateUnpaidSavedPaymentDataGridView();
        }

        private void Button_Cancel_UnpaidPayment_Click(object sender, EventArgs e)
        {
            if (this.selectedStudentSavedPaymentIndex > 0)
            {
                SavedPayment savedPayment = this.studentSavedPaymentList[this.selectedStudentSavedPaymentIndex - 1];

                if (savedPayment != null)
                {
                    savedPayment.Status = SavedPayment.SavedPaymentStatus.CANCELED.ToString();

                    if (SavedPaymentManager.updateSavedPayment(savedPayment) == true)
                    {
                        MessageBox.Show(PianoForte.Constant.Constant.CANCEL_PAYMENT_SUCCESS);
                        this.reset(true);
                    }
                    else
                    {
                        MessageBox.Show(PianoForte.Constant.Constant.CANCEL_PAYMENT_FAIL);
                    }
                }
            }
        }
    }
}
