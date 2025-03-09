using AspSecond.Models;

namespace AspSecond.Abstract
{
    public interface IOpenLibraryService
    {
        public Task<BookDto> GetBookByNameAsync(string query);
    }
}
