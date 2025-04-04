using MantisCode.EmailSender.Models;

namespace MantisCode.EmailSender.Interfaces;

public interface ISmtpService
{
    Task<SmtpEmailSenderResponse> SendEmailWithDictionariesAsync(EmailMessage message, EmailBuilder emailBuilder, int emailTypeId);
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message);
    Task<SmtpEmailSenderResponse> SendEmailAsync(List<EmailMessage> messages);
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message, Dictionary<string, string> bodyVariableValues);
}
