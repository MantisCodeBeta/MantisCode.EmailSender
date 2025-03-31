using MantisCodeEmailSender.Models;
using System.Net.Mail;
using System.Net;
using MantisCodeEmailSender.Interfaces;
using System.Text;
using MantisCode.EmailSender.Interfaces;
using MantisCode.EmailSender.Enums;

namespace MantisCodeEmailSender;

public class SmtpEmailSender(SmtpSettings smtpSettings, 
    IEmailDictionaryCommandRepository emailDictionaryCommand, 
    IEmailDictionaryQueryRepository emailDictionaryQuery) : IEmailSender
{
    public DatabaseProviderEnum? DatabaseProviderEnum { get; set; } = null;
    private readonly SmtpSettings _smtpSettings = smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings));
    private static bool IsTableCreated { get; set; } = false;

    public async Task<SmtpEmailSenderResponse> SendEmailAsyncWithDictionaries(EmailMessage message, EmailBuilder emailBuilder, int emailTypeId, string connectionString, DatabaseProviderEnum databaseProvider)
    {
        SmtpEmailSenderResponse response = new() { Success = true };
        try
        {
            if (DatabaseProviderEnum is null)
                throw new Exception("Database Provider Not Set");
            if (!IsTableCreated && !emailDictionaryQuery.IsDictionaryTableCreatedAsync(connectionString, databaseProvider).Result)
            {
                await emailDictionaryCommand.CreateEmailDictionariesTable(connectionString, databaseProvider);
                IsTableCreated = true;
            }

            using var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };

            var email = await emailDictionaryQuery.GetEmailDictionaryByIdAsync(emailTypeId, connectionString, databaseProvider) ?? throw new Exception("Email Type With Specified Id Not Found");
            var subject = email.Subject;
            var body = email.Body ?? email.BodyAsHtml;
            if (emailBuilder.Count() != email.ReplacementQuantity)
                throw new Exception("Insufficient Replaceble Variables To Build Email");

            foreach (var replace in emailBuilder.Get())
            {
                if (body!.IndexOf(replace.Key) < 0)
                    throw new Exception("Incorrect Replaceable Variables To Build Email");
                body = body.Replace(replace.Key, replace.Value);
            }

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };
            foreach (var recipient in message.To)
            {
                mailMessage.IsBodyHtml = !string.IsNullOrEmpty(email.BodyAsHtml);
                mailMessage.To.Add(recipient);
            }
            await smtpClient.SendMailAsync(mailMessage);

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
            using var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };

            foreach (var recipient in message.To)
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
            using var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            foreach (var message in messages)
            {
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(message.From),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = message.IsBodyHtml
                };

                foreach (var recipient in message.To)
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
            using var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            };

            foreach (var recipient in message.To)
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
        foreach (var variable in bodyVariableValues)
        {
            if (body.Contains($"{{{variable.Key}}}"))
                newBody = newBody.Replace($"{{{variable.Key}}}", variable.Value);
        }
        return newBody.ToString();
    }
}

public class EmailBuilder
{
    private Dictionary<string, string> _replace = new Dictionary<string, string>();
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