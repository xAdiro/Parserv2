using Parsers.TimetableOLD.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Parsers.TimetableNew.Models
{
    public class Timetable
    {
        public DateTime Date { get; set; }
        public List<TimetableDepartment> Departments { get; set; } = new List<TimetableDepartment>();

        public List<TimetableDepartment> this[string DepartmentName] 
        {
            get
            {
                return Departments.FindAll(i => i.Department == DepartmentName);
            } 
        }

        public static explicit operator Timetable(TimetableOLD.Models.Timetable timetableOLD)
        {
            Timetable t = new Timetable() { Date = timetableOLD.Date };
            foreach (var item in timetableOLD.Events)
            {
                TimetableEvent e = new TimetableEvent()
                {
                    Building = item.Building ?? "",
                    EndTime = DateTime.ParseExact(item.EndTime, new[] { "HH:mm", "H:mm" }, null, DateTimeStyles.AllowWhiteSpaces),
                    StartTime = DateTime.ParseExact(item.StartTime, new[] { "HH:mm", "H:mm" }, null, DateTimeStyles.AllowWhiteSpaces),
                    Groups = new List<TimetableGroup>() { new TimetableGroup() { Group = Convert.ToInt32(item.Group), Specialization = item.Specialization } },
                    IsFaculty = item.IsFaculty,
                    Lecturers = item.Lecturers.ToList(),
                    FacultyGroups = new List<string>() { item.FacultyGroup },
                    Name = item.Name,
                    Remarks = item.Remarks,
                    Room = item.Room,
                    Type = item.Type,

                };
                TimetableDepartment td = Parser.MakeTimetableDepartment(item.Department, item.Mode, item.FieldOfStudy, Convert.ToInt32(item.Semester), item.Degree.ToLower(), Convert.ToInt32(item.Year), (DayOfWeek)Convert.ToInt32(Dictionaries.DaysOfWeekDictionary.First(i => i.Value == item.DayOfWeek).Key), e);
                Parser.MergeToTimetable(t, td);
            }
            Parser.SortTimetable(t);

            return t;
        }

    }
}
