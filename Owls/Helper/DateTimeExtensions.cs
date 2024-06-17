using System.Globalization;

namespace Owls.Helper
{
    public static class DateTimeExtensions
    {
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            Calendar cal = CultureInfo.InvariantCulture.Calendar;

            DayOfWeek day = cal.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
