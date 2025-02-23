using AspSecond.Abstract;
using AspSecond.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspSecond.Pages
{
    public class PersonModel : PageModel
    {
        [BindProperty]
        public PersonDto Person { get; set; }
        public List<PersonDto> Persons { get; set; }

        private readonly IPersonService _personService;

        public PersonModel(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task OnGetAsync()
        {
            Persons = await _personService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _personService.AddAsync(Person);

            return RedirectToPage("/Person");

            //Persons = await _personService.GetAllAsync();
        }
    }
}
