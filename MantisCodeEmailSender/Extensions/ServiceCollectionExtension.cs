using MantisCodeEmailSender.EntityFramework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MantisCodeEmailSender.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection CreateEmailLogger(this IServiceCollection services, string connectionString, string baseLocation)
    {
        try
        {
            McEmailSenderDbContextFactory.ConnectionString = connectionString;
            McEmailSenderDbContext._connectionString = connectionString;
            //McEmailSenderDbContext.ProjectDirectory = baseLocation;
            //services.AddDbContext<McEmailSenderDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            RunMigrationAddCommand("AddEmailLogTable", baseLocation);

            services.AddDbContext<McEmailSenderDbContext>(options =>
                options.UseNpgsql(connectionString));
            //RunMigrationUpdateCommand(baseLocation);

            using var serviceProvider = services.BuildServiceProvider();
            

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<McEmailSenderDbContext>();

                dbContext.Database.Migrate();
            }
            serviceProvider.Dispose();

            return services;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return services;
        }
    }

    private static void RunMigrationAddCommand(string migrationName, string projectDirectory)
    {
        string command = "dotnet";
        string arguments = $"ef migrations add {migrationName}";

        ProcessStartInfo psi = new()
        {
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = projectDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            using Process? process = Process.Start(psi);

            if (process == null)
            {
                Console.WriteLine("Process could not be started. Please verify that the command and working directory are correct.");
                return;
            }

            process.WaitForExit();
            string output = process!.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            Console.WriteLine("Migrations Add Output:");
            Console.WriteLine(output);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("Migrations Add Errors:");
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while running the migration command: {ex.Message}");
        }
    }

    public static void RunMigrationUpdateCommand(string projectDirectory)
    {
        string command = "dotnet";
        string arguments = $"ef database update";

        ProcessStartInfo psi = new()
        {
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = projectDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            using Process? process = Process.Start(psi);

            if (process == null)
            {
                Console.WriteLine("Process could not be started. Please verify that the command and working directory are correct.");
                return;
            }

            process.WaitForExit();
            string output = process!.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine("Migrations Update Output:");
            Console.WriteLine(output);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("Migrations Update Errors:");
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while running the migration command: {ex.Message}");
        }
    }
}
