using AspSecond.Abstract;
using AspSecond.Core;
using AspSecond.DAL;
using AspSecond.DAL.Abstract;
using AspSecond.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;


namespace AspSecond
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var conString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=School;Integrated Security=True;Connect Timeout=30;";
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conString));

            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddHttpContextAccessor();
            

            builder.Services.AddHttpClient<IOpenLibraryService, OpenLibraryService>(client =>
            {
                client.BaseAddress = new Uri("https://openlibrary.org");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });


            var app = builder.Build();

            //using ()

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
