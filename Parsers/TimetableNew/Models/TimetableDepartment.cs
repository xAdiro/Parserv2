using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableDepartment
    {
        public string Department { get; set; }
        public List<TimetableMode> Modes { get; set; } = new List<TimetableMode>();

        public List<TimetableMode> this[string ModeName]    // Indexer declaration  
        {
            get
            {
                return Modes.FindAll(i => i.Mode == ModeName);
            }
        }
    }
}