namespace Awesome.Services.SmsService;

public class MockSmsService : ISmsService
{
    private ILogger<MockSmsService> logger;
    
    public MockSmsService(ILogger<MockSmsService> logger)
    {
        this.logger = logger;
    }
    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        logger.LogInformation($"Sending SMS to {phoneNumber}: {message}");
    }
}