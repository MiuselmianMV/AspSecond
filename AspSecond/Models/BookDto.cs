using System.ComponentModel.DataAnnotations;

namespace AspSecond.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Author_name { get; set; }
        
        [Required]
        public string? Style { get; set; }
        
        [Required]
        public int? First_publish_year { get; set; }
        public string? OtherInfo { get; set; }
    }
}
