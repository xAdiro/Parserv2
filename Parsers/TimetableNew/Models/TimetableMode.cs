using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableMode
    {
        public string Mode { get; set; }
        public List<TimetableField> Fields { get; set; } = new List<TimetableField>();

        public List<TimetableField> this[string FieldName]    // Indexer declaration  
        {
            get
            {
                return Fields.FindAll(i => i.FieldOfStudy == FieldName);
            }
        }
    }
}