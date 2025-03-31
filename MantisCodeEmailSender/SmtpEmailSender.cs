using MantisCodeEmailSender.Models;
using System.Net.Mail;
using System.Net;
using MantisCodeEmailSender.Interfaces;
using System.Text;

namespace MantisCodeEmailSender;

public class SmtpEmailSender(SmtpSettings smtpSettings) : IEmailSender
{
    private readonly SmtpSettings _smtpSettings = smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings));

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
