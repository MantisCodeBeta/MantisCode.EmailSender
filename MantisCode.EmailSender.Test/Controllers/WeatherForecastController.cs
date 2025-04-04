using MantisCode.EmailSender.Interfaces;
using MantisCode.EmailSender.Models;
using Microsoft.AspNetCore.Mvc;

namespace MantisCode.EmailSender.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ISmtpService emailSender) : ControllerBase
    {
        [HttpPost("SendEmail")]
        public async Task<SmtpEmailSenderResponse> Send()
        {
            var message = new EmailMessage
            {
                Body = "Satesto Emailis Gagzavna",
                IsBodyHtml = false,
                Subject = "Test Email",
                From = " no-replay@actresearch-global.com"
            };
            message.To.Add("jdundua@gmail.com");

            return await emailSender.SendEmailAsync(message);
        }

        [HttpPost("SendEmailByDictionaryId")]
        public async Task<SmtpEmailSenderResponse> SendEmailByDictionaryId()
        {
            var message = new EmailMessage
            {
                Body = "Satesto Emailis Gagzavna Dictionary-is daxmarebit",
                IsBodyHtml = false,
                Subject = "Test Dictionary Email",
                From = " no-replay@actresearch-global.com"
            };
            message.To.Add("jdundua@gmail.com");

            var emailBuilder = new EmailBuilder();
            emailBuilder.AddReplacement("{OTP}", "1305");

            return await emailSender.SendEmailWithDictionariesAsync(message, emailBuilder, 1);
        }
    }
}
