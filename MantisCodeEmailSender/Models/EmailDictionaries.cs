namespace MantisCode.EmailSender.Models;

public class EmailDictionaries
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;
    public string BodyAsHtml { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int ReplacementQuantity { get; set; }
    public bool IsActive { get; set; }
}
