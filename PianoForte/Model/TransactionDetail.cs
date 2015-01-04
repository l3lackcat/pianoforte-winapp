using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    public class TransactionDetail
    {
        public static string tableName = "transaction_detail";
        public static string columnTransactionDetailId = "transactionDetailId";
        public static string columnTransactionId = "transactionId";
        public static string columnProductId = "productId";
        public static string columnProductType = "productType";
        public static string columnProductName = "productName";
        public static string columnProductPrice = "productPrice";
        public static string columnQuantity = "quantity";

        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public TransactionDetail()
        {
            this.Product = new Product();
        }
    }
}
