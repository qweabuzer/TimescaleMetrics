using CSharpFunctionalExtensions;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Core.Interfaces
{
    public interface IResultService
    {
        Task<Result<List<IntegrationResults>>> GetFilterResultsAsync(ResultsFilter filter);
    }
}
