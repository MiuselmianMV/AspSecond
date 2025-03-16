using AspSecond.Models;

namespace AspSecond.Abstract
{
    public interface IOpenLibraryService
    {
        public Task<List<BookDto>> GetBookByNameAsync(string query);
        public List<BookDto> ExtractBooks(string json);

    }
}
