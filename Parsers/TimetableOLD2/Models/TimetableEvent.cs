using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parsers.TimetableOLD2.Models
{
    public class TimetableGroup
    {
        public string Group { get; set; }
        public string Specialization { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Specialization, Group);
        }

        public override bool Equals(object obj)
        {
            if (obj is TimetableGroup group)
            {
                if (group.Group == Group && group.Specialization == Specialization)
                    return true;
            }
            return false;
        }

        public override int GetHashCode() => (
            Group,
            Specialization
            ).GetHashCode();
    }
    public class TimetableEvent : IEquatable<TimetableEvent>
    {
        public string Department { get; set; }
        public string FieldOfStudy { get; set; }
        public string Mode { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
        public HashSet<TimetableGroup> Group { get; set; }
        public bool IsFaculty { get; set; }
        public string FacultyGroup { get; set; }
        public string Degree { get; set; }
        public string Name { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Building { get; set; }
        public string Room { get; set; }
        public HashSet<string> Lecturers { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string AcademicYear { get; set; }

        public static explicit operator TimetableEvent(TimetableOLD.Models.TimetableEvent timetableOldEvent)
        {
            HashSet<string> lecturers = new HashSet<string>();
            List<string> days = new List<string>() { "PN", "WT", "ŚR", "CZW", "PT", "SO", "NIE" };
            foreach (var l in timetableOldEvent.Lecturers)
            {
                lecturers.Add(l);
            }
            int dayIndex = days.IndexOf(timetableOldEvent.DayOfWeek);
            if(dayIndex > days.Count || dayIndex < 0)
            {
                MessageBox.Show("dzień nieznaleziony!");
            }
            return new TimetableEvent()
            {
                AcademicYear = timetableOldEvent.AcademicYear,
                Building = timetableOldEvent.Building,
                DayOfWeek = dayIndex,
                Degree = timetableOldEvent.Degree,
                Department = timetableOldEvent.Department,
                EndTime = timetableOldEvent.EndTime,
                FacultyGroup = timetableOldEvent.FacultyGroup,
                FieldOfStudy = timetableOldEvent.FieldOfStudy,
                IsFaculty = timetableOldEvent.IsFaculty,
                Lecturers = lecturers,
                Mode = timetableOldEvent.Mode,
                Name = timetableOldEvent.Name,
                Remarks = timetableOldEvent.Remarks,
                Room = timetableOldEvent.Room,
                Semester = timetableOldEvent.Semester,
                StartTime = timetableOldEvent.StartTime,
                Type = timetableOldEvent.Type,
                Year = timetableOldEvent.Year,
                Group = new HashSet<TimetableGroup>() { new TimetableGroup() { Group = timetableOldEvent.Group, Specialization = timetableOldEvent.Specialization } },
            };
        }

        public bool Equals(TimetableEvent other)
        {
            if (other is null) return false;

            if (Department != other.Department) return false;
            if (FieldOfStudy != other.FieldOfStudy) return false;
            if (Mode != other.Mode) return false;
            if (Year != other.Year) return false;
            if (Semester != other.Semester) return false;
            if (IsFaculty != other.IsFaculty) return false;
            if (FacultyGroup != other.FacultyGroup) return false;
            if (Degree != other.Degree) return false;
            if (Name != other.Name) return false;
            if (DayOfWeek != other.DayOfWeek) return false;
            if (StartTime != other.StartTime) return false;
            if (EndTime != other.EndTime) return false;
            if (Building != other.Building) return false;
            if (Room != other.Room) return false;
            //if (!Lecturers.SetEquals(other.Lecturers)) return false;
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
