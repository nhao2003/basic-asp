using Vonage;
using Vonage.Request;
namespace Awesome.Services.SmsService;

public class VonageSmsMessage : ISmsService
{
    private VonageClient _vonageClient;
    private ILogger<VonageSmsMessage> _logger;
    private IConfiguration _configuration;
    public VonageSmsMessage(ILogger<VonageSmsMessage> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        var credentials = Credentials.FromApiKeyAndSecret(
            _configuration["Vonage:ApiKey"],
            _configuration["Vonage:ApiSecret"]
        );
        _vonageClient = new VonageClient(credentials);
    }
    public async Task SendSmsAsync(string toPhoneNumber, string message)
    {
        if (toPhoneNumber.StartsWith("0"))
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
        _logger.LogInformation($"Message sent to {toPhoneNumber}" +
            $" with message: {message}");
    }
}