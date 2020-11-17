using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.TimetableOLD3.Models
{
    public class Timetable
    {
        public string version = "0.0.1";
        public string academic_year; // tylko dla celow informacyjnych (nie trzymamy planow z innych lat!)
        public DateTime date;
        public string department;
        public List<TimetableEvent> events = new List<TimetableEvent>();

        public Timetable(DateTime date, string department, string academic_year) {
            this.academic_year = academic_year;
            this.date = date;
            this.department = department;
        }

        public static explicit operator Timetable(TimetableOLD2.Models.Timetable timetableOld)
        {
            Timetable t = new Timetable(timetableOld.Date, timetableOld.Events.ToList()[0].Department, timetableOld.Events.ToList()[0].AcademicYear);
            foreach (var e in timetableOld.Events)
            {
                t.events.Add((TimetableEvent)e);
            }
            return t;
        }
    }
}
