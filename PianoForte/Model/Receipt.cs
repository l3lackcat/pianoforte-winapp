using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    public class Receipt
    {
        public static string tableName = "receipt";
        public static string columnReceiptId = "receiptId";
        public static string columnPrintReceiptId = "printReceiptId";
        public static string columnFirstReceiptId = "firstReceiptId";
        public static string columnTransactionId = "transactionId";
        public static string columnStudentId = "studentId";
        public static string columnReceiverId = "receiverId";
        public static string columnReceiptDate = "receiptDate";
        public static string columnCreditCardNumber = "creditCardNumber";
        public static string columnAmount = "amount";
        public static string columnStatus = "status";

        public enum ReceiptStatus
        {
            PAID,
            UNPAID,
            CANCELED
        }

        public int Id { get; set; }
        public int PrintReceiptId { get; set; }
        public int FirstReceiptId { get; set; }
        public int TransactionId { get; set; }
        public int StudentId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public string CreditCardNumber { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}
