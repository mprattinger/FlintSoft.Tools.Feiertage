using System;

namespace FlintSoft.Tools.Feiertage.Models
{
    public class Feiertag
    {
        public bool IsFix { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;

        public Feiertag()
        {

        }

        public Feiertag(bool isFix, DateTime date, string name)
        {
            IsFix = isFix;
            Date = date;
            Name = name;
        }
    }
}
