using AspSecond.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace AspSecond.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    }
}
