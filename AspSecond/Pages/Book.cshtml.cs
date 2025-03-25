using AspSecond.Abstract;
using AspSecond.Models;
using AspSecond.Optimization;
using Microsoft.AspNetCore.Http;
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
        public BooksContainer BooksFromApi { get; set; } = new BooksContainer();
        public string BooksNotFoundErrorMessage { get; set; } = string.Empty;

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

            var requestCookies = _httpContextAccessor.HttpContext.Request.Cookies;
            var responseCookies = _httpContextAccessor.HttpContext.Response.Cookies;

            bool isNotFirstVisit = requestCookies.ContainsKey("FirstVisit");

            if (isNotFirstVisit)
            {

                var result = BooksFromApi.UpdateBooksFromCookies(_httpContextAccessor);
                if (result == 0)
                {
                    BooksNotFoundErrorMessage = "Невірно введена назва книги";
                }
            }
            else
            {
                responseCookies.Append("FirstVisit", "true", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                });
            }

        }

        public async Task<IActionResult> OnPostAddToDBAsync(BookDto book)
        {

            BooksFromApi.UpdateBooksFromCookies(_httpContextAccessor);
            //foreach (var it in BooksFromApi.Books)
            //{
            //    if (it.Title == book.Title)
            //    {
            //        BooksFromApi.Books.Remove(it);
            //        break;
            //    }
            //}
            BooksFromApi.Books.RemoveAll(it => it.Title == book.Title);
            ClearCookies();
            RewriteCookiesAsync(BooksFromApi.Books);
            await _bookService.AddAsync(book);

            return RedirectToPage("/Book");
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
            BooksNotFoundErrorMessage = string.Empty;

            var data = await _openLibraryService.GetBookByNameAsync(Title);
            if (data != null)
            {
                await SetBooksFromApiInfoCookieAsync(data);
            }
            Title = string.Empty;

            return RedirectToPage("/Book");
        }

        private async Task RewriteCookiesAsync(List<BookDto> books)
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




