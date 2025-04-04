using MantisCode.EmailSender.Enums;
using MantisCode.EmailSender.Models;

namespace MantisCode.EmailSender;

internal static class SmtpClientOptions
{
    internal static SmtpSettings SmtpSettings { get; set; } = new SmtpSettings();
    internal static DatabaseProviderEnum DatabaseProvider { get; set; } = DatabaseProviderEnum.SqlServer;
    internal static string ConnectionString { get; set; } = string.Empty;
}
