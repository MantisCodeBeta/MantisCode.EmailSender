using Microsoft.Data.SqlClient;
using Dapper;
using MantisCode.EmailSender.Models;
using MantisCode.EmailSender.Interfaces;
using Npgsql;
using MantisCode.EmailSender.Enums;

namespace MantisCode.EmailSender.Repositories;

public class EmailDictionaryQueryRepository : IEmailDictionaryQueryRepository
{
    public async Task<EmailDictionaries?> GetEmailDictionaryByIdAsync(int id, string connectionString, DatabaseProviderEnum provider)
    {
        try
        {
            string query;

            if (provider == DatabaseProviderEnum.SQLServer)
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
                        """;

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = id });
            }
            else if (provider == DatabaseProviderEnum.PostgreSQL)
            {
                query = """
                        SELECT Id, 
                               Body, 
                               BodyAsHtml, 
                               Subject, 
                               ReplacementQuantity, 
                               IsActive
                        FROM "EmailDictionaries"
                        WHERE Id = @Id
                        """;

                using var connection = new NpgsqlConnection(connectionString);
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

    public async Task<bool> IsDictionaryTableCreatedAsync(string connectionString, DatabaseProviderEnum provider)
    {
        string query;

        if (provider == DatabaseProviderEnum.SQLServer)
        {
            query = """
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'EmailDictionaries'
            """;

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
        }
        else if (provider == DatabaseProviderEnum.PostgreSQL)
        {
            query = """
                SELECT tablename
                FROM pg_catalog.pg_tables
                WHERE tablename = 'emaildictionaries'
            """;

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<string>(query) != null;
        }
        else
            throw new NotSupportedException("Unsupported database provider.");
        
    }
}
