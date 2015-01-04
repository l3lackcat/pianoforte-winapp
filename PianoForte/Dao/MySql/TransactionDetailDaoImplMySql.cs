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
    public class TransactionDetailDaoImplMySql : TransactionDetailDao
    {
        //--------------------------------------------------------------------------------

        public bool insertTransactionDetail(TransactionDetail transactionDetail)
        {
            string sql = "INSERT INTO " +
                         TransactionDetail.tableName + " (" +
                         TransactionDetail.columnTransactionId + ", " +
                         TransactionDetail.columnProductId + ", " +
                         TransactionDetail.columnProductType + ", " +
                         TransactionDetail.columnProductName + ", " +
                         TransactionDetail.columnProductPrice + ", " +
                         TransactionDetail.columnQuantity + ") " +
                         "VALUES(" +
                         "?" + TransactionDetail.columnTransactionId + ", " +
                         "?" + TransactionDetail.columnProductId + ", " +
                         "?" + TransactionDetail.columnProductType + ", " +
                         "?" + TransactionDetail.columnProductName + ", " +
                         "?" + TransactionDetail.columnProductPrice + ", " +
                         "?" + TransactionDetail.columnQuantity + ")";

            return this.processInsertCommand(sql, transactionDetail);
        }

        public bool updateTransactionDetail(TransactionDetail transactionDetail)
        {
            string sql = "UPDATE " +
                         TransactionDetail.tableName + " SET " +
                         TransactionDetail.columnTransactionId + " = ?" + TransactionDetail.columnTransactionId + ", " +
                         TransactionDetail.columnProductId + " = ?" + TransactionDetail.columnProductId + ", " +
                         TransactionDetail.columnProductType + " = ?" + TransactionDetail.columnProductType + ", " +
                         TransactionDetail.columnProductName + " = ?" + TransactionDetail.columnProductName + ", " +
                         TransactionDetail.columnProductPrice + " = ?" + TransactionDetail.columnProductPrice + ", " +
                         TransactionDetail.columnQuantity + " = ?" + TransactionDetail.columnQuantity + " " +
                         "WHERE " + TransactionDetail.columnTransactionDetailId + " = ?" + TransactionDetail.columnTransactionDetailId;

            return this.processUpdateCommand(sql, transactionDetail);
        }

        //--------------------------------------------------------------------------------

        public TransactionDetail findTransactionDetail(int transactionDetailId)
        {
            TransactionDetail transactionDetail = null;

            string sql = "SELECT * " +
                         "FROM " + TransactionDetail.tableName + " " +
                         "WHERE " + TransactionDetail.columnTransactionDetailId + " = " + transactionDetailId;

            List<TransactionDetail> transactionDetailList = this.processSelectCommand(sql);
            if (transactionDetailList.Count > 0)
            {
                transactionDetail = transactionDetailList[0];
            }

            return transactionDetail;
        }

        public TransactionDetail findLastTransactionDetail()
        {
            TransactionDetail transactionDetail = null;

            string sql = "SELECT * " +
                         "FROM " + TransactionDetail.tableName + " " +
                         "ORDER BY " + TransactionDetail.columnTransactionDetailId + " DESC " +
                         "LIMIT 1";

            List<TransactionDetail> transactionDetailList = this.processSelectCommand(sql);
            if (transactionDetailList.Count > 0)
            {
                transactionDetail = transactionDetailList[0];
            }

            return transactionDetail;
        }

        //--------------------------------------------------------------------------------

        private bool processInsertCommand(string sql, TransactionDetail transactionDetail)
        {
            bool returnFlag = false;

            if (transactionDetail != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(TransactionDetail.columnTransactionId, transactionDetail.TransactionId);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductId, transactionDetail.Product.Id);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductType, transactionDetail.Product.Type);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductName, transactionDetail.Product.Name);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductPrice, transactionDetail.Product.Price);
                        command.Parameters.AddWithValue(TransactionDetail.columnQuantity, transactionDetail.Quantity);

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

        private bool processUpdateCommand(string sql, TransactionDetail transactionDetail)
        {
            bool returnFlag = false;

            if (transactionDetail != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(TransactionDetail.columnTransactionId, transactionDetail.TransactionId);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductId, transactionDetail.Product.Id);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductType, transactionDetail.Product.Type);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductName, transactionDetail.Product.Name);
                        command.Parameters.AddWithValue(TransactionDetail.columnProductPrice, transactionDetail.Product.Price);
                        command.Parameters.AddWithValue(TransactionDetail.columnQuantity, transactionDetail.Quantity);
                        command.Parameters.AddWithValue(TransactionDetail.columnTransactionDetailId, transactionDetail.Id);

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

        private List<TransactionDetail> processSelectCommand(string sql)
        {
            List<TransactionDetail> transactionList = new List<TransactionDetail>();

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
                    dataAdapter.Fill(data, TransactionDetail.tableName);

                    for (int i = 0; i < data.Tables[TransactionDetail.tableName].Rows.Count; i++)
                    {
                        transactionList.Add(generateTransactionDetail(data, i));
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

        private TransactionDetail generateTransactionDetail(DataSet data, int index)
        {
            TransactionDetail transactionDetail = new TransactionDetail();

            transactionDetail.Id = Convert.ToInt32(data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnTransactionDetailId].ToString());
            transactionDetail.TransactionId = Convert.ToInt32(data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnTransactionId].ToString());
            transactionDetail.Product.Id = Convert.ToInt32(data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnProductId].ToString());
            transactionDetail.Product.Type = data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnProductType].ToString();
            transactionDetail.Product.Name = data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnProductName].ToString();
            transactionDetail.Product.Price = Convert.ToDouble(data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnProductPrice].ToString());
            transactionDetail.Quantity = Convert.ToInt32(data.Tables[TransactionDetail.tableName].Rows[index][TransactionDetail.columnQuantity].ToString());

            return transactionDetail;
        }

        //--------------------------------------------------------------------------------
    }
}
