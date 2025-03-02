using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspSecond.Pages
{
    public class EventModel : PageModel
    {
        [BindProperty]
        public string Text { get; set; }

        public void OnGet()
        {
        }
        
        public void OnPost(string text)
        {
            Text = text;
        }
    }
}
