namespace TimescaleMetrics.DataAccess.Entities
{
    public class ResultEntity
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public decimal DeltaTime { get; set; }
        public DateTime MinDate { get; set; }
        public decimal AvgExecutionTime { get; set; }
        public decimal AvgValue { get; set; }
        public decimal MedianValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
    }
}
