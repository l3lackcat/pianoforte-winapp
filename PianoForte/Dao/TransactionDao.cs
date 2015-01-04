using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Model;

namespace PianoForte.Dao
{
    public interface TransactionDao
    {
        bool insertTransaction(Transaction transaction);
        bool updateTransaction(Transaction transaction);
        //bool deleteTransaction(int transactionId);

        Transaction findTransaction(int transactionId);
        Transaction findLastTransaction();

        //List<Transaction> findAllTransaction();
        //List<Transaction> findAllTransaction(int startIndex, int offset);
        //List<Transaction> findAllTransaction(Transaction.TransactionStatus status);
        //List<Transaction> findAllTransaction(Transaction.TransactionStatus status, int startIndex, int offset);

        //List<Transaction> findAllTransactionByDate(DateTime startDate, DateTime endDate);
        //List<Transaction> findAllTransactionByDate(DateTime startDate, DateTime endDate, int startIndex, int offset);
        //List<Transaction> findAllTransactionByDate(DateTime startDate, DateTime endDate, Transaction.TransactionStatus status);
        //List<Transaction> findAllTransactionByDate(DateTime startDate, DateTime endDate, Transaction.TransactionStatus status, int startIndex, int offset);

        //List<Transaction> findAllTransactionByStudentId(int studentId);
        //List<Transaction> findAllTransactionByStudentId(int studentId, int startIndex, int offset);
        //List<Transaction> findAllTransactionByStudentId(int studentId, Transaction.TransactionStatus status);
        //List<Transaction> findAllTransactionByStudentId(int studentId, Transaction.TransactionStatus status, int startIndex, int offset);

        //List<Transaction> findAllTransactionByStudentIdAndDate(int studentId, DateTime startDate, DateTime endDate);
        //List<Transaction> findAllTransactionByStudentIdAndDate(int studentId, DateTime startDate, DateTime endDate, int startIndex, int offset);
        //List<Transaction> findAllTransactionByStudentIdAndDate(int studentId, DateTime startDate, DateTime endDate, Transaction.TransactionStatus status);
        //List<Transaction> findAllTransactionByStudentIdAndDate(int studentId, DateTime startDate, DateTime endDate, Transaction.TransactionStatus status, int startIndex, int offset);
    }
}
