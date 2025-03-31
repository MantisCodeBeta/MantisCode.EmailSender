using MantisCodeEmailSender;
using MantisCodeEmailSender.Extensions;
using MantisCodeEmailSender.Models;
using Microsoft.Extensions.DependencyInjection;

new ServiceCollection()
    .CreateEmailLogger("Host=urbandb.cn88qoa6y4em.us-east-1.rds.amazonaws.com,5432;Database=urbanDb;UserName=postgres;Password=SHARLOTT88",
    "C:\\Users\\jdund\\source\\repos\\MantisCodeEmailSender\\MantisCodeEmailSender");

//var emailSender = new SmtpEmailSender(new SmtpSettings() { Host = "smtp.office365.com", Port = 587, EnableSsl = true, Password = "jK7N)EG,GIEV5(hSjT/OV_-W{3[/l^", Username = "no-replay@actresearch-global.com" });
//var message = new EmailMessage
//{
//    Body = "Hello World!",
//    IsBodyHtml = false,
//    Subject = "Test Email",
//    From = " no-replay@actresearch-global.com"
//};
//message.To.Add("jdundua@gmail.com");

//await emailSender.SendEmailAsync(message);



//var oauth = new OAuth("smtp.office365.com", 587, "no-replay@actresearch-global.com");
//oauth.SendEmailAsync("jdundua@gmail.com", "test email", "Hello Juba!", )