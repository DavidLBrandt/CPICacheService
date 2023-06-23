using System.Globalization;
using System.Text.RegularExpressions;

namespace CPICacheService.Utilities
{
    public class PropertyValidator : IPropertyValidator
    {
        public bool IsValidSeriesIdFormat(string value)
        {
            // The series ID(s) can include underscore (_), dash (-) and hash (#)
            // but must not include lower case letters or special characters.
            return !string.IsNullOrEmpty(value)
                && Regex.IsMatch(value, @"^[A-Z0-9_\-#]+$");
        }

        public bool IsValidYear(string value)
        {
            // Consumer Price Index (CPI) has data available from
            // as early as 1913, which is the inception of the CPI.
            // The CPI has no data for future years.

            // Check if the string length is exactly 4
            if (value.Length != 4)
                return false;

            // Parse the string into an integer
            if (!int.TryParse(value, out int year))
                return false;

            // Get the current year
            int currentYear = DateTime.Now.Year;

            // Validate the year range
            if (year < 1913 || year > currentYear)
                return false;

            return true;
        }

        public bool IsValidMonth(string value)
        {
            string[] formats = { "MMMM" };
            return DateTime.TryParseExact(
                value,
                formats,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out DateTime dummyDate);
        }
    }
}
