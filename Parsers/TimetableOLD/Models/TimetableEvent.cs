using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.TimetableOLD.Models
{
    public class TimetableEvent
    {
        public string Department { get; set; }
        public string FieldOfStudy { get; set; }
        public string Mode { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
        public string Group { get; set; }
        public bool IsFaculty { get; set; }
        public string FacultyGroup { get; set; }
        public string Specialization { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public string DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public string[] Lecturers { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string AcademicYear { get; set; }
    }
}
