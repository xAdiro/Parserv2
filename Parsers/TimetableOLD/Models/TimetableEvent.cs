using System;

namespace Parsers.TimetableOLD.Models
{
    public class TimetableEvent : IEquatable<TimetableEvent>
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

        public bool Equals(TimetableEvent other)
        {
            if (other is null) return false;

            if (Department != other.Department) return false;
            if (FieldOfStudy != other.FieldOfStudy) return false;
            if (Mode != other.Mode) return false;
            if (Year != other.Year) return false;
            if (Semester != other.Semester) return false;
            if (Group != other.Group) return false;
            if (IsFaculty != other.IsFaculty) return false;
            if (FacultyGroup != other.FacultyGroup) return false;
            if (Specialization != other.Specialization) return false;
            if (Degree != other.Degree) return false;
            if (Name != other.Name) return false;
            if (DayOfWeek != other.DayOfWeek) return false;
            if (StartTime != other.StartTime) return false;
            if (EndTime != other.EndTime) return false;
            if (Building != other.Building) return false;
            if (Room != other.Room) return false;
            if (Lecturers != other.Lecturers) return false;
            if (Type != other.Type) return false;
            if (Remarks != other.Remarks) return false;
            if (AcademicYear != other.AcademicYear) return false;
            return true;
        }
        public override bool Equals(object obj) => Equals(obj as TimetableEvent);
        public override int GetHashCode() => (
            Department,
            FieldOfStudy,
            Mode,
            Year,
            Semester,
            Group,
            IsFaculty,
            FacultyGroup,
            Specialization,
            Degree,
            Name,
            DayOfWeek,
            StartTime,
            EndTime,
            Building,
            Room,
            Lecturers,
            Type,
            Remarks,
            AcademicYear
            ).GetHashCode();
    }
}
