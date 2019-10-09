using Parsers.TimetableOLD.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parsers.TimetableOLD
{
    public static class Parser
    {
        public static Timetable ParseTimetableFiles(IEnumerable<string> fileContents)
        {
            var events = new List<TimetableEvent>();
            var date = new DateTime();

            foreach (var content in fileContents)
            {
                //var lines = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var lines = Regex.Split(content, Environment.NewLine);
                var i = 0;
                DateTime dateFromFile;
                try
                {
                    // get info about current group timetable
                    dateFromFile = DateTime.ParseExact(lines[i], new[] { "dd.MM.yyyy HH:mm", "d.MM.yyyy HH:mm", "dd.M.yyyy HH:mm", "d.M.yyyy HH:mm",
                                                                         "dd.MM.yyyy H:mm", "d.MM.yyyy H:mm", "dd.M.yyyy H:mm", "d.M.yyyy H:mm"}, null, DateTimeStyles.AllowWhiteSpaces);
                }
                catch (Exception ex)
                {
                    string errMsg = "Niepoprawna data: " + lines[i];
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }

                date = dateFromFile > date ? dateFromFile : date; // get the most recent date from files
                i++; // next line

                var info = lines[i].Split(',');

                // checks
                if (info.Length < 9)
                {
                    string errMsg = "Spodziewano się min 9 wartości w drugiej linii pliku, a otrzymano " + info.Length;
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                if (info[6].Length < 1 || !char.IsDigit(info[6].Trim()[1]))
                {
                    string errMsg = "Rok studiów w złym formacie " + info[6];
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                if (info[7].Length < 1 || !char.IsDigit(info[7].Trim()[1]))
                {
                    string errMsg = "Semestr studiów w złym formacie " + info[7];
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                if (info[8].Length < 2 || !char.IsDigit(info[8].Trim()[2]))
                {
                    string errMsg = "Grupa w złym formacie " + info[8];
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }

                var department = info[0];
                var isFieldKnown = Dictionaries.FieldsDictionary.TryGetValue(info[4].Trim().ToUpper(), out var fieldOfStudy);
                var mode = Dictionaries.ModesDictionary.ContainsKey(info[3].Trim().ToUpper())
                    ? Dictionaries.ModesDictionary[info[3].Trim().ToUpper()]
                    : info[3].Trim().ToUpper();
                //var fieldOfStudy = Dictionaries.FieldsDictionary[info[4].Trim().ToUpper()];
                var degree = info[5].Trim();
                var year = info[6].Trim()[1].ToString();
                var semester = info[7].Trim()[1].ToString();
                var group = info[8].Trim()[2].ToString();
                var specialization = info.Length == 10 ? string.Concat(info[9].Take(info[9].Length - 1)).Trim() : "";
                int.TryParse(info[1], out int creationYear);
                var academicYear = info[2].ToLower().Trim() == "jesień" || info[2].ToLower().Trim() == "j"
                    ? $"{creationYear}/{creationYear + 1}"
                    : $"{creationYear - 1}/{creationYear}";

                while (true)
                {
                    var endOfFile = false;


                    while (!lines[i].Contains("ZJ"))
                    {
                        i++;
                        if (!lines[i].Contains("end.")) continue;
                        endOfFile = true;
                        break;
                    }

                    if (endOfFile)
                    {
                        break;
                    }
                    //check lines amount
                    if (i + 5 >= lines.Length)
                    {
                        string errMsg = "Ilość linii w pliku nie jest prawidłowa";
                        Console.WriteLine(errMsg);
                        throw new Exception(errMsg);
                    }

                    var room = lines[i + 3].Split(',')[0].Trim();
                    string day = "";

                    try
                    {
                        Dictionaries.DaysOfWeekDictionary.TryGetValue(lines[i + 2][1].ToString().ToUpper(), out day);
                    }
                    catch (Exception ex)
                    {
                        string errMsg = "Podano niepoprawny dzień tygodnia " + lines[i + 2][1] + " " + ex.Message;
                        Console.WriteLine(errMsg);
                        throw new Exception(errMsg);
                    }

                    var timetableEvent =
                        new TimetableEvent
                        {
                            Name = string.Concat(lines[i + 1].TakeWhile(c => c != '[')).Trim(),
                            DayOfWeek = day,
                            StartTime = string.Concat(lines[i + 2].SkipWhile(c => c != ',').Skip(1).TakeWhile(c => c != '-')).Trim(),
                            EndTime = string.Concat(lines[i + 2].SkipWhile(c => c != '-').Skip(1)).Trim(),
                            Room = room,
                            Building = lines[i + 3].Split(',').ElementAtOrDefault(1)?.Trim() ?? (department.Equals("WZIM") && !string.IsNullOrWhiteSpace(room) ? "34" : null),
                            Lecturers = lines[i + 4].Split(new char[','], StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToArray(),
                            Remarks = string.Concat(lines[i + 5].SkipWhile(c => c != ':').Skip(1)).Trim(),
                            Department = department,
                            Mode = mode,
                            Degree = degree,
                            Year = year,
                            Semester = semester,
                            Group = group,
                            Specialization = specialization,
                            FieldOfStudy = isFieldKnown ? fieldOfStudy : info[4].Trim().ToUpper(),
                            AcademicYear = academicYear,
                            IsFaculty = lines[i + 1].Contains("(Faq)") || lines[i + 1].Contains("(faq)") || lines[i + 1].Contains("(f)") || lines[i + 1].Contains("(F)"),
                            FacultyGroup = string.Concat(lines[i + 1].SkipWhile(c => c != ')').Skip(1).SkipWhile(c => c != ')').SkipWhile(c => c != '(').Skip(1).TakeWhile(c => c != ')')).Trim()
                        };

                    var type = string.Concat(lines[i + 1]
                        .SkipWhile(c => c != '[')
                        .Skip(1)
                        .TakeWhile(c => c != ']'))
                        .Trim()
                        .ToUpper();
                    timetableEvent.Type = Dictionaries.TypesOfEventDictionary.ContainsKey(type)
                        ? Dictionaries.TypesOfEventDictionary[type]
                        : type;

                    events.Add(timetableEvent);

                    i++;
                }
            }
            return new Models.Timetable { Date = date, Events = events };
        }

        public static void SaveTimetableToFiles(Timetable timetable, string folderName)
        {
            DateTime date = timetable.Date;
            List<TimetableEvent> events = (List<TimetableEvent>)timetable.Events;
            Console.WriteLine("Saving events to files");
            if (!Directory.Exists(@"./" + folderName)) Directory.CreateDirectory(@"./" + folderName);
            List<string> files = new List<string>();
            foreach (var e in events)
            {
                SaveEventToFile(e, date, folderName, out string f);
                if (!files.Contains(f)) files.Add(f);
            }
            foreach (var f in files)
            {
                StreamWriter sw = new StreamWriter(f, true);
                sw.WriteLine("end.");
                sw.Close();
            }
        }

        public static void SaveEventToFile(TimetableEvent e, DateTime date, string folderName, out string fileSavedName)
        {
            string mode = e.Mode, fieldOfStudy = e.FieldOfStudy, type = e.Type;
            if (Dictionaries.ModesDictionary2.ContainsKey(mode)) Dictionaries.ModesDictionary2.TryGetValue(mode, out mode);
            if (Dictionaries.FieldsDictionary2.ContainsKey(fieldOfStudy)) Dictionaries.FieldsDictionary2.TryGetValue(fieldOfStudy, out fieldOfStudy);
            if (Dictionaries.TypesOfEventDictionary2.ContainsKey(type)) Dictionaries.TypesOfEventDictionary2.TryGetValue(type, out type);


            string fileName = folderName + "/" + e.Department + "_2019_W_" + mode + "_" + fieldOfStudy + "_" + e.Degree + "_R" + e.Year + "S" + e.Semester + "_";
            if (e.Specialization != "") fileName += e.Specialization; else fileName += "gr" + e.Group;
            fileName += ".txt";

            fileSavedName = fileName;
            bool alreadyExists = File.Exists(fileName);
            if (!alreadyExists)
            {
                File.CreateText(fileName).Close();
            }

            //count lines
            int linesCount = 0;
            StreamReader streamReader = new StreamReader(fileName);
            while (!streamReader.EndOfStream) { streamReader.ReadLine(); linesCount++; }
            streamReader.Close();

            StreamWriter streamWriter = new StreamWriter(fileName, true);
            if (!alreadyExists)
            {
                string formatedDate = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).Replace('/', '.');
                streamWriter.WriteLine(formatedDate);
                string formatedInfo = string.Format(e.Department + ", 2018, J, " + mode + ", " + fieldOfStudy + ", " + e.Degree + ", R" + e.Year + ", S" + e.Semester + ", gr" + e.Group + (e.Specialization != "" ? ", " + e.Specialization + ";" : ";"));
                streamWriter.WriteLine(formatedInfo);
                streamWriter.WriteLine("------------------------------------------------");
            }
            if (linesCount > 9) linesCount -= 2;
            string eventIndex = "ZJ_" + (linesCount / 7 + 1).ToString().PadLeft(2, '0');
            streamWriter.WriteLine(eventIndex);
            streamWriter.WriteLine(e.Name + " [" + type + "]");
            streamWriter.WriteLine("d" + Dictionaries.DaysOfWeekDictionary2[e.DayOfWeek] + ", " + e.StartTime + "-" + e.EndTime);
            streamWriter.WriteLine(e.Room + (e.Building != "34" ? ", " + e.Building : ""));
            string lecturers = "";
            for (int i = 0; i < e.Lecturers.Length; i++)
            {
                lecturers += e.Lecturers[i];
                if (i != e.Lecturers.Length - 1)
                    lecturers += ", ";
            }
            streamWriter.WriteLine(lecturers);
            streamWriter.WriteLine("U: " + e.Remarks);
            streamWriter.WriteLine("------------------------------------------------");

            streamWriter.Close();



        }

        public static Timetable SortTimetable(Timetable t)
        {
            t.Events.OrderBy(i => i.Department)
                .ThenBy(i => i.Mode)
                .ThenBy(i => i.FieldOfStudy)
                .ThenBy(i => i.Degree)
                .ThenBy(i => i.Semester)
                .ThenBy(i => i.DayOfWeek)
                .ThenBy(i => i.StartTime)
                .ThenBy(i => i.EndTime);
            return t;
        }

    }
}
