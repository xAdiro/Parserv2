using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableAcademicYear
    {
        public string AcademicYear { get; set; }
        public List<TimetableDepartment> Departments { get; set; } = new List<TimetableDepartment>();

        public List<TimetableDepartment> this[string DepartmentName]    // Indexer declaration  
        {
            get
            {
                return Departments.FindAll(i => i.Department == DepartmentName);
            }
        }
    }
}