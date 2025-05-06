namespace ItemsStoreWebAPI.DTOs
{
    public class RequestTvDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Size { get; set; }
        public string Resolution { get; set; }
        public float Frequency { get; set; }
        public int ReleasedYear { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}