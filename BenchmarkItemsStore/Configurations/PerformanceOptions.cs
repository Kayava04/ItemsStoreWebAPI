namespace BenchmarkItemsStore.Configurations
{
    public class PerformanceOptions
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public StockApiOptions StockApi { get; set; } = new();
    }

    public class StockApiOptions
    {
        public string Protocol { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string BasePath { get; set; }
    }
}