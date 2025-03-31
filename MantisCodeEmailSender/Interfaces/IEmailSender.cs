using MantisCode.EmailSender.Enums;
using MantisCodeEmailSender.Models;

namespace MantisCodeEmailSender.Interfaces;

public interface IEmailSender
{
    Task<SmtpEmailSenderResponse> SendEmailAsyncWithDictionaries(EmailMessage message, EmailBuilder emailBuilder, int emailTypeId, string connectionString, DatabaseProviderEnum databaseProvider);
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message);
    Task<SmtpEmailSenderResponse> SendEmailAsync(List<EmailMessage> messages);
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message, Dictionary<string, string> bodyVariableValues);
}
