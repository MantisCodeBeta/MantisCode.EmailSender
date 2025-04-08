using Microsoft.Data.SqlClient;
using Dapper;
using MantisCode.EmailSender.Models;
using MantisCode.EmailSender.Interfaces;
using Npgsql;
using MantisCode.EmailSender.Enums;

namespace MantisCode.EmailSender.Repositories;

internal class EmailDictionaryQueryRepository : IEmailDictionaryQueryRepository
{
    public async Task<EmailDictionaries?> GetEmailDictionaryByIdAsync(int id)
    {
        try
        {
            string query;

            switch (SmtpClientOptions.DatabaseProvider)
            {
                case DatabaseProviderEnum.SqlServer:
                {
                    query = $"""
                             SELECT Id,
                                    Body,
                                    BodyAsHtml,
                                    Subject,
                                    ReplacementQuantity,
                                    IsActive
                             FROM EmailDictionaries ed WITH (NOLOCK)
                             WHERE ed.Id = @Id
                             AND ed.IsActive = 1
                             """;

                    await using SqlConnection connection = new(SmtpClientOptions.ConnectionString);
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = id });
                }
                case DatabaseProviderEnum.PostgreSql:
                {
                    query = """
                            SELECT ed."Id",
                                   ed."Body",
                                   ed."BodyAsHtml",
                                   ed."Subject",
                                   ed."ReplacementQuantity",
                                   ed."IsActive"
                            FROM public."EmailDictionaries" AS ed
                            WHERE ed."Id" = @Id
                            AND ed."IsActive" = true
                            """;
                    
                    await using NpgsqlConnection connection = new(SmtpClientOptions.ConnectionString);
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = id });
                }
                default:
                    throw new NotSupportedException("Unsupported database provider.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> IsDictionaryTableCreatedAsync()
    {
        string query;

        switch (SmtpClientOptions.DatabaseProvider)
        {
            case DatabaseProviderEnum.SqlServer:
            {
                query = """
                            SELECT TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            WHERE TABLE_NAME = 'EmailDictionaries'
                        """;

                await using SqlConnection connection = new(SmtpClientOptions.ConnectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
            }
            case DatabaseProviderEnum.PostgreSql:
            {
                query = """
                            SELECT tablename
                            FROM pg_catalog.pg_tables
                            WHERE tablename = 'emaildictionaries'
                        """;

                await using NpgsqlConnection connection = new(SmtpClientOptions.ConnectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
            }
            default:
                throw new NotSupportedException("Unsupported database provider.");
        }
    }
}
