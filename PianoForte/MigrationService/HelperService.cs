using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

namespace PianoForte.MigrationService
{
    class HelperService
    {
        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine(ex);
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void addCellHeader(Excel.Worksheet excelWorkSheet, string topLeft, string bottomRight, int columnWidth, string text)
        {
            Excel.Range chartRange;

            excelWorkSheet.get_Range(topLeft, bottomRight).Merge(false);
            chartRange = excelWorkSheet.get_Range(topLeft, bottomRight);
            chartRange.ColumnWidth = columnWidth;
            chartRange.FormulaR1C1 = text;
            chartRange.Font.Name = "MS Sans Serif";
            chartRange.Font.Size = 10;
            chartRange.Font.Bold = true;
            chartRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            chartRange.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
        }

        public static void addCellData(Excel.Worksheet excelWorkSheet, string topLeft, string bottomRight, Excel.XlHAlign horizontalAlignment, string text)
        {
            Excel.Range chartRange;

            chartRange = excelWorkSheet.get_Range(topLeft, bottomRight);
            chartRange.FormulaR1C1 = text;
            chartRange.Font.Name = "MS Sans Serif";
            chartRange.Font.Size = 10;
            chartRange.HorizontalAlignment = horizontalAlignment;
            chartRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }
    }
}
