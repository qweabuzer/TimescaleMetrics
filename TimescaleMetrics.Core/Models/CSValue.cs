using CSharpFunctionalExtensions;

namespace TimescaleMetrics.Core.Models
{
    public class CSValue
    {
        private static readonly DateTime minDate = new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime maxDate => DateTime.UtcNow;

        private CSValue(Guid id, DateTime date, decimal exectime, decimal value)
        {
            Id = id;
            Date = date;
            ExecutionTime = exectime;
            Value = value;
        }

        public Guid Id { get; }
        public DateTime Date { get; }
        public decimal ExecutionTime { get; }
        public decimal Value { get; }

        public static Result<CSValue> Create(Guid id, DateTime date, decimal execTime, decimal value)
        {
            if(date < minDate)
                return Result.Failure<CSValue>("дата не может быть раньше 01.01.2000");

            if (date > maxDate)
                return Result.Failure<CSValue>("дата не может быть позже текущей");

            if (execTime < 0)
                return Result.Failure<CSValue>("время выполнения не может быть меньше 0");

            if (value < 0)
                return  Result.Failure<CSValue>("значение показателя не может быть меньше 0");

            var csvalue = new CSValue(
                id,
                date,
                execTime,
                value);

            return Result.Success<CSValue>(csvalue);
        }
    }
}
