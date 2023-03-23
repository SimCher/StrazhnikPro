using Microsoft.EntityFrameworkCore;
using WebTopOdin.Models;

namespace WebTopOdin.Context;

public class StrazhnikProContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public StrazhnikProContext(DbContextOptions opts) : base(opts)
    {
    }

    public StrazhnikProContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;" +
                                "password=root;database=strazhnikpro;",
            new MySqlServerVersion(new Version(5, 7, 24)));
    }
}