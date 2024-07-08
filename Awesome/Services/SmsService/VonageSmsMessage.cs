using Vonage;
using Vonage.Request;
namespace Awesome.Services.SmsService;

public class VonageSmsMessage : ISmsService
{
    private readonly VonageClient _vonageClient;
    private readonly ILogger<VonageSmsMessage> _logger;

    public VonageSmsMessage(ILogger<VonageSmsMessage> logger, IConfiguration configuration)
    {
        _logger = logger;
        var configuration1 = configuration;
        var credentials = Credentials.FromApiKeyAndSecret(
            configuration1["Vonage:ApiKey"],
            configuration1["Vonage:ApiSecret"]
        );
        _vonageClient = new VonageClient(credentials);
    }
    public async Task SendSmsAsync(string toPhoneNumber, string message)
    {
        if (toPhoneNumber.StartsWith('0'))
        {
            toPhoneNumber = toPhoneNumber.Remove(0, 1);
            toPhoneNumber = $"+84{toPhoneNumber}";
        }
        var response = await _vonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
        {
            To = toPhoneNumber,
            From = "Support",
            Text = message
        });
        _logger.LogDebug(response.ToString());
        _logger.LogDebug("Message sent to {ToPhoneNumber} with message: {Message}", toPhoneNumber, message);
    }
}