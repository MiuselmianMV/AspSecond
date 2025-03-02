using AspSecond.Abstract;
using AspSecond.DAL.Abstract;
using AspSecond.DAL.Entities;
using AspSecond.Models;
using System.ComponentModel.DataAnnotations;

namespace AspSecond.Core
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        public PersonService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(PersonDto person)
        {
            person.Id = Guid.NewGuid();

            var pr = new Person
            {
                GuidId = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Birthday = person.Birthday,
                OtherInfo = person.OtherInfo
            };
            await _repository.AddAsync(pr);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

        }

        public async Task<List<PersonDto>> GetAllAsync()
        {
            var personsDto = new List<PersonDto>();

            var result = await _repository.GetAllAsync();

            foreach (var person in result)
            {
                personsDto.Add(new PersonDto
                {
                    Birthday = person.Birthday,
                    Name = person.Name,
                    Surname = person.Surname,
                    Id = person.GuidId,
                    OtherInfo = person.OtherInfo
                });
            }

            return personsDto;
        }

        //public async Task<PersonDto> GetByName(string Name)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task UpdateAsync(PersonDto person)
        {
            var pr = new Person
            {
                GuidId = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Birthday = person.Birthday,
                OtherInfo = person.OtherInfo
            };
            
            await _repository.UpdateAsync(pr);
        }
    }
}
