using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableField
    {
        public string FieldOfStudy { get; set; }
        public List<TimetableSemester> Semesters { get; set; } = new List<TimetableSemester>();

        public List<TimetableSemester> this[int semesterNumber, string degree, int year]    // Indexer declaration  
        {
            get
            {
                return Semesters.FindAll(i => i.Semester == semesterNumber && i.Degree == degree && i.Year == year);
            }
        }
    }
}