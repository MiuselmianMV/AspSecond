using AspSecond.Models;

namespace AspSecond.Abstract
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllAsync();
        //Task<BookDto> GetByName(string Name);
        Task AddAsync(BookDto book);
        Task UpdateAsync(BookDto book);
        Task DeleteAsync(Guid id);
    }
}
