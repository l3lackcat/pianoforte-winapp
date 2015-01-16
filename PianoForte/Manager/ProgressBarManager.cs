using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PianoForte.View;

namespace PianoForte.Manager
{
    public class ProgressBarManager
    {
        private static ProgressBarDialogBox progressBarDialogBox = new ProgressBarDialogBox();

        public enum ProgressBarState
        {
            QUERY,
            CREATE_FILE,
            GENERATE_DATE,
            BACKUP_DATABASE,
            UPDATE_STUDENT_STATUS,
            UPDATE_CLASSROOM_STATUS,
            UPDATE_CLASSROOM_DAY_OF_WEEK
        }

        public static void showProgressBar(bool isVisible)
        {
            if (isVisible)
            {
                progressBarDialogBox.show();
            }
            else
            {
                progressBarDialogBox.hide();
            }            
        }

        public static void updateProgressBar(int progressPercentage, ProgressBarState progressBarState)
        {
            switch (progressBarState)
            {
                case ProgressBarState.QUERY:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Marquee);
                    progressBarDialogBox.update("Querying");
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.CREATE_FILE:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Continuous);
                    progressBarDialogBox.update("Creating file", progressPercentage);
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.GENERATE_DATE:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Continuous);
                    progressBarDialogBox.update("Generating data", progressPercentage);
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.BACKUP_DATABASE:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Marquee);
                    progressBarDialogBox.update("Backing up Database");
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.UPDATE_STUDENT_STATUS:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Continuous);
                    progressBarDialogBox.update("Updating students' status", progressPercentage);
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.UPDATE_CLASSROOM_STATUS:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Continuous);
                    progressBarDialogBox.update("Updating classrooms' status", progressPercentage);
                    progressBarDialogBox.update(progressPercentage);
                    break;
                case ProgressBarState.UPDATE_CLASSROOM_DAY_OF_WEEK:
                    progressBarDialogBox.setProgressBarStyle(System.Windows.Forms.ProgressBarStyle.Continuous);
                    progressBarDialogBox.update("Updating classrooms' day of week", progressPercentage);
                    progressBarDialogBox.update(progressPercentage);
                    break;
            }
        }
    }
}
