using CSharpFunctionalExtensions;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Core.Interfaces
{
    public interface ICsvParserService
    {
        Task<Result<List<CSValue>>> ParseAsync(Stream stream, string fileName);
    }
}
