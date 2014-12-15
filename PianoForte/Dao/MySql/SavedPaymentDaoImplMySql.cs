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
    public class SavedPaymentDaoImplMySql : SavedPaymentDao
    {
        public bool insertSavedPayment(SavedPayment savedPayment)
        {
            string sql = "INSERT INTO " +
                         SavedPayment.tableName + " (" +
                         SavedPayment.columnStudentId + ", " +
                         SavedPayment.columnReceiverId + ", " +
                         SavedPayment.columnTotalPrice + ", " +
                         SavedPayment.columnSavedDate + ", " +
                         SavedPayment.columnPaymentId + ", " +
                         SavedPayment.columnStatus + ") " +
                         "VALUES(" +
                         "?" + SavedPayment.columnStudentId + ", " +
                         "?" + SavedPayment.columnReceiverId + ", " +
                         "?" + SavedPayment.columnTotalPrice + ", " +
                         "?" + SavedPayment.columnSavedDate + ", " +
                         "?" + SavedPayment.columnPaymentId + ", " +
                         "?" + SavedPayment.columnStatus + ")";

            return this.processInsertCommand(sql, savedPayment);
        }

        public bool updateSavedPayment(SavedPayment savedPayment)
        {
            string sql = "UPDATE " +
                         SavedPayment.tableName + " SET " +
                         SavedPayment.columnStudentId + " = ?" + SavedPayment.columnStudentId + ", " +
                         SavedPayment.columnReceiverId + " = ?" + SavedPayment.columnReceiverId + ", " +
                         SavedPayment.columnTotalPrice + " = ?" + SavedPayment.columnTotalPrice + ", " +
                         SavedPayment.columnSavedDate + " = ?" + SavedPayment.columnSavedDate + ", " +
                         SavedPayment.columnPaymentId + " = ?" + SavedPayment.columnPaymentId + ", " +
                         SavedPayment.columnStatus + " = ?" + SavedPayment.columnStatus + " " +
                         "WHERE " + SavedPayment.columnSavedPaymentId + " = ?" + SavedPayment.columnSavedPaymentId;

            return this.processUpdateCommand(sql, savedPayment);
        }

        public bool deleteSavedPayment(int savedPaymentId)
        {
            string sql = "DELETE " +
                         "FROM " + SavedPayment.tableName + " " +
                         "WHERE " + SavedPayment.columnSavedPaymentId + " = " + savedPaymentId;

            return this.processDeleteCommand(sql);
        }

        //--------------------------------------------------------------------------------

        public SavedPayment findSavedPayment(int savedPaymentId)
        {
            SavedPayment savedPayment = null;

            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "WHERE " + SavedPayment.columnSavedPaymentId + " = " + savedPaymentId;

            List<SavedPayment> savedPaymentList = this.processSelectCommand(sql);
            if (savedPaymentList.Count > 0)
            {
                savedPayment = savedPaymentList[0];
            }

            return savedPayment;
        }

        public SavedPayment findLastSavedPayment()
        {
            SavedPayment savedPayment = null;

            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "ORDER BY " + SavedPayment.columnSavedPaymentId + " DESC " +
                         "LIMIT 1";

            List<SavedPayment> savedPaymentList = this.processSelectCommand(sql);
            if (savedPaymentList.Count > 0)
            {
                savedPayment = savedPaymentList[0];
            }

            return savedPayment;
        }        

        public List<SavedPayment> findAllSavedPayment()
        {
            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "ORDER BY " + SavedPayment.columnSavedPaymentId + " ASC";

            return this.processSelectCommand(sql);
        }

        public List<SavedPayment> findAllSavedPayment(SavedPayment.SavedPaymentStatus status)
        {
            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "WHERE " + SavedPayment.columnStatus + " = '" + status.ToString() + "' " +
                         "ORDER BY " + SavedPayment.columnSavedPaymentId + " ASC";

            return this.processSelectCommand(sql);
        }

        public List<SavedPayment> findAllSavedPaymentByStudentId(int studentId)
        {
            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "WHERE " + SavedPayment.columnStudentId + " = " + studentId + " " +
                         "ORDER BY " + SavedPayment.columnSavedPaymentId + " ASC";

            return this.processSelectCommand(sql);
        }

        public List<SavedPayment> findAllSavedPaymentByStudentId(int studentId, SavedPayment.SavedPaymentStatus status)
        {
            string sql = "SELECT * " +
                         "FROM " + SavedPayment.tableName + " " +
                         "WHERE " + SavedPayment.columnStudentId + " = " + studentId + " " +
                         "AND " + SavedPayment.columnStatus + " = '" + status.ToString() + "' " +
                         "ORDER BY " + SavedPayment.columnSavedPaymentId + " ASC";

            return this.processSelectCommand(sql);
        }

        //--------------------------------------------------------------------------------

        private bool processInsertCommand(string sql, SavedPayment savedPayment)
        {
            bool returnFlag = false;

            if (savedPayment != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(SavedPayment.columnStudentId, savedPayment.StudentId);
                        command.Parameters.AddWithValue(SavedPayment.columnReceiverId, savedPayment.ReceiverId);
                        command.Parameters.AddWithValue(SavedPayment.columnTotalPrice, savedPayment.TotalPrice);
                        command.Parameters.AddWithValue(SavedPayment.columnSavedDate, savedPayment.SavedDate);
                        command.Parameters.AddWithValue(SavedPayment.columnPaymentId, savedPayment.PaymentId);
                        command.Parameters.AddWithValue(SavedPayment.columnStatus, savedPayment.Status);

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

        private bool processUpdateCommand(string sql, SavedPayment savedPayment)
        {
            bool returnFlag = false;

            if (savedPayment != null)
            {
                MySqlConnection conn = null;
                try
                {
                    conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                    if (conn != null)
                    {
                        conn.Open();

                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.Parameters.AddWithValue(SavedPayment.columnStudentId, savedPayment.StudentId);
                        command.Parameters.AddWithValue(SavedPayment.columnReceiverId, savedPayment.ReceiverId);
                        command.Parameters.AddWithValue(SavedPayment.columnTotalPrice, savedPayment.TotalPrice);
                        command.Parameters.AddWithValue(SavedPayment.columnSavedDate, savedPayment.SavedDate);
                        command.Parameters.AddWithValue(SavedPayment.columnStatus, savedPayment.Status);
                        command.Parameters.AddWithValue(SavedPayment.columnPaymentId, savedPayment.PaymentId);
                        command.Parameters.AddWithValue(SavedPayment.columnSavedPaymentId, savedPayment.Id);

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

        private bool processDeleteCommand(string sql)
        {
            bool returnFlag = false;

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(DatabaseManager.getMySqlConnectionString());
                if (conn != null)
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);

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

            return returnFlag;
        }

        private List<SavedPayment> processSelectCommand(string sql)
        {
            List<SavedPayment> savedPaymentList = new List<SavedPayment>();

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
                    dataAdapter.Fill(data, SavedPayment.tableName);

                    for (int i = 0; i < data.Tables[SavedPayment.tableName].Rows.Count; i++)
                    {
                        savedPaymentList.Add(generateSavedPayment(data, i));
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

            return savedPaymentList;
        }

        private SavedPayment generateSavedPayment(DataSet data, int index)
        {
            SavedPayment savedPayment = new SavedPayment();
            savedPayment.Id = Convert.ToInt32(data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnSavedPaymentId].ToString());
            savedPayment.StudentId = Convert.ToInt32(data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnStudentId].ToString());
            savedPayment.ReceiverId = Convert.ToInt32(data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnReceiverId].ToString());
            savedPayment.TotalPrice = Convert.ToDouble(data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnTotalPrice].ToString());
            savedPayment.SavedDate = (DateTime)data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnSavedDate];
            savedPayment.PaymentId = Convert.ToInt32(data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnPaymentId].ToString());
            savedPayment.Status = data.Tables[SavedPayment.tableName].Rows[index][SavedPayment.columnStatus].ToString();

            return savedPayment;
        }

        //--------------------------------------------------------------------------------
    }
}
