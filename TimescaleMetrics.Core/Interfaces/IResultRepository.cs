using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Core.Interfaces
{
    public interface IResultRepository
    {
        Task<bool> ExistsFileAsync(string fileName);
        Task DeleteResultsAsync(string fileName);
        Task<IntegrationResults> AddOrUpdateAsync(string fileName, IntegrationResults result);
        Task<List<IntegrationResults>> GetFilteredResultsAsync(ResultsFilter filter);
    }
}
