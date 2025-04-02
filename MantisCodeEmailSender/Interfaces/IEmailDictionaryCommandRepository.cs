using MantisCode.EmailSender.Enums;

namespace MantisCode.EmailSender.Interfaces;

internal interface IEmailDictionaryCommandRepository
{
    Task<bool> CreateEmailDictionariesTable(string connectionString, DatabaseProviderEnum provider);
}
