using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableGroup
    {
        public int Group { get; set; }
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

        public override int GetHashCode()
        {
            var hashCode = 464063316;
            hashCode = hashCode * -1521134295 + Group.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Specialization);
            return hashCode;
        }
    }
}