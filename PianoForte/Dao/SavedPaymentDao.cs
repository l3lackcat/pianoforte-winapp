using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Model;

namespace PianoForte.Dao
{
    public interface SavedPaymentDao
    {
        bool insertSavedPayment(SavedPayment savedPayment);
        bool updateSavedPayment(SavedPayment savedPayment);
        bool deleteSavedPayment(int savedPaymentId);

        SavedPayment findSavedPayment(int savedPaymentId);

        SavedPayment findLastSavedPayment();

        List<SavedPayment> findAllSavedPayment();
        List<SavedPayment> findAllSavedPayment(SavedPayment.SavedPaymentStatus status);

        List<SavedPayment> findAllSavedPaymentByStudentId(int studentId);
        List<SavedPayment> findAllSavedPaymentByStudentId(int studentId, SavedPayment.SavedPaymentStatus status);
    }
}
