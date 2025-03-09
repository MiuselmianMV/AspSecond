using AspSecond.Abstract;
using AspSecond.Models;
using System.Text.Json;


namespace AspSecond.Core
{
    public class OpenLibraryService:IOpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
   
        public async Task<BookDto> GetBookByNameAsync(string query)
        {
            var response = await _httpClient.GetAsync($"/api/books?bibkeys=ISBN:{Uri.EscapeDataString(query)}&format=json&jscmd=data");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, BookDto>>(content);
            return data?.Values.FirstOrDefault();
        }
    }
}
