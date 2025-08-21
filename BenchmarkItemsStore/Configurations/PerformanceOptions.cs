namespace BenchmarkItemsStore.Configurations
{
    public class PerformanceOptions
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public BaseUrlsOptions BaseUrls { get; set; }
    }

    public class BaseUrlsOptions
    {
        public string TV { get; set; }
        public string Mobile { get; set; }
    }
}