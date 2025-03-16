using AspSecond.Abstract;
using AspSecond.Core;
using AspSecond.DAL.Entities;
using AspSecond.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AspSecond.Pages
{
    public class BookModel : PageModel
    {
        [BindProperty]
        public BookDto Book { get; set; } = new BookDto();

        [BindProperty]
        public string Title { get; set; } = string.Empty;
        public List<BookDto> Books { get; set; } = new List<BookDto>();
        public List<BookDto> BooksFromApi { get; set; } = new List<BookDto>();

        private readonly IBookService _bookService;
        private readonly IOpenLibraryService _openLibraryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookModel(IBookService bookService, IOpenLibraryService openLibraryService, IHttpContextAccessor httpContextAccessor)
        {
            _bookService = bookService;
            _openLibraryService = openLibraryService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGet()
        {
            Books = await _bookService.GetAllAsync();
         
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["BooksFromApi"] ?? string.Empty;
            if (cookieValue != string.Empty )
            {
                BooksFromApi = JsonSerializer.Deserialize<List<BookDto>>(cookieValue) ?? new List<BookDto>();

            }
        }

        public async Task<IActionResult> OnPostFirstAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await _bookService.AddAsync(Book);

            
            return RedirectToPage("/Book");
        }

        public async Task<IActionResult> OnPostSecondAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var data = await _openLibraryService.GetBookByNameAsync(Title);
            if (data != null)
            {
                await SetBooksFromApiInfoCookieAsync(data);
            }
            Title = string.Empty;

            return RedirectToPage("/Book");
        }

        public async Task SetBooksFromApiInfoCookieAsync(List<BookDto> booksLst)
        {


            string booksJson = JsonSerializer.Serialize(booksLst);

            await Task.Run(() =>
              _httpContextAccessor.HttpContext.Response.Cookies.Append("BooksFromApi", booksJson, new CookieOptions
              {
                  HttpOnly = true,
                  Secure = true,
                  Expires = DateTime.UtcNow.AddHours(1)
              })
            );
        }

    }
}




