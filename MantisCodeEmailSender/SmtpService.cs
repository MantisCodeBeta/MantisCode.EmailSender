using MantisCode.EmailSender.Interfaces;
using MantisCode.EmailSender.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MantisCode.EmailSender;

internal class SmtpService(IEmailDictionaryCommandRepository emailDictionaryCommand,
    IEmailDictionaryQueryRepository emailDictionaryQuery) : ISmtpService
{
    private readonly SmtpSettings _smtpSettings = SmtpClientOptions.SmtpSettings ?? throw new ArgumentNullException(nameof(SmtpClientOptions.SmtpSettings));
    private static bool IsTableCreated { get; set; } = false;

    public async Task<SmtpEmailSenderResponse> SendEmailWithDictionariesAsync(EmailMessage message, EmailBuilder emailBuilder, int emailTypeId)
    {
        SmtpEmailSenderResponse response = new() { Success = true };
        try
        {
            if (!IsTableCreated && !emailDictionaryQuery.IsDictionaryTableCreatedAsync().Result)
            {
                await emailDictionaryCommand.CreateEmailDictionariesTable();
                IsTableCreated = true;
            }

            using SmtpClient smtpClient = new()
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };

            EmailDictionaries? email = await emailDictionaryQuery.GetEmailDictionaryByIdAsync(emailTypeId) ?? throw new Exception("Email Type With Specified Id Not Found");
            string? body = email.Body ?? email.BodyAsHtml;
            if (emailBuilder.Count() != email.ReplacementQuantity)
                throw new Exception("Insufficient Replaceble Variables To Build Email");

            foreach (KeyValuePair<string, string> replace in emailBuilder.Get())
            {
                if (body.IndexOf(replace.Key, StringComparison.OrdinalIgnoreCase) < 0)
                    throw new Exception("Incorrect Replaceable Variables To Build Email");
                body = body.Replace(replace.Key, replace.Value);
            }

            using MailMessage mailMessage = new()
            {
                From = new MailAddress(message.From),
                Subject = email.Subject,
                Body = body,
                IsBodyHtml = !string.IsNullOrEmpty(email.BodyAsHtml)
            };
            foreach (string? recipient in message.To)
            {
                mailMessage.To.Add(recipient);
            }
            await smtpClient.SendMailAsync(mailMessage);
            smtpClient.Dispose();
            mailMessage.Dispose();

            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }

    public async Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message)
    {
        SmtpEmailSenderResponse response = new() { Success = true };
        try
        {
            using SmtpClient smtpClient = new()
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            using MailMessage mailMessage = new()
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };

            foreach (string? recipient in message.To)
                mailMessage.To.Add(recipient);
            await smtpClient.SendMailAsync(mailMessage);

            smtpClient.Dispose();
            mailMessage.Dispose();
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }

    public async Task<SmtpEmailSenderResponse> SendEmailAsync(List<EmailMessage> messages)
    {
        SmtpEmailSenderResponse response = new() { Success = true };
        try
        {
            using SmtpClient smtpClient = new()
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            foreach (EmailMessage? message in messages)
            {
                using MailMessage mailMessage = new()
                {
                    From = new MailAddress(message.From),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = message.IsBodyHtml
                };

                foreach (string? recipient in message.To)
                    mailMessage.To.Add(recipient);
                await smtpClient.SendMailAsync(mailMessage);
                mailMessage.Dispose();
            }

            smtpClient.Dispose();
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }

    public async Task<SmtpEmailSenderResponse> SendEmailAsync(EmailMessage message, Dictionary<string, string> bodyVariableValues)
    {
        SmtpEmailSenderResponse response = new() { Success = true };
        try
        {
            var body = ReplaceBodyVariables(message.Body, bodyVariableValues);
            using SmtpClient smtpClient = new()
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            using MailMessage mailMessage = new()
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = body,
                IsBodyHtml = message.IsBodyHtml
            };

            foreach (string? recipient in message.To)
                mailMessage.To.Add(recipient);
            await smtpClient.SendMailAsync(mailMessage);

            smtpClient.Dispose();
            mailMessage.Dispose();
            return response;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }

    }
    private static string ReplaceBodyVariables(string body, Dictionary<string, string> bodyVariableValues)
    {
        StringBuilder newBody = new(body);
        newBody = bodyVariableValues.Where(variable => 
            body.Contains($"{{{variable.Key}}}")).Aggregate(newBody, (current, variable) => 
                current.Replace($"{{{variable.Key}}}", variable.Value)
            );
        return newBody.ToString();
    }
}

public class EmailBuilder
{
    private Dictionary<string, string> _replace = [];
    public void AddReplacement(string oldValue, string newValue)
    {
        _replace[oldValue] = newValue;
    }
    public int Count()
    {
        return _replace.Count;
    }
    public Dictionary<string, string> Get()
    {
        return _replace;
    }
}