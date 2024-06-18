using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using Awesome.Services.MailService;
using MailKit.Net.Smtp;

public class MailKitEmailSender(IOptions<MailKitEmailSenderOptions> options) : IEmailSender
{
    public MailKitEmailSenderOptions Options { get; set; } = options.Value;

    public Task SendEmailAsync(string email, string subject, string message)
    {
        return Execute(email, subject, message);
    }
 
    public Task Execute(string to, string subject, string message)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
        if (!string.IsNullOrEmpty(Options.Sender_Name))
            email.Sender.Name = Options.Sender_Name;
        email.From.Add(email.Sender);
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = message };
 
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