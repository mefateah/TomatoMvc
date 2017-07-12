using System;

namespace MvcTomato.Helpers
{
    public static class TimeFormatter
    {
        public static string FormatTimeSpan(TimeSpan? time)
        {
            if (!time.HasValue)
            {
                return string.Empty;
            }
            var sign = time < TimeSpan.Zero ? "+" : "-";
            return $"{sign}{time:hh\\:mm}";
        } 
    }
}