using AspSecond.Models;
using System.Text.Json;

namespace AspSecond.Optimization
{
    public class BooksContainer
    {
        public List<BookDto> Books { get; set; } = new List<BookDto>();
        public BooksContainer() { }

        public int UpdateBooksFromCookies(IHttpContextAccessor httpContextAccessor)
        {
            var cookieValue = httpContextAccessor.HttpContext.Request.Cookies["BooksFromApi"] ?? string.Empty;

            if (cookieValue != string.Empty && cookieValue != "[]")
            {
                Books = JsonSerializer.Deserialize<List<BookDto>>(cookieValue) ?? new List<BookDto>();
                return Books.Count;
            }
            else
            {
                return 0;
            }
        }
        
    }
}
