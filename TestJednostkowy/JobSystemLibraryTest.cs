using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSystemApp;
using JobSystemApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManagementApp;


namespace TestJednostkowy
{

    public class JobSystemLibraryTest
    {
        [Fact]
        public void IsAdmin_ShouldReturnTrueIfUserIsAdmin()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=JobSystemDb;Username=postgres;Password=123");
            using var context = new ApplicationDbContext(optionsBuilder.Options);
            UserManager userManager = new UserManager(context);
            var user = new User
            {
                Id = 999,
                Username = "Test",
                Password = "Test",
                Role = "admin"
            };

            var result = userManager.IsAdmin(user);

            Assert.True(result);
        }
    }
}
