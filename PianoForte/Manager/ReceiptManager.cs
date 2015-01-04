using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Dao;
using PianoForte.Model;

namespace PianoForte.Manager
{
    public class ReceiptManager
    {
        private static ReceiptDao receiptDao = DaoFactory.getDaoFactory(DaoFactory.FactoryType.MYSQL).getReceiptDao();

        public static Receipt insertReceipt(Receipt receipt)
        {
            bool isInsertComplete = receiptDao.insertReceipt(receipt);

            if (isInsertComplete)
            {
                Receipt lastReceipt = receiptDao.findLastReceipt();

                if (lastReceipt != null)
                {
                    receipt.Id = lastReceipt.Id;
                }
                else
                {
                    receipt = null;
                }
            }
            else
            {
                receipt = null;
            }

            return receipt;
        }

        public static bool updateReceipt(Receipt receipt)
        {
            return receiptDao.updateReceipt(receipt);
        }

        public static Receipt findReceipt(int receiptId)
        {
            return receiptDao.findReceipt(receiptId);
        }

        public static Receipt findLastReceipt()
        {
            return receiptDao.findLastReceipt();
        }
    }
}
