using MantisCode.EmailSender.Enums;
using MantisCode.EmailSender.Interfaces;
using MantisCode.EmailSender.Repositories;
using MantisCode.EmailSender.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MantisCode.EmailSender;

public static class Extensions
{
    public static IServiceCollection SmtpClientBuilder(this IServiceCollection services, SmtpSettings smtpSettings, string connectionString, DatabaseProviderEnum databaseProvider = DatabaseProviderEnum.SqlServer )
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

        services.AddScoped<IEmailDictionaryCommandRepository, EmailDictionaryCommandRepository>();
        services.AddScoped<IEmailDictionaryQueryRepository, EmailDictionaryQueryRepository>();
        services.AddScoped<ISmtpService, SmtpService>();

        SmtpClientOptions.SmtpSettings = smtpSettings;
        SmtpClientOptions.DatabaseProvider = databaseProvider;
        SmtpClientOptions.ConnectionString = connectionString;

        return services;
    }

    public static IServiceCollection SmtpClientBuilderSingleton(this IServiceCollection services, SmtpSettings smtpSettings, string connectionString, DatabaseProviderEnum databaseProvider = DatabaseProviderEnum.SqlServer)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

        services.AddSingleton<IEmailDictionaryCommandRepository, EmailDictionaryCommandRepository>();
        services.AddSingleton<IEmailDictionaryQueryRepository, EmailDictionaryQueryRepository>();
        services.AddSingleton<ISmtpService, SmtpService>();

        SmtpClientOptions.SmtpSettings = smtpSettings;
        SmtpClientOptions.DatabaseProvider = databaseProvider;
        SmtpClientOptions.ConnectionString = connectionString;

        return services;
    }
}
