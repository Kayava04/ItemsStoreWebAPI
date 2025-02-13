using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BenchmarkItemsStore.Entities
{
    public class LogEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public long Timestamp { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Message { get; set; }
    }
}