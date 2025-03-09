using System.ComponentModel.DataAnnotations;

namespace AspSecond.Models
{
    public class BookDto
    {
        public Guid Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Author { get; set; }
        
        [Required]
        public string? Style { get; set; }
        
        [Required]
        public int? PublicationDate { get; set; }
        public string? OtherInfo { get; set; }
    }
}
