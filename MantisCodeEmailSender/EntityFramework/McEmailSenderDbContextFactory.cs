using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MantisCodeEmailSender.EntityFramework;

public class McEmailSenderDbContextFactory : IDesignTimeDbContextFactory<McEmailSenderDbContext>
{
    public static string ConnectionString { get; set; }

    public McEmailSenderDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"Connection String In Factory \n{ConnectionString}");
        var optionsBuilder = new DbContextOptionsBuilder<McEmailSenderDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        return new McEmailSenderDbContext(optionsBuilder.Options);
    }
}
