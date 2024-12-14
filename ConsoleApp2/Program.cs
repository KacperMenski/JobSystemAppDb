
using Domain;
using JobSystemApp.Data;
using JobSystemApp.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using UserManagementApp;


namespace JobSystemApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=JobSystemDb;Username=postgres;Password=123");

            using var context = new ApplicationDbContext(optionsBuilder.Options);

            
            var jobInterface = new Interface.Interface(context);
            jobInterface.Menu();
        }
    }
}
