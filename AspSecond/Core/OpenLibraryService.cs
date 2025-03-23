using AspSecond.Abstract;
using AspSecond.Models;
using System.Text.Json;


namespace AspSecond.Core
{
    public class OpenLibraryService : IOpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookDto>> GetBookByNameAsync(string query = "")
        {
            var response = await _httpClient.GetAsync($"/search.json?q={query.Replace(" ", "+")}&format=json&jscmd=data");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            //var data = JsonSerializer.Deserialize<BookDto>(content);

            var data = ExtractBooks(content);
            return data;
        }

        public List<BookDto> ExtractBooks(string json)
        {
            var books = new List<BookDto>();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var document = JsonDocument.Parse(json);
            JsonElement contentArray = new JsonElement();
            var root = document.RootElement;
            if (!root.TryGetProperty("docs", out contentArray)
                || contentArray.ValueKind != JsonValueKind.Array
                || contentArray.GetArrayLength() == 0)
            {
                return new List<BookDto>();
            }
            
            for (int i = 0; i < 10; i++)
            {
                var firstContent = contentArray[i];

                var author_name = firstContent.TryGetProperty("author_name", out var auth_name)
                                  ? auth_name[0].GetRawText() : string.Empty;

                var publish_year = firstContent.TryGetProperty("first_publish_year", out var year)
                                   ? int.Parse(year.GetRawText()) : 0;

                var title = firstContent.TryGetProperty("title", out var bookTitle)
                            ? bookTitle.GetRawText() : string.Empty;

                var result = new BookDto()
                {
                    Title = title,
                    Author_name = author_name,
                    First_publish_year = publish_year,
                };
                books.Add(result);
            }

            return books;
        }
    }
}
