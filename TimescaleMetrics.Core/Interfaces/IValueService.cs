using CSharpFunctionalExtensions;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.Core.Interfaces
{
    public interface IValueService
    {
        Task<Result<FileData>> ParseFileAsync(string fileName, Stream csvStream);

        Task<Result<IEnumerable<CSValue>>> GetLastValuesAsync(string fileName, int count = 10);
    }
}
