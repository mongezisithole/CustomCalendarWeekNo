using System.Globalization;

namespace CustomCalendarWeekNo
{
    public static class CustomCalendar
    {
        private static DayOfWeek _firstDayOfWeek = DayOfWeek.Monday;
        private static CalendarWeekRule _calendarWeekRule = CalendarWeekRule.FirstFullWeek;

        private static readonly int WeeksInLongYear = 53;
        private static readonly int WeeksInShortYear = 52;

        private static readonly int MinWeek = 1;

        private static CultureInfo _culture = CultureInfo.CurrentCulture;

        private static int GetWeekNumber(DateTime dateTime)
        {
            return _culture.Calendar.GetWeekOfYear(dateTime, _calendarWeekRule, _firstDayOfWeek);
        }

        private static int GetWeeksInYear(int year)
        {
            static int P(int y) => (y + (y / 4) - (y / 100) + (y / 400)) % (int)_firstDayOfWeek;

            if (P(year) == 4 || P(year - 1) == 3)
            {
                return WeeksInLongYear;
            }

            return WeeksInShortYear;
        }

        private static int GetYear(DateTime date)
        {
            int week = GetWeekNumber(date);

            if (week < MinWeek)
            {
                // If the week number obtained equals 0, it means that the
                // given date belongs to the preceding (week-based) year.
                return date.Year - 1;
            }

            if (week > GetWeeksInYear(date.Year))
            {
                // If a week number of 53 is obtained, one must check that
                // the date is not actually in week 1 of the following year.
                return date.Year + 1;
            }

            return date.Year;
        }

        private static int GetWeekNumber(DateTime date)
        {
            return _culture.Calendar.GetWeekOfYear(date, _calendarWeekRule, _firstDayOfWeek);
        }
        
        private static string WeekNumberAsString(DateTime dateTime)
        {
            return $"{GetYear(dateTime)}{GetWeekNumber(dateTime):00}";
        }
        public static string GetCurrentWeek()
        {
            var date = DateTime.Now;

            return GetWeekNumberAsString(date);
        }

        public static string GetWeekNumberAsString(DateTime dateTime)
        {
            return WeekNumberAsString(dateTime);
        }

        public static string GetWeekNumberAsString(DateTime dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            _firstDayOfWeek = firstDayOfWeek;

            return WeekNumberAsString(dateTime);
        }

        public static string GetWeekNumberAsString(DateTime dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Monday
            , CalendarWeekRule calendarWeekRule = CalendarWeekRule.FirstFullWeek)
        {
            _firstDayOfWeek = firstDayOfWeek;
            _calendarWeekRule = calendarWeekRule;

            return WeekNumberAsString(dateTime);
        }

        public static int GetRemainingDaysInWeek(DateTime date)
        {
            var currentDay = (int)date.DayOfWeek;
            var startDay = (int)_firstDayOfWeek;

            //Add - 1 to exclude the current day
            return 7 - (currentDay % startDay) - (currentDay < startDay ? 1 : 0) - 1;
        }

        public static DateTime GetDateWeekFirstDate(DateTime date)
        {
            //Add 7 - 1 to exclude the current day 
            return date.AddDays(GetRemainingDaysInWeek(date) - (7 - 1));
        }
    }
}
