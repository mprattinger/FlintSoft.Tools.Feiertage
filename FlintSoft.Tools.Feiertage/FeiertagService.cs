using FlintSoft.Extensions;
using FlintSoft.Tools.Feiertage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlintSoft.Tools.Feiertage
{
    public interface IFeiertagService
    {
        Feiertag GetFeiertag(DateTime value);
        bool IsFeiertag(DateTime value);
        bool IsFenstertag(DateTime value, DayOfWeek lastDayOfWorkWeek = DayOfWeek.Friday);
    }

    public class FeiertagService : IFeiertagService
    {
        private readonly List<Feiertag> _feierTage = new List<Feiertag>();

        public FeiertagService()
        {
            load(DateTime.Now.Year);
        }

        /// <summary>
        /// Checks if the given Date is a holiday
        /// </summary>
        /// <param name="value">Date to check</param>
        /// <returns>true if it is a holiday</returns>
        public bool IsFeiertag(DateTime value)
        {
            load(value.Year);
            return _feierTage.Any(x => x.Date.Date == value.Date);
        }

        /// <summary>
        /// Returns the Feiertag class for this date
        /// </summary>
        /// <param name="value">Holiday date</param>
        /// <returns>Feiertag object</returns>
        public Feiertag GetFeiertag(DateTime value)
        {
            load(value.Year);
            return _feierTage.FirstOrDefault(x => x.Date.Date == value.Date);
        }

        /// <summary>
        /// Checks if the given date is a "Fenstertag" (four day weekend) -> Monday if the holiday is on Tuesday or Friday if the holiday is on Thursday
        /// </summary>
        /// <param name="value">Date to check</param>
        /// <param name="lastDayOfWorkWeek">Move the last day of week away from friday. So the Thursday holiday wouldn't be "Fenstertag"</param>
        /// <returns>true if it is a Fensterag</returns>
        public bool IsFenstertag(DateTime value, DayOfWeek lastDayOfWorkWeek = DayOfWeek.Friday)
        {
            load(value.Year);
            if (IsFeiertag(value)) return false;
            if (IsFeiertag(value.Date.AddDays(-1)) && !(value.Date.IsSaturday() || value.Date.IsSunday()) && value.DayOfWeek == lastDayOfWorkWeek) return true;
            if (IsFeiertag(value.Date.AddDays(1)) && !(value.Date.IsSaturday() || value.Date.IsSunday()) && value.DayOfWeek == DayOfWeek.Monday) return true;
            return false;
        }

#pragma warning disable IDE1006 // Naming Styles
        private void load(int year)
#pragma warning restore IDE1006 // Naming Styles
        {
            if (_feierTage.Any(x => x.Date.Year == year)) return; //Bereits geladen

            _feierTage.Add(new Feiertag(true, new DateTime(year, 1, 1), "Neujahr"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 1, 6), "Heilige Drei Könige"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 5, 1), "Staatsfeiertag"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 8, 15), "Mariä Himmelfahrt"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 10, 26), "Nationalfeiertag"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 11, 1), "Allerheiligen "));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 12, 8), "Maria Empfängnis "));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 12, 24), "Hl. Abend"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 12, 25), "Weihnachten"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 12, 26), "Stefanitag"));
            _feierTage.Add(new Feiertag(true, new DateTime(year, 12, 31), "Silverster"));

            DateTime osterSonntag = GetOsterSonntag(year);
            _feierTage.Add(new Feiertag(false, osterSonntag, "Ostersonntag"));
            //_feiertage.Add(new Feiertag(false, osterSonntag.AddDays(-2), "Karfreitag"));
            _feierTage.Add(new Feiertag(false, osterSonntag.AddDays(1), "Ostermontag"));
            _feierTage.Add(new Feiertag(false, osterSonntag.AddDays(39), "Christi Himmelfahrt"));
            _feierTage.Add(new Feiertag(false, osterSonntag.AddDays(49), "Pfingstsonntag"));
            _feierTage.Add(new Feiertag(false, osterSonntag.AddDays(50), "Pfingstmontag"));
            _feierTage.Add(new Feiertag(false, osterSonntag.AddDays(60), "Fronleichnam"));
        }

        private static DateTime GetOsterSonntag(int year)
        {
            int g, h, c, j, l, i;

            g = year % 19;
            c = year / 100;
            h = ((c - (c / 4)) - (((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
            i = h - (h / 28) * (1 - (29 / (h + 1)) * ((21 - g) / 11));
            j = (year + (year / 4) + i + 2 - c + (c / 4)) % 7;

            l = i - j;
            int month = (int)(3 + ((l + 40) / 44));
            int day = (int)(l + 28 - 31 * (month / 4));

            return new DateTime(year, month, day);
        }
    }
}
