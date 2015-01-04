using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;

using PianoForte.Dao;
using PianoForte.Manager;
using PianoForte.Model;

namespace PianoForte.Dao.MySql
{
    public class ReceiptDaoImplMySql : ReceiptDao
    {
        //--------------------------------------------------------------------------------

        public bool insertReceipt(Receipt receipt)
        {
            string sql = "INSERT INTO " +
                         Receipt.tableName + " (" +
                         Receipt.columnFirstReceiptId + ", " +
                         Receipt.columnTransactionId + ", " +
                         Receipt.columnStudentId + ", " +
                         Receipt.columnReceiverId + ", " +
                         Receipt.columnReceiptDate + ", " +
                         Receipt.columnCreditCardNumber + ", " +
                         Receipt.columnAmount + ", " +
                         Receipt.columnStatus + ") " +
                         "VALUES(" +
                         "?" + Receipt.columnFirstReceiptId + ", " +
                         "?" + Receipt.columnTransactionId + ", " +
                         "?" + Receipt.columnStudentId + ", " +
                         "?" + Receipt.columnReceiverId + ", " +
                         "?" + Receipt.columnReceiptDate + ", " +
                         "?" + Receipt.columnCreditCardNumber + ", " +
                         "?" + Receipt.columnAmount + ", " +
                         "?" + Receipt.columnStatus + ")";

            return this.processInsertCommand(sql, receipt);
        }

        public bool updateReceipt(Receipt receipt)
        {
            string sql = "UPDATE " +
                         Receipt.tableName + " SET " +
                         Receipt.columnFirstReceiptId + " = ?" + Receipt.columnFirstReceiptId + ", " +
                         Receipt.columnTransactionId + " = ?" + Receipt.columnTransactionId + ", " +
                         Receipt.columnStudentId + " = ?" + Receipt.columnStudentId + ", " +
                         Receipt.columnReceiverId + " = ?" + Receipt.columnReceiverId + ", " +
                         Receipt.columnReceiptDate + " = ?" + Receipt.columnReceiptDate + ", " +
                         Receipt.columnCreditCardNumber + " = ?" + Receipt.columnCreditCardNumber + ", " +
                         Receipt.columnAmount + " = ?" + Receipt.columnAmount + ", " +
                         Receipt.columnStatus + " = ?" + Receipt.columnStatus + " " +
                         "WHERE " + Receipt.columnReceiptId + " = ?" + Receipt.columnReceiptId;

            return this.processUpdateCommand(sql, receipt);
        }

        //--------------------------------------------------------------------------------

        public Receipt findReceipt(int receiptId)
        {
            Receipt receipt = null;

            string sql = "SELECT * " +
                         "FROM " + Receipt.tableName + " " +
                         "WHERE " + Receipt.columnReceiptId + " = " + receiptId;

            List<Receipt> receiptList = this.processSelectCommand(sql);
            if (receiptList.Count > 0)
            {
                receipt = receiptList[0];
            }

            return receipt;
        }

        public Receipt findLastReceipt()
        {
            Receipt receipt = null;

            string sql = "SELECT * " +
                         "FROM " + Receipt.tableName + " " +
                         "ORDER BY " + Receipt.columnReceiptId + " DESC " +
                         "LIMIT 1";

            List<Receipt> receiptList = this.processSelectCommand(sql);
            if (receiptList.Count > 0)
            {
                receipt = receiptList[0];
            }

            return receipt;
        }

        //--------------------------------------------------------------------------------

