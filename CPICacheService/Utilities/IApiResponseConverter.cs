using CPICacheService.Models;

namespace CPICacheService.Utilities
{
    public interface IApiResponseConverter
    {
        List<ApiResponse> ConvertFromJson(string json);
    }
}