using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PianoForte.Model
{
    class Schedule
    {
        public int SaleId { get; set; }
        public int SaleDetailId { get; set; }
        public int TeacherId { get; set; }
        public int NumberOfRepeat { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
