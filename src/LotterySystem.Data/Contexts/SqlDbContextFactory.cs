using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LotterySystem.Data.Contexts
{
    public class SqlDbContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
    {
        public SqlDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return new SqlDbContext(
                new DbContextOptionsBuilder<SqlDbContext>()
                    .UseSqlServer(config["DatabaseConnection"])
                    .Options);
        }
    }
}
