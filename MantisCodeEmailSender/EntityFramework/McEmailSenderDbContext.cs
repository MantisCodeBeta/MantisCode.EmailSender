using MantisCodeEmailSender.EntityFramework.Configurations;
using MantisCodeEmailSender.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace MantisCodeEmailSender.EntityFramework;

public class McEmailSenderDbContext(DbContextOptions<McEmailSenderDbContext> options) : DbContext(options)
{
    public static string _connectionString = string.Empty;

    public DbSet<EmailLogs> EmailLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine($"Connection String \n{_connectionString}");
        optionsBuilder.UseNpgsql(_connectionString, b => b.MigrationsAssembly("MantisCodeEmailSender"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmailLogConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
