namespace MantisCode.EmailSender.Models;

public class SmtpEmailSenderResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
