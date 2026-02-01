namespace TimescaleMetrics.DataAccess.Entities
{
    public class ValueEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal ExecutionTime { get; set; }
        public decimal Value { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}
