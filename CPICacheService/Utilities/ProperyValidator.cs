using System.Text.RegularExpressions;

namespace CPICacheService.Utilities
{
    public static class ProperyValidator
    {
        public static bool IsValidSeriesIdFormat(string value)
        {
            // The series ID(s) can include underscore (_), dash (-) and hash (#)
            // but must not include lower case letters or special characters.
            return string.IsNullOrEmpty(value)
                && Regex.IsMatch(value, @"^[A-Z0-9_\-#]+$"); ;
        }

        public static bool IsValidYear(int value)
        {
            // Consumer Price Index (CPI) has data available from
            // as early as 1913, which is the inception of the CPI.
            // The CPI has no data for future years.
            return value >= 1913 && value <= DateTime.Now.Year;
        }

        public static bool IsValidMonth(int value)
        {
            // January = 1, ..., December = 12
            return value >= 1 && value <= 12;
        }
    }
}
