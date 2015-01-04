﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Model;

namespace PianoForte.Dao
{
    public interface ReceiptDao
    {
        bool insertReceipt(Receipt receipt);
        bool updateReceipt(Receipt receipt);

        Receipt findReceipt(int receiptId);
        Receipt findLastReceipt();
    }
}
