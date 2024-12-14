using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using JobSystemApp.Data;

namespace JobSystemApp.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=JobSystemDb;Username=postgres;Password=123");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
