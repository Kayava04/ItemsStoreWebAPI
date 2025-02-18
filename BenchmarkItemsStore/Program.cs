namespace BenchmarkItemsStore
{
    public class Program
    {
        private static string PATH = Directory.GetCurrentDirectory()[..^16] + @"\Logs\logs.txt";
        
        static async Task Main(string[] args)
        {
            var storePerformance = new StorePerformance();
            // await storePerformance.RunAsync(nameof(storePerformance.AddItemsAsync), tvCount: 10);
            
            await SearchLogs("id");
        }
        
        private static async Task SearchLogs(string keyWord)
        {
            if (!File.Exists(PATH))
            {
                Console.WriteLine("Log file not found.");
                return;
            }

            try
            {
                await using var stream = new FileStream(PATH, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();
                var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var results = lines.Where(line => line.Contains(keyWord, StringComparison.OrdinalIgnoreCase));
        
                Console.WriteLine("Search results:");
                
                foreach (var result in results)
                    Console.WriteLine(result);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
    }
}