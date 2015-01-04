using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Model;

namespace PianoForte.Dao
{
    public interface TransactionDetailDao
    {
        bool insertTransactionDetail(TransactionDetail transactionDetail);
        bool updateTransactionDetail(TransactionDetail transactionDetail);

        TransactionDetail findTransactionDetail(int transactionDetailId);
        TransactionDetail findLastTransactionDetail();
    }
}
