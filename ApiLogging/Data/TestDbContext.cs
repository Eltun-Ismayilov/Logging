using ApiLogging.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiLogging.Data
{
    public class TestDbContext: DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Test> Tests { get; set; }
    }
}
