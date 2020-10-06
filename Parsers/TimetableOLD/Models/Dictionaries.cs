using System.Collections.Generic;

namespace Parsers.TimetableOLD.Models
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> ModesDictionary = new Dictionary<string, string>
        {
            {"ST", "Stacjonarne"},
            {"NS", "Niestacjonarne" },
            {"NST", "Niestacjonarne" },
            {"NSTA", "NiestacjonarneA" },
            {"NSTB", "NiestacjonarneB" },
            {"NSA", "NiestacjonarneA" },
            {"NSB", "NiestacjonarneB" },
        };

        public static Dictionary<string, string> ModesDictionary2 = new Dictionary<string, string>
        {
            {"Stacjonarne", "ST"},
            {"Niestacjonarne", "NST" },
            {"NiestacjonarneA", "NSTA" },
            {"NiestacjonarneB", "NSTB" },
        };

        public static Dictionary<string, string> FieldsDictionary = new Dictionary<string, string>
        {
            {"INF", "Informatyka"},
            {"IIE", "Informatyka i Ekonometria" }
        };

        public static Dictionary<string, string> FieldsDictionary2 = new Dictionary<string, string>
        {
            {"Informatyka", "Inf"},
            {"Informatyka i Ekonometria", "IiE" }
        };

        public static Dictionary<string, string> DaysOfWeekDictionary = new Dictionary<string, string>
        {
            {"1", "PN"},
            {"2", "WT"},
            {"3", "ŚR"},
            {"4", "CZW"},
            {"5", "PT"},
            {"6", "SO"},
            {"7", "NIE"}
        };
        public static Dictionary<string, string> DaysOfWeekDictionary2 = new Dictionary<string, string>
        {
            {"PN", "1"},
            {"WT", "2"},
            {"ŚR", "3"},
            {"CZW", "4"},
            {"PT", "5"},
            {"SO", "6"},
            {"NIE", "7"}
        };

        public static Dictionary<string, string> TypesOfEventDictionary = new Dictionary<string, string>()
        {
            {"W", "wykład"},
            {"ĆW", "ćwiczenia"},
            {"CW", "ćwiczenia" },
            {"LAB", "laboratorium"},
            {"FAQ", "fakultet" },
            {"F", "fakultet" }
        };

        public static Dictionary<string, string> TypesOfEventDictionary2 = new Dictionary<string, string>()
        {
            {"wykład", "W"},
            {"ćwiczenia", "ĆW"},
            {"laboratorium", "LAB"},
            {"Fakultet", "FAQ" }
        };
        public static Dictionary<string, string> DegreesDictionary = new Dictionary<string, string>()
        {
            {"INŻ", "inżyrierskie"},
            {"INZ", "inżyrierskie"},
            {"MGR", "magisterskie"},
            {"LIC", "licencjackie"},
            {"DR", "doktorat" }
        };
        public static Dictionary<string, string> DegreesDictionary2 = new Dictionary<string, string>()
        {
            {"INŻYNIERSKIE", "inż"},
            {"INZYNIERSKIE", "inż"},
            {"INŻYNIER", "inż"},
            {"INZYNIER", "inż"},
            {"INZ", "inż"},
            {"INŻ", "inż"},
            {"LICENCJACKIE", "lic"},
            {"LICENCJAT", "lic"},
            {"LIC", "lic"},
            {"MAGISTERSKIE", "mgr"},
            {"MAGISTER", "mgr"},
            {"MGR", "mgr"},
            {"DOKTORAT", "dr"},
            {"DOKTOR", "dr"},
            {"DR", "dr"},
        };
    }
}
