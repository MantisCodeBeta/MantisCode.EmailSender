using Dapper;
using MantisCode.EmailSender.Enums;
using MantisCode.EmailSender.Interfaces;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace MantisCode.EmailSender.Repositories;

internal class EmailDictionaryCommandRepository : IEmailDictionaryCommandRepository
{
    public async Task<bool> CreateEmailDictionariesTable()
    {
        try
        {
            string query;

            switch (SmtpClientOptions.DatabaseProvider)
            {
                case DatabaseProviderEnum.SqlServer:
                {
                    query = """
                            CREATE TABLE EmailDictionaries
                            (
                                Id INT PRIMARY KEY IDENTITY(1, 1),
                                Body NVARCHAR(MAX),
                                BodyAsHtml NVARCHAR(MAX),
                                Subject NVARCHAR(MAX),
                                ReplacementQuantity INT,
                                IsActive BIT
                            )
                            """;

                    await using SqlConnection connection = new(SmtpClientOptions.ConnectionString);
                    await connection.OpenAsync();
                    return await connection.ExecuteAsync(query) > 0;
                }
                case DatabaseProviderEnum.PostgreSql:
                {
                    query = """
                            CREATE TABLE EmailDictionaries
                            (
                                Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                Body VARCHAR,
                                BodyAsHtml VARCHAR,
                                Subject VARCHAR,
                                ReplacementQuantity INT,
                                IsActive BOOLEAN
                            )
                            """;

                    await using NpgsqlConnection connection = new(SmtpClientOptions.ConnectionString);
                    await connection.OpenAsync();
                    return await connection.ExecuteAsync(query) > 0;
                }
                default:
                    throw new NotSupportedException("Unsupported database provider.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
}