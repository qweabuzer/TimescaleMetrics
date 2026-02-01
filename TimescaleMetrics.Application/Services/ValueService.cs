using CSharpFunctionalExtensions;
using System.Collections.Generic;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Application.Services
{
    public class ValueService : IValueService
    {
        
        private readonly ICsvParserService _csvParser;
        private readonly IValueRepository _valueRepository;
        private readonly IResultRepository _resultRepository;

        public ValueService(ICsvParserService csvParser, IValueRepository valueRepository, IResultRepository resultRepository)
        {
            _csvParser = csvParser;
            _valueRepository = valueRepository;
            _resultRepository = resultRepository;
        }

        public async Task<Result<FileData>> ParseFileAsync(string fileName, Stream csvStream)
        {
            var parseResult = await _csvParser.ParseAsync(csvStream, fileName);

            if (parseResult.IsFailure)
                return Result.Failure<FileData>(parseResult.Error);

            var values = parseResult.Value;


            bool fileExists = await _valueRepository.FileExistsAsync(fileName);

            try
            {
                if (fileExists)
                    await _valueRepository.DeleteValuesAsync(fileName);

                await _valueRepository.AddValuesAsync(values, fileName);

                var integrationResult = IntegrateValues(values);

                await _resultRepository.AddOrUpdateAsync(fileName, integrationResult);

                return Result.Success(new FileData
                {
                    FileName = fileName,
                    Lines = values.Count,
                    IntegrationResults = integrationResult
                });
            }
            catch (Exception ex)
            {
                return Result.Failure<FileData>($"ошибка сохранения данных: {ex.Message}");
            }
        }

        private IntegrationResults IntegrateValues(IReadOnlyList<CSValue> values)
        {
            var dates = values.Select(v => v.Date).ToList();
            var executionTimes = values.Select(v => v.ExecutionTime).ToList();
            var dataValues = values.Select(v => v.Value).ToList();

            return new IntegrationResults
            {
                TimeDelta = (decimal)(dates.Max() - dates.Min()).TotalSeconds,
                MinDate = dates.Min(),
                AvgExecutionTime = executionTimes.Average(),
                AvgValue = dataValues.Average(),
                MedianValue = CalculateMedian(dataValues),
                MaxValue = dataValues.Max(),
                MinValue = dataValues.Min()
            };
        }

        private decimal CalculateMedian(List<decimal> values)
        {
            var sortedValues = values.OrderBy(v => v).ToList();
            int count = sortedValues.Count;

            if (count % 2 == 0)
            {
                return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2;
            }
            else
            {
                return sortedValues[count / 2];
            }
        }

        public async Task<Result<IEnumerable<CSValue>>> GetLastValuesAsync(string fileName, int count = 10)
        {
            var values = await _valueRepository.GetLastValuesAsync(fileName, count);

            if (values == null)
                return Result.Failure<IEnumerable<CSValue>>("по заданным фильтрам значений нет");

            return Result.Success(values);
        }
    }
}     