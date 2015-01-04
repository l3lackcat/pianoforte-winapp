using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Dao;
using PianoForte.Model;

namespace PianoForte.Manager
{
    public class TransactionDetailManager
    {
        private static TransactionDetailDao transactionDetailDao = DaoFactory.getDaoFactory(DaoFactory.FactoryType.MYSQL).getTransactionDetailDao();

        public static TransactionDetail insertTransactionDetail(TransactionDetail transactionDetail)
        {
            bool isInsertComplete = transactionDetailDao.insertTransactionDetail(transactionDetail);

            if (isInsertComplete)
            {
                TransactionDetail lastTransactionDetail = transactionDetailDao.findLastTransactionDetail();

                if (lastTransactionDetail != null)
                {
                    transactionDetail.Id = lastTransactionDetail.Id;
                }
                else
                {
                    transactionDetail = null;
                }
            }
            else
            {
                transactionDetail = null;
            }

            return transactionDetail;
        }

        public static bool updateTransactionDetail(TransactionDetail transactionDetail)
        {
            return transactionDetailDao.updateTransactionDetail(transactionDetail);
        }

        public static TransactionDetail findTransactionDetail(int transactionDetailId)
        {
            return transactionDetailDao.findTransactionDetail(transactionDetailId);
        }

        public static TransactionDetail findLastTransactionDetail()
        {
            return transactionDetailDao.findLastTransactionDetail();
        }
    }
}
