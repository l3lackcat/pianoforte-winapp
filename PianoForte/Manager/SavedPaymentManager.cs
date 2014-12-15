using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Dao;
using PianoForte.Model;
using System.Globalization;

namespace PianoForte.Manager
{
    public class SavedPaymentManager
    {
        private static SavedPaymentDao savedPaymentDao = DaoFactory.getDaoFactory(DaoFactory.FactoryType.MYSQL).getSavedPaymentDao();

        //--------------------------------------------------------------------------------

        public static SavedPayment insertSavedPayment(SavedPayment savedPayment)
        {
            bool isInsertComplete = savedPaymentDao.insertSavedPayment(savedPayment);

            if (isInsertComplete)
            {
                savedPayment.Id = savedPaymentDao.findLastSavedPayment().Id;
            }
            else
            {
                savedPayment = null;
            }

            return savedPayment;
        }

        public static bool updateSavedPayment(SavedPayment savedPayment)
        {
            return savedPaymentDao.updateSavedPayment(savedPayment);
        }

        public static bool deleteSavedPayment(int savedPaymentId)
        {
            return savedPaymentDao.deleteSavedPayment(savedPaymentId);
        }

        //--------------------------------------------------------------------------------

        public static SavedPayment findSavedPayment(int savedPaymentId)
        {
            return savedPaymentDao.findSavedPayment(savedPaymentId);
        }

        public static SavedPayment findLastSavedPayment()
        {
            return savedPaymentDao.findLastSavedPayment();
        }

        public static List<SavedPayment> findAllSavedPayment()
        {
            return savedPaymentDao.findAllSavedPayment();
        }

        public static List<SavedPayment> findAllSavedPayment(SavedPayment.SavedPaymentStatus status)
        {
            List<SavedPayment> savedPaymentList;

            if (status == SavedPayment.SavedPaymentStatus.ALL)
            {
                savedPaymentList = savedPaymentDao.findAllSavedPayment();
            }
            else
            {
                savedPaymentList = savedPaymentDao.findAllSavedPayment(status);
            }

            return savedPaymentList;
        }

        public static List<SavedPayment> findAllSavedPaymentByStudentId(int studentId)
        {
            return savedPaymentDao.findAllSavedPaymentByStudentId(studentId);
        }

        public static List<SavedPayment> findAllSavedPaymentByStudentId(int studentId, SavedPayment.SavedPaymentStatus status)
        {
            List<SavedPayment> savedPaymentList;

            if (status == SavedPayment.SavedPaymentStatus.ALL)
            {
                savedPaymentList = savedPaymentDao.findAllSavedPaymentByStudentId(studentId);
            }
            else
            {
                savedPaymentList = savedPaymentDao.findAllSavedPaymentByStudentId(studentId, status);
            }

            return savedPaymentList;
        }

        //--------------------------------------------------------------------------------

        public static SavedPayment processSavedPayment(int studentId, int receiverId, double totalPrice, SavedPayment.SavedPaymentStatus status)
        {
            SavedPayment savedPayment = new SavedPayment();
            savedPayment.StudentId = studentId;
            savedPayment.ReceiverId = receiverId;
            savedPayment.TotalPrice = totalPrice;
            savedPayment.SavedDate = DateTime.Today;
            savedPayment.PaymentId = 0;
            savedPayment.Status = status.ToString();

            return SavedPaymentManager.insertSavedPayment(savedPayment);
        }
    }
}
