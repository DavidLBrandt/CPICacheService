namespace CPICacheService.Utilities
{
    public interface IPropertyValidator
    {
        bool IsValidMonth(string value);
        bool IsValidSeriesIdFormat(string value);
        bool IsValidYear(string value);
    }
}