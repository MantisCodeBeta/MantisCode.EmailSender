namespace MantisCodeEmailSender.EntityFramework.Entities;

public class EmailLogs
{
    public long Id { get; set; }
    public string EmailBody { get; set; } = null!;
    public string EmailSubject { get; set; } = null!;
    public string EmailTo { get; set; } = null!;
    public string EmailFrom { get; set; } = null!;
    public bool IsBodyHtml { get; set; }
    public DateTime SendDate { get; set; }
    public DateTime LogDate { get; set; }
}
