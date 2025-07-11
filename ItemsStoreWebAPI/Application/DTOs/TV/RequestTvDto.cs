namespace ItemsStoreWebAPI.Application.DTOs.TV
{
    public class RequestTvDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ScreenSize { get; set; }
        public string Resolution { get; set; }
        public float Frequency { get; set; }
        public int ReleasedYear { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}