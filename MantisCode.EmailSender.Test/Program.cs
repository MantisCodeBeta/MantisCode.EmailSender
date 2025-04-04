using MantisCode.EmailSender;
using MantisCode.EmailSender.Enums;
using MantisCode.EmailSender.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.SmtpClientBuilder(new SmtpSettings
    {
        Host = "smtp.office365.com",
        Port = 587,
        EnableSsl = true,
        Password = "jK7N)EG,GIEV5(hSjT/OV_-W{3[/l^",
        Username = "no-replay@actresearch-global.com"
    },
    "Server=urbandb.cn88qoa6y4em.us-east-1.rds.amazonaws.com,5432;Database=urbanDb;UserName=postgres;Password=SHARLOTT88",
    DatabaseProviderEnum.PostgreSQL
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
