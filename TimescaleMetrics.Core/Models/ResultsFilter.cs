namespace TimescaleMetrics.Core.Models
{
    public class ResultsFilter
    { 
        public string? FileName { get; set; }
        public DateTime? MinDateFrom { get; set; }
        public DateTime? MinDateTo { get; set; }
        public decimal? AvgValueFrom { get; set; }
        public decimal? AvgValueTo { get; set; }
        public decimal? AvgExecutionTimeFrom { get; set; }
        public decimal? AvgExecutionTimeTo { get; set; }
    }
}
