using MantisCodeEmailSender.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MantisCodeEmailSender.EntityFramework.Configurations;

public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.ToTable("EmailLog");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.EmailBody).IsRequired().HasColumnType("Varchar(255)"); ;
        builder.Property(e => e.EmailSubject).IsRequired().HasColumnType("Varchar(255)"); ;
        builder.Property(e => e.EmailTo).IsRequired().HasColumnType("Varchar(255)"); ;
        builder.Property(e => e.EmailFrom).IsRequired().HasColumnType("Varchar(255)"); ;
        builder.Property(e => e.IsBodyHtml).IsRequired();
        builder.Property(e => e.SendDate).IsRequired();
        builder.Property(e => e.LogDate).IsRequired();
    }
}
