using AspSecond.Abstract;
using AspSecond.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspSecond.Pages
{
    public class PersonModel : PageModel
    {
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

    }
}
