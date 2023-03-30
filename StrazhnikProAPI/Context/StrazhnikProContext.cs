using Microsoft.EntityFrameworkCore;
using StrazhnikProAPI.Models;

namespace StrazhnikProAPI.Context;

public class StrazhnikProContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public StrazhnikProContext(DbContextOptions opts) : base(opts)
    {
    }

    public StrazhnikProContext()
    {
        
    }

    public async Task AddUserAsync(User user)
    {
        await Users.AddAsync(user);

        await SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;" +
                                "password=root;database=strazhnikpro;",
            new MySqlServerVersion(new Version(5, 7, 24)));
        
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Указываем контексту, что между таблицами User и Group есть связанные записи
        //которые должны храниться в указанных свойствах классов
        //также указываем свойство, которое будет хранить внешний ключ
        modelBuilder.Entity<User>()
            .HasOne<Group>(u => u.Group)
            .WithMany(g => g.Users)
            .HasForeignKey(u => u.GroupId);
    }
}