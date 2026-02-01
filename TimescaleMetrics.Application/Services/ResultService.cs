using CSharpFunctionalExtensions;
using CsvHelper;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Application.Services
{
    public class ResultService : IResultService
    {

        private readonly IResultRepository _resultRepository;

        public ResultService(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }
        public async Task<Result<List<IntegrationResults>>> GetFilterResultsAsync(ResultsFilter filter)
        {
            var results = await _resultRepository.GetFilteredResultsAsync(filter);

            if (results == null)
                return Result.Failure<List<IntegrationResults>>("по заданным фильтрам ничего не найдено");

            return Result.Success(results);
        }
    }
}
