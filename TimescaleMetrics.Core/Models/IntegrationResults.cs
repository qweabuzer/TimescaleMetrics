namespace TimescaleMetrics.Core.Models
{
    public class IntegrationResults
    {
        public decimal TimeDelta { get; set; }
        public DateTime MinDate { get; set; }
        public decimal AvgExecutionTime { get; set; }
        public decimal AvgValue { get; set; }
        public decimal MedianValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
    }
}
