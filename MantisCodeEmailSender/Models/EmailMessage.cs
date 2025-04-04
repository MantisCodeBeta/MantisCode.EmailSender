namespace MantisCode.EmailSender.Models;

public class EmailMessage
{
    public string From { get; set; } = string.Empty;
    public List<string> To { get; } = [];
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsBodyHtml { get; set; } = false;
}
