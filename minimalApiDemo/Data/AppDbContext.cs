using Microsoft.EntityFrameworkCore;

namespace minimalApiDemo.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ToDo> ToDos { get; set; }
    }
}