        private bool processInsertCommand(string sql, Receipt receipt)
        {
            bool returnFlag = false;

            if (receipt != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(Receipt.columnFirstReceiptId, receipt.FirstReceiptId);
                        command.Parameters.AddWithValue(Receipt.columnTransactionId, receipt.TransactionId);
                        command.Parameters.AddWithValue(Receipt.columnStudentId, receipt.StudentId);
                        command.Parameters.AddWithValue(Receipt.columnReceiverId, receipt.ReceiverId);
                        command.Parameters.AddWithValue(Receipt.columnReceiptDate, receipt.Date);
                        command.Parameters.AddWithValue(Receipt.columnCreditCardNumber, receipt.CreditCardNumber);
                        command.Parameters.AddWithValue(Receipt.columnAmount, receipt.Amount);
                        command.Parameters.AddWithValue(Receipt.columnStatus, receipt.Status);

                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow != -1)
                        {
                            returnFlag = true;
                        }
                    }
                }
                catch (System.InvalidOperationException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (MySqlException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (System.SystemException e)
                {
                    LogManager.writeLog(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return returnFlag;
        }

        private bool processUpdateCommand(string sql, Receipt receipt)
        {
            bool returnFlag = false;

            if (receipt != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(Receipt.columnFirstReceiptId, receipt.FirstReceiptId);
                        command.Parameters.AddWithValue(Receipt.columnTransactionId, receipt.TransactionId);
                        command.Parameters.AddWithValue(Receipt.columnReceiverId, receipt.ReceiverId);
                        command.Parameters.AddWithValue(Receipt.columnStudentId, receipt.StudentId);
                        command.Parameters.AddWithValue(Receipt.columnReceiptDate, receipt.Date);
                        command.Parameters.AddWithValue(Receipt.columnCreditCardNumber, receipt.CreditCardNumber);
                        command.Parameters.AddWithValue(Receipt.columnAmount, receipt.Amount);
                        command.Parameters.AddWithValue(Receipt.columnStatus, receipt.Status);
                        command.Parameters.AddWithValue(Receipt.columnReceiptId, receipt.Id);

                        int affectedRow = command.ExecuteNonQuery();
                        if (affectedRow != -1)
                        {
                            returnFlag = true;
                        }
                    }
                }
                catch (System.InvalidOperationException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (MySqlException e)
                {
                    LogManager.writeLog(e.Message);
                }
                catch (System.SystemException e)
                {
                    LogManager.writeLog(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return returnFlag;
        }

        private List<Receipt> processSelectCommand(string sql)
        {
            List<Receipt> receiptList = new List<Receipt>();

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                if (conn != null)
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                    DataSet data = new DataSet();
                    dataAdapter.Fill(data, Receipt.tableName);

                    for (int i = 0; i < data.Tables[Receipt.tableName].Rows.Count; i++)
                    {
                        receiptList.Add(generateReceipt(data, i));
                    }
                }
            }
            catch (System.InvalidOperationException e)
            {
                LogManager.writeLog(e.Message);
            }
            catch (MySqlException e)
            {
                LogManager.writeLog(e.Message);
            }
            catch (System.SystemException e)
            {
                LogManager.writeLog(e.Message);
            }
            finally
            {
                conn.Close();
            }

            return receiptList;
        }

        private Receipt generateReceipt(DataSet data, int index)
        {
            Receipt receipt = new Receipt();

            receipt.Id = Convert.ToInt32(data.Tables[Receipt.tableName].Rows[index][Receipt.columnReceiptId].ToString());
            receipt.FirstReceiptId = Convert.ToInt32(data.Tables[Receipt.tableName].Rows[index][Receipt.columnFirstReceiptId].ToString());
            receipt.TransactionId = Convert.ToInt32(data.Tables[Receipt.tableName].Rows[index][Receipt.columnTransactionId].ToString());
            receipt.StudentId = Convert.ToInt32(data.Tables[Receipt.tableName].Rows[index][Receipt.columnStudentId].ToString());
            receipt.ReceiverId = Convert.ToInt32(data.Tables[Receipt.tableName].Rows[index][Receipt.columnReceiverId].ToString());
            receipt.Date = (DateTime)data.Tables[Receipt.tableName].Rows[index][Receipt.columnReceiptDate];
            receipt.CreditCardNumber = data.Tables[Receipt.tableName].Rows[index][Receipt.columnCreditCardNumber].ToString();
            receipt.Amount = Convert.ToDouble(data.Tables[Receipt.tableName].Rows[index][Receipt.columnAmount].ToString());
            receipt.Status = data.Tables[Receipt.tableName].Rows[index][Receipt.columnStatus].ToString();

            return receipt;
        }

        //--------------------------------------------------------------------------------
    }
}
