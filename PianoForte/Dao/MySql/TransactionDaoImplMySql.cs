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
    public class TransactionDaoImplMySql : TransactionDao
    {
        //--------------------------------------------------------------------------------

        public bool insertTransaction(Transaction transaction)
        {
            string sql = "INSERT INTO " +
                         Transaction.tableName + " (" +
                         Transaction.columnTransactionDate + ", " +
                         Transaction.columnTransactionAmount + ", " +
                         Transaction.columnStudentId + ", " +
                         Transaction.columnReceiverId + ", " +
                         Transaction.columnStatus + ") " +
                         "VALUES(" +
                         "?" + Transaction.columnTransactionDate + ", " +
                         "?" + Transaction.columnTransactionAmount + ", " +
                         "?" + Transaction.columnStudentId + ", " +
                         "?" + Transaction.columnReceiverId + ", " +
                         "?" + Transaction.columnStatus + ")";

            return this.processInsertCommand(sql, transaction);
        }

        public bool updateTransaction(Transaction transaction)
        {
            string sql = "UPDATE " +
                         Transaction.tableName + " SET " +
                         Transaction.columnTransactionDate + " = ?" + Transaction.columnTransactionDate + ", " +
                         Transaction.columnTransactionAmount + " = ?" + Transaction.columnTransactionAmount + ", " +
                         Transaction.columnStudentId + " = ?" + Transaction.columnStudentId + ", " +
                         Transaction.columnReceiverId + " = ?" + Transaction.columnReceiverId + ", " +
                         Transaction.columnStatus + " = ?" + Transaction.columnStatus + " " +
                         "WHERE " + Transaction.columnTransactionId + " = ?" + Transaction.columnTransactionId;

            return this.processUpdateCommand(sql, transaction);
        }

        //--------------------------------------------------------------------------------

        public Transaction findTransaction(int transactionId)
        {
            Transaction transaction = null;

            string sql = "SELECT * " +
                         "FROM " + Transaction.tableName + " " +
                         "WHERE " + Transaction.columnTransactionId + " = " + transactionId;

            List<Transaction> transactionList = this.processSelectCommand(sql);
            if (transactionList.Count > 0)
            {
                transaction = transactionList[0];
            }

            return transaction;
        }

        public Transaction findLastTransaction()
        {
            Transaction transaction = null;

            string sql = "SELECT * " +
                         "FROM " + Transaction.tableName + " " +
                         "ORDER BY " + Transaction.columnTransactionId + " DESC " +
                         "LIMIT 1";

            List<Transaction> transactionList = this.processSelectCommand(sql);
            if (transactionList.Count > 0)
            {
                transaction = transactionList[0];
            }

            return transaction;
        }

        //--------------------------------------------------------------------------------

        private bool processInsertCommand(string sql, Transaction transaction)
        {
            bool returnFlag = false;

            if (transaction != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(Transaction.columnTransactionDate, transaction.Date);
                        command.Parameters.AddWithValue(Transaction.columnTransactionAmount, transaction.Amount);
                        command.Parameters.AddWithValue(Transaction.columnStudentId, transaction.StudentId);
                        command.Parameters.AddWithValue(Transaction.columnReceiverId, transaction.ReceiverId);
                        command.Parameters.AddWithValue(Transaction.columnStatus, transaction.Status);

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

        private bool processUpdateCommand(string sql, Transaction transaction)
        {
            bool returnFlag = false;

            if (transaction != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(Transaction.columnTransactionDate, transaction.Date);
                        command.Parameters.AddWithValue(Transaction.columnTransactionAmount, transaction.Amount);
                        command.Parameters.AddWithValue(Transaction.columnStudentId, transaction.StudentId);
                        command.Parameters.AddWithValue(Transaction.columnReceiverId, transaction.ReceiverId);
                        command.Parameters.AddWithValue(Transaction.columnStatus, transaction.Status);
                        command.Parameters.AddWithValue(Transaction.columnTransactionId, transaction.Id);                        

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

        private List<Transaction> processSelectCommand(string sql)
        {
            List<Transaction> transactionList = new List<Transaction>();

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
                    dataAdapter.Fill(data, Transaction.tableName);

                    for (int i = 0; i < data.Tables[Transaction.tableName].Rows.Count; i++)
                    {
                        transactionList.Add(generateTransaction(data, i));
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

            return transactionList;
        }

        private Transaction generateTransaction(DataSet data, int index)
        {
            Transaction transaction = new Transaction();

            transaction.Id = Convert.ToInt32(data.Tables[Transaction.tableName].Rows[index][Transaction.columnTransactionId].ToString());
            transaction.Date = (DateTime)data.Tables[Transaction.tableName].Rows[index][Transaction.columnTransactionDate];
            transaction.Amount = Convert.ToDouble(data.Tables[Transaction.tableName].Rows[index][Transaction.columnTransactionAmount].ToString());
            transaction.StudentId = Convert.ToInt32(data.Tables[Transaction.tableName].Rows[index][Transaction.columnStudentId].ToString());
            transaction.ReceiverId = Convert.ToInt32(data.Tables[Transaction.tableName].Rows[index][Transaction.columnReceiverId].ToString());
            transaction.Status = data.Tables[Transaction.tableName].Rows[index][Transaction.columnStatus].ToString();

            return transaction;
        }

        //--------------------------------------------------------------------------------
    }
}
