namespace TimescaleMetrics.Core.Models
{
    public class FileData
    {
        public string FileName { get; set; } = string.Empty;
        public int Lines { get; set; }
        public IntegrationResults IntegrationResults { get; set; } = null!;
    }
}