using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    public class Transaction
    {
        public static string tableName = "transaction";
        public static string columnTransactionId = "transactionId";
        public static string columnTransactionDate = "transactionDate";
        public static string columnTransactionAmount = "transactionAmount";
        public static string columnStudentId = "studentId";
        public static string columnReceiverId = "receiverId";
        public static string columnStatus = "status";

        public enum TransactionStatus
        {
            PAID,
            UNPAID,
            CANCELED
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int StudentId { get; set; }
        public int ReceiverId { get; set; }
        public string Status { get; set; }
    }
}
