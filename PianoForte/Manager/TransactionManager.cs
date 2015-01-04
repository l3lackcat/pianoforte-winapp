using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Dao;
using PianoForte.Model;

namespace PianoForte.Manager
{
    public class TransactionManager
    {
        private static TransactionDao transactionDao = DaoFactory.getDaoFactory(DaoFactory.FactoryType.MYSQL).getTransactionDao();

        public static Transaction insertTransaction(Transaction transaction)
        {
            bool isInsertComplete = transactionDao.insertTransaction(transaction);

            if (isInsertComplete)
            {
                Transaction lastTransaction = transactionDao.findLastTransaction();

                if (lastTransaction != null)
                {
                    transaction.Id = lastTransaction.Id;
                }
                else
                {
                    transaction = null;
                }
            }
            else
            {
                transaction = null;
            }

            return transaction;
        }

        public static bool updateTransaction(Transaction transaction)
        {
            return transactionDao.updateTransaction(transaction);
        }

        public static Transaction findTransaction(int transactionId)
        {
            return transactionDao.findTransaction(transactionId);
        }

        public static Transaction findLastTransaction()
        {
            return transactionDao.findLastTransaction();
        }
    }
}
