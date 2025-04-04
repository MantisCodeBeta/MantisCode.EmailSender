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

            if (SmtpClientOptions.DatabaseProvider == DatabaseProviderEnum.SqlServer)
            {
                query = """
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

                using var connection = new SqlConnection(SmtpClientOptions.ConnectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = id });
            }
            else if (SmtpClientOptions.DatabaseProvider == DatabaseProviderEnum.PostgreSQL)
            {
                query = """
                        SELECT ed."Id",
                               ed."Body",
                               ed."BodyAsHtml",
                               ed."Subject",
                               ed."ReplacementQuantity"
                        FROM public."EmailDictionaries" AS ed
                        WHERE ed."Id" = @Id
                        """;

                //query = """
                //        SELECT ed."Id",
                //               ed."Body",
                //               ed."BodyAsHtml",
                //               ed."Subject",
                //               ed."ReplacementQuantity",
                //               ed."IsActive"
                //        FROM public."EmailDictionaries" AS ed
                //        WHERE ed."Id" = @Id
                //        AND ed."IsActive" = true
                //        """;

                using var connection = new NpgsqlConnection(SmtpClientOptions.ConnectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = id });
            }
            else
                throw new NotSupportedException("Unsupported database provider.");
            
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

        if (SmtpClientOptions.DatabaseProvider == DatabaseProviderEnum.SqlServer)
        {
            query = """
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'EmailDictionaries'
            """;

            using var connection = new SqlConnection(SmtpClientOptions.ConnectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
        }
        else if (SmtpClientOptions.DatabaseProvider == DatabaseProviderEnum.PostgreSQL)
        {
            query = """
                SELECT tablename
                FROM pg_catalog.pg_tables
                WHERE tablename = 'emaildictionaries'
            """;

            using var connection = new NpgsqlConnection(SmtpClientOptions.ConnectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
        }
        else
            throw new NotSupportedException("Unsupported database provider.");
        
    }
}
