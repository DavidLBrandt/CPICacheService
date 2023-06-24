using CPICacheService.Models;

namespace CPICacheService.Utilities
{
    public interface IRepository
    {
        Task<Cpi> GetCpi(string seriesId, string year, string month);
    }
}