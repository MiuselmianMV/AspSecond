using AspSecond.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspSecond.DAL.Abstract
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(Guid id);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(Guid id);
    }
}
