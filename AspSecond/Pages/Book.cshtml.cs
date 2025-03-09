using AspSecond.Abstract;
using AspSecond.Core;
using AspSecond.DAL.Entities;
using AspSecond.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspSecond.Pages
{
    public class BookModel : PageModel
    {
        [BindProperty]
        public BookDto Book { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();

        private readonly IBookService _bookService;
        //private readonly IOpenLibraryService _openLibraryService;

        public BookModel(IBookService bookService, IOpenLibraryService openLibraryService)
        {
            _bookService = bookService;
            //_openLibraryService = openLibraryService;
        }

        public async Task OnGet()
        {
            Books = await _bookService.GetAllAsync();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Request.BodyReader.ToString().Contains("Form1Submitted"))
            {
                await _bookService.AddAsync(Book);
            }
            else if (Request.BodyReader.ToString().Contains("Form2Submitted"))
            {
                var data = await _openLibraryService.GetBookByNameAsync("");
            }
            return RedirectToPage("/Book");
        }
    }
}
