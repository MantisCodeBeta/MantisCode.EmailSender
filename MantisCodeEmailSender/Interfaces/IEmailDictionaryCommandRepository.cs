using MantisCode.EmailSender.Enums;

namespace MantisCode.EmailSender.Interfaces;

public interface IEmailDictionaryCommandRepository
{
    Task<bool> CreateEmailDictionariesTable(string connectionString, DatabaseProviderEnum provider);
}
