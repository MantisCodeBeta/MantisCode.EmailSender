namespace MantisCode.EmailSender.Interfaces;

internal interface IEmailDictionaryCommandRepository
{
    Task<bool> CreateEmailDictionariesTable();
}
