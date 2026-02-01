using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Core.Interfaces
{
    public interface IValueRepository
    {
        Task<bool> FileExistsAsync(string fileName);
        Task DeleteValuesAsync(string fileName);
        Task AddValuesAsync(IEnumerable<CSValue> values, string fileName);
        Task<IEnumerable<CSValue>> GetLastValuesAsync(string fileName, int count = 10);
    }
}
