﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    public class PaymentDetail
    {
        public static string tableName = "payment_detail";
        public static string columnPaymentDetailId = "paymentDetailId";
        public static string columnPaymentId = "paymentId";
        public static string columnSavedPaymentId = "savedPaymentId";
        public static string columnProductId = "productId";
        public static string columnProductType = "productType";
        public static string columnProductName = "productName"; 
        public static string columnAmount = "amount";
        public static string columnDiscount = "discount";
        public static string columnProductPrice = "productPrice";

        private int id;
        private int paymentId;
        private int savedPaymentId;
        private Product product;
        private int amount;
        private double discount;

        public PaymentDetail()
        {
            this.product = new Product();
        }

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

        public int SavedPaymentId
        {
            get
            {
                return this.savedPaymentId;
            }

            set
            {
                this.savedPaymentId = value;
            }
        }

        public Product Product
        {
            get
            {
                return this.product;
            }

            set
            {
                this.product = value;
            }
        }

        public int Quantity
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public double Discount
        {
            get
            {
                return this.discount;
            }

            set
            {
                this.discount = value;
            }
        }
    }
}
