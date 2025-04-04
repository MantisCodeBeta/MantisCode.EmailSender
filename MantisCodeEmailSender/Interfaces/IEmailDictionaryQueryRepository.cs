using MantisCode.EmailSender.Models;

namespace MantisCode.EmailSender.Interfaces;

internal interface IEmailDictionaryQueryRepository
{
    Task<EmailDictionaries?> GetEmailDictionaryByIdAsync(int id);
    Task<bool> IsDictionaryTableCreatedAsync();
}
