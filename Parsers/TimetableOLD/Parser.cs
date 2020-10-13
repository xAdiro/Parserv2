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
        public static Timetable ParseTimetableFile(string filepath)
        {

            StreamReader sr = new StreamReader(filepath);
            string[] lines = Regex.Split(sr.ReadToEnd(), Environment.NewLine);
            sr.Close();

            var events = new List<TimetableEvent>();
            var date = new DateTime();
            var i = 0;
            string BuildErrorMsg(string msg)
            {
                return string.Format("{0}\nw linii: {1}\nPlik: {2}", msg, i, filepath);
            }
            DateTime dateFromFile;
            try
            {
                // get info about current group timetable
                dateFromFile = DateTime.ParseExact(lines[i], new[] { "dd.MM.yyyy HH:mm", "d.MM.yyyy HH:mm", "dd.M.yyyy HH:mm", "d.M.yyyy HH:mm",
                                                                         "dd.MM.yyyy H:mm", "d.MM.yyyy H:mm", "dd.M.yyyy H:mm", "d.M.yyyy H:mm"}, null, DateTimeStyles.AllowWhiteSpaces);
            }
            catch (Exception ex)
            {
                string errMsg = BuildErrorMsg("Niepoprawna data: " + lines[i]);
                Console.WriteLine(errMsg);
                throw new Exception(errMsg);
            }

            date = dateFromFile > date ? dateFromFile : date; // get the most recent date from files
            i++; // next line

            var info = lines[i].Split(',');

            // checks
            if (info.Length < 9)
            {
                string errMsg = BuildErrorMsg("Spodziewano się min 9 wartości w drugiej linii pliku, a otrzymano " + info.Length);
                Console.WriteLine(errMsg);
                throw new Exception(errMsg);
            }
            if (info[6].Length < 1 || !char.IsDigit(info[6].Trim()[1]))
            {
                string errMsg = BuildErrorMsg("Rok studiów w złym formacie " + info[6]);
                Console.WriteLine(errMsg);
                throw new Exception(errMsg);
            }
            if (info[7].Length < 1 || !char.IsDigit(info[7].Trim()[1]))
            {
                string errMsg = BuildErrorMsg("Semestr studiów w złym formacie " + info[7]);
                Console.WriteLine(errMsg);
                throw new Exception(errMsg);
            }
            if (info[8].Length < 2 || !char.IsDigit(info[8].Trim()[2]))
            {
                string errMsg = BuildErrorMsg("Grupa w złym formacie " + info[8]);
                Console.WriteLine(errMsg);
                throw new Exception(errMsg);
            }

            var department = info[0];
            var isFieldKnown = Dictionaries.FieldsDictionary.TryGetValue(info[4].Trim().ToUpper(), out var fieldOfStudy);
            var mode = Dictionaries.ModesDictionary.ContainsKey(info[3].Trim().ToUpper())
                ? Dictionaries.ModesDictionary[info[3].Trim().ToUpper()]
                : info[3].Trim();
            //var fieldOfStudy = Dictionaries.FieldsDictionary[info[4].Trim().ToUpper()];
            var degree = Dictionaries.DegreesDictionary2.ContainsKey(info[5].Trim().ToUpper()) ? Dictionaries.DegreesDictionary2[info[5].Trim().ToUpper()] : info[5].Trim();
            var year = info[6].Trim().Substring(1).ToString();
            var semester = info[7].Trim().Substring(1).ToString();
            // szybki fix grup (jesli grupy sa brane pod uwage to powinno byc ok, jesli specka jest brana ta wartosc mozliwe ze bedzie spierdolona)
            info[8] = info[8].Trim();
            int groupLastCharIndex = info[8].Length;
            if(info[8].IndexOf(';') != -1)
            {
                groupLastCharIndex = info[8].IndexOf(';');
            }
            if (info[8].IndexOf(' ') != -1)
            {
                if(info[8].IndexOf(' ') < groupLastCharIndex)
                    groupLastCharIndex = info[8].IndexOf(' ');
            }

            var group = info[8].Trim().Substring(2, groupLastCharIndex-2).ToString();
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
                    string errMsg = BuildErrorMsg("Ilość linii w pliku nie jest prawidłowa");
                    Console.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }

                var room = lines[i + 3].Split(',')[0].Trim();
                var building = lines[i + 3].Split(',').ElementAtOrDefault(1);
                if(string.IsNullOrEmpty(building)) building = "";
                string day = "";

                try
                {
                    Dictionaries.DaysOfWeekDictionary.TryGetValue(lines[i + 2][1].ToString().ToUpper(), out day);
                }
                catch (Exception ex)
                {
                    string errMsg = BuildErrorMsg("Podano niepoprawny dzień tygodnia " + lines[i + 2][1]);
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
                        //Building = lines[i + 3].Split(',').ElementAtOrDefault(1)?.Trim() ?? (department.Equals("WZIM") && !string.IsNullOrWhiteSpace(room) ? "34" : null),
                        Building = building,
                        Lecturers = lines[i + 4].Split(new char[','], StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToArray(),
                        Remarks = string.Concat(lines[i + 5].SkipWhile(c => c != ':').Skip(1)).Trim(),
                        Department = department,
                        Mode = mode,
                        Degree = degree,
                        Year = year,
                        Semester = semester,
                        Group = group,
                        Specialization = specialization,
                        FieldOfStudy = isFieldKnown ? fieldOfStudy : info[4].Trim(),
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
            return new Timetable { Date = date, Events = events };
        }

        public static Timetable ParseTimetableFiles(IEnumerable<string> filepaths)
        {
            Timetable result = new Timetable();
            foreach (var fp in filepaths)
            {
                result = result.MergeTimetables(ParseTimetableFile(fp));
            }
            result.SortTimetable();
            return result;
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


            string fileName = folderName + "/" + e.Department + "_2019_J_" + mode + "_" + fieldOfStudy + "_" + e.Degree + "_R" + e.Year + "S" + e.Semester + "_";
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
                string formatedInfo = string.Format(e.Department + ", 2019, J, " + mode + ", " + fieldOfStudy + ", " + e.Degree + ", R" + e.Year + ", S" + e.Semester + ", gr" + e.Group + (e.Specialization != "" ? ", " + e.Specialization + ";" : ";"));
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

    }
}
