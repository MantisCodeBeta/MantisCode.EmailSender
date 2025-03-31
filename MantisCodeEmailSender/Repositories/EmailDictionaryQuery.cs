using Microsoft.Data.SqlClient;
using Dapper;
using MantisCode.EmailSender.Models;

namespace MantisCode.EmailSender.Repositories;

public class EmailDictionaryQuery(string _connectionString)
{
    public async Task<EmailDictionaries?> GetEmailDictionaryById(int Id)
    {
		try
		{
			string query = """
				SELECT Id, 
				Body, 
				BodyAsHtml, 
				Subject, 
				ReplacementQuantity, 
				IsActive
				FROM EmailDictionaries ed WITH (NOLOCK)
				WHERE ed.Id = @Id
				""";

			using SqlConnection connection = new(_connectionString);
			await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<EmailDictionaries>(query, new { @Id = Id });
        }
		catch (Exception ex)
		{

			throw;
		}
    }
}
