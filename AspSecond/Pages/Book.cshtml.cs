using AspSecond.Abstract;
using AspSecond.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string BooksNotFoundErrorMessage { get; set; } = string.Empty;

        private readonly IBookService _bookService;
        private readonly IOpenLibraryService _openLibraryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? cookieValue;

        public BookModel(IBookService bookService, IOpenLibraryService openLibraryService, IHttpContextAccessor httpContextAccessor)
        {
            _bookService = bookService;
            _openLibraryService = openLibraryService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGet()
        {
            //IsFoundBooksLst = true;
            Books = await _bookService.GetAllAsync();
            cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["BooksFromApi"] ?? string.Empty;
            if (cookieValue != string.Empty && cookieValue != "[]")
            {
                BooksFromApi = JsonSerializer.Deserialize<List<BookDto>>(cookieValue) ?? new List<BookDto>();
            }
            else
            {
                BooksNotFoundErrorMessage = "Невірно введена назва книги";
            }
        }

        public async Task<IActionResult> OnPostAddToDBAsync(BookDto book)
        {
            cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["BooksFromApi"] ?? string.Empty;
            BooksFromApi = JsonSerializer.Deserialize<List<BookDto>>(cookieValue) ?? new List<BookDto>();
            foreach (var it in BooksFromApi)
            {
                if (it.Title == book.Title)
                {
                    BooksFromApi.Remove(it);
                    break;
                }
            }
            ClearCookies();
            RewriteCookiesAsync(BooksFromApi);
            await _bookService.AddAsync(book);

            return RedirectToPage("/Book");
        }

        public async Task<IActionResult> OnPostFirstAsync()
        {
            BooksNotFoundErrorMessage = string.Empty;
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
            BooksNotFoundErrorMessage = string.Empty;

            var data = await _openLibraryService.GetBookByNameAsync(Title);
            if (data != null)
            {
                await SetBooksFromApiInfoCookieAsync(data);
            }
            Title = string.Empty;

            return RedirectToPage("/Book");
        }

        private async void RewriteCookiesAsync(List<BookDto> books)
        {
            string booksJson = JsonSerializer.Serialize(books);

            await Task.Run(() =>
              _httpContextAccessor.HttpContext.Response.Cookies.Append("BooksFromApi", booksJson, new CookieOptions
              {
                  HttpOnly = true,
                  Secure = true,
                  Expires = DateTime.UtcNow.AddHours(1)
              }));
        }

        private void ClearCookies()
        {
            var responseCookies = _httpContextAccessor.HttpContext.Response.Cookies;
            responseCookies.Delete("BooksFromApi");
        }

        public async Task SetBooksFromApiInfoCookieAsync(List<BookDto> booksLst)
        {
            var requestCookies = _httpContextAccessor.HttpContext.Request.Cookies;
            var responseCookies = _httpContextAccessor.HttpContext.Response.Cookies;

            if (booksLst.Count == 0)
            {
                BooksNotFoundErrorMessage = "Невірно введена назва книги";
                if (requestCookies.ContainsKey("BooksFromApi"))
                {
                    responseCookies.Delete("BooksFromApi");
                }
                //IsFoundBooksLst = false;
                return;
            }

            string booksJson = JsonSerializer.Serialize(booksLst);

            if (requestCookies.ContainsKey("BooksFromApi"))
            {
                responseCookies.Delete("BooksFromApi");
            }
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




