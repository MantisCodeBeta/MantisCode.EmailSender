using MantisCodeEmailSender.Models;

namespace MantisCodeEmailSender.Interfaces;

public interface IEmailSender
{
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message);
    Task<SmtpEmailSenderResponse> SendEmailAsync(List<EmailMessage> messages);
    Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message, Dictionary<string, string> bodyVariableValues);
}
