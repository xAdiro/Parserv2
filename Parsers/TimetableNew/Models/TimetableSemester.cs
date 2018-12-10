using System;
using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableSemester
    {
        public int Semester { get; set; }
        public int Year { get; set; }
        public string Degree { get; set; }
        public List<TimetableDay> Days { get; set; } = new List<TimetableDay>();//all 5 days or 3

        public List<TimetableDay> this[DayOfWeek dayName]    // Indexer declaration  
        {
            get
            {
                return Days.FindAll(i => i.DayOfWeek == dayName);
            }
        }
    }
}