using AspSecond.Models;

namespace AspSecond.Abstract
{
    public interface IPersonService
    {
        Task<List<PersonDto>> GetAllAsync();
        //Task<PersonDto> GetByName(string Name);
        Task AddAsync(PersonDto person);
        Task UpdateAsync(PersonDto person);
        Task DeleteAsync(Guid id);
    }
}
