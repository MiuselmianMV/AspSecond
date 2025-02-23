using AspSecond.DAL.Abstract;
using AspSecond.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspSecond.DAL.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(Guid id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task UpdateAsync(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }
    }
}
