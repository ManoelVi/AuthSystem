using Microsoft.EntityFrameworkCore;
using AuthSystem.API.Models;

namespace AuthSystem.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da tabela User
        modelBuilder.Entity<User>(entity =>
        {
            // Email deve ser único
            entity.HasIndex(u => u.Email).IsUnique();

            // Tamanho máximo dos campos
            entity.Property(u => u.Name).HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.Property(u => u.PasswordHash).HasMaxLength(255);
        });
    }
}
