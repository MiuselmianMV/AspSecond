using AspSecond.Abstract;
using AspSecond.DAL.Abstract;
using AspSecond.DAL.Entities;
using AspSecond.DAL.Migrations;
using AspSecond.Models;

namespace AspSecond.Core
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task AddAsync(BookDto book)
        {
            book.Id = Guid.NewGuid();

            var bk = new Book
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author_name,
                Style = book.Style,
                PublicationDate = book.First_publish_year,
                OtherInfo = book.OtherInfo
            };
            
            await _bookRepository.AddAsync(bk);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        } 

        public async Task<List<BookDto>> GetAllAsync()
        {
            var booksDto = new List<BookDto>();

            var result = await _bookRepository.GetAllAsync();

            foreach (var book in result)
            {
                booksDto.Add(new BookDto 
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author_name = book.Author,
                    Style = book.Style,
                    First_publish_year = book.PublicationDate,
                    OtherInfo = book.OtherInfo
                });
            }

            return booksDto;
        }

        //public async Task<BookDto> GetByName(string Name)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task UpdateAsync(BookDto book)
        {
            book.Id = Guid.NewGuid();

            var bk = new Book
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author_name,
                Style = book.Style,
                PublicationDate = book.First_publish_year,
                OtherInfo = book.OtherInfo
            };
            await _bookRepository.UpdateAsync(bk);
        }
    }
}
