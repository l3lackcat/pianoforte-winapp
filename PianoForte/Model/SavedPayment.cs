using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    public class SavedPayment
    {
        public static string tableName = "saved_payment";
        public static string columnSavedPaymentId = "savedPaymentId";
        public static string columnStudentId = "studentId";
        public static string columnReceiverId = "receiverId";
        public static string columnTotalPrice = "totalPrice";
        public static string columnSavedDate = "savedDate";
        public static string columnPaymentId = "paymentId";
        public static string columnStatus = "status";

        public enum SavedPaymentStatus
        {
            ALL,
            PAID,
            CANCELED,
            NOT_PAID
        }

        protected int id;
        protected int studentId;
        protected int receiverId;
        protected double totalPrice;
        protected DateTime savedDate;
        protected int paymentId;
        protected string status;

        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        public int StudentId
        {
            get
            {
                return this.studentId;
            }

            set
            {
                this.studentId = value;
            }
        }

        public int ReceiverId
        {
            get
            {
                return this.receiverId;
            }

            set
            {
                this.receiverId = value;
            }
        }

        public double TotalPrice
        {
            get
            {
                return this.totalPrice;
            }

            set
            {
                this.totalPrice = value;
            }
        }

        public DateTime SavedDate
        {
            get
            {
                return this.savedDate;
            }

            set
            {
                this.savedDate = value;
            }
        }

        public int PaymentId
        {
            get
            {
                return this.paymentId;
            }

            set
            {
                this.paymentId = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
            }
        } 
    }
}
