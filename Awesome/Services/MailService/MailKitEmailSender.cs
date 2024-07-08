using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Awesome.Services.MailService;

public class MailKitEmailSender(IOptions<MailKitEmailSenderOptions> options) : IEmailSender
{
    private MailKitEmailSenderOptions Options { get; set; } = options.Value;

    public Task SendEmailAsync(string email, string subject, string message)
    {
        return Execute(email, subject, message);
    }

    private Task Execute(string to, string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
        if (!string.IsNullOrEmpty(Options.Sender_Name))
            email.Sender.Name = Options.Sender_Name;
        email.From.Add(email.Sender);
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };
 
        using (var smtp = new SmtpClient())
        {
            smtp.Connect(Options.Host_Address, 25, Options.Host_SecureSocketOptions);
            smtp.Authenticate(Options.Host_Username, Options.Host_Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
 
        return Task.FromResult(true);
    }
}