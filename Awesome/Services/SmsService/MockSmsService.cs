namespace Awesome.Services.SmsService;

public class MockSmsService(ILogger<MockSmsService> logger) : ISmsService
{
    public Task SendSmsAsync(string toPhoneNumber, string message)
    {
        logger.LogDebug("Sending SMS to {PhoneNumber} with message: {Message}", toPhoneNumber, message);
        return Task.CompletedTask;
    }
}