using CSharpFunctionalExtensions;
using System.Globalization;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Application.Services
{
    public class CsvParserService : ICsvParserService
    {
        public async Task<Result<List<CSValue>>> ParseAsync(Stream stream, string fileName)
        {
            var values = new List<CSValue>();

            using var reader = new StreamReader(stream);

            await reader.ReadLineAsync();

            string line;
            int lineCount = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                lineCount++;

                if (lineCount > 10000)
                    return Result.Failure<List<CSValue>>("файл содержит более 10000 строк. максимум: 10000");

                var parseResult = ParseLine(line, lineCount);

                if (parseResult.IsFailure)
                    return Result.Failure<List<CSValue>>(parseResult.Error);

                values.Add(parseResult.Value);
            }

            if (lineCount == 0)
                return Result.Failure<List<CSValue>>("файл должен содержать хотя бы одну строку данных");

            return Result.Success(values);
        }

        private Result<CSValue> ParseLine(string line, int lineNumber)
        {
            try
            {
                var parts = line.Split(';');

                if (parts.Length != 3)
                {
                    return Result.Failure<CSValue>($"ожидается 3 колонки с данными. получено {parts.Length}");
                }

                var date = DateTime.ParseExact(
                    parts[0].Trim(),
                    "yyyy-MM-dd'T'HH-mm-ss.ffff'Z'",
                    CultureInfo.InvariantCulture)
                    .ToUniversalTime();

                var execTime = decimal.Parse(
                    parts[1].Trim(),
                    CultureInfo.InvariantCulture);

                var value = decimal.Parse(
                    parts[2].Trim(),
                    CultureInfo.InvariantCulture);

                return CSValue.Create(Guid.NewGuid(), date, execTime, value);
            }
            catch (Exception ex)
            {
                return Result.Failure<CSValue>($"строка {lineNumber}: ошибка обработки - {ex.Message}");
            }
        }
    }
}
