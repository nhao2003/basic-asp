using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Awesome.Services.SmsService;

public class TwilioSmsService
{
    private readonly IConfiguration _configuration;

    private string phoneNumber;

    public TwilioSmsService(IConfiguration configuration)
    {
        _configuration = configuration;
        var accountSid = _configuration["Twilio:AccountSid"] ??
                         throw new ArgumentNullException(nameof(configuration));
        var authToken = _configuration["Twilio:AuthToken"] ??
                        throw new ArgumentNullException(nameof(configuration));
        phoneNumber = _configuration["Twilio:PhoneNumber"] ?? throw new ArgumentNullException(nameof(configuration));
        TwilioClient.Init(accountSid, authToken);
    }

    public async Task SendSmsAsync(string toPhoneNumber, string message)
    {
        var messageResource = await MessageResource.CreateAsync(
            body: message,
            from: new PhoneNumber(phoneNumber),
            to: new PhoneNumber(toPhoneNumber)
        );

        if (messageResource.ErrorCode.HasValue)
        {
            throw new Exception(messageResource.ErrorMessage);
        }
    }
}