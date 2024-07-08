using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Awesome.Services.SmsService;

public class TwilioSmsService
{
    private readonly string _phoneNumber;

    public TwilioSmsService(IConfiguration configuration)
    {
        var configuration1 = configuration;
        var accountSid = configuration1["Twilio:AccountSid"] ??
                         throw new ArgumentNullException(nameof(configuration));
        var authToken = configuration1["Twilio:AuthToken"] ??
                        throw new ArgumentNullException(nameof(configuration));
        _phoneNumber = configuration1["Twilio:PhoneNumber"] ?? throw new ArgumentNullException(nameof(configuration));
        TwilioClient.Init(accountSid, authToken);
    }

    public async Task SendSmsAsync(string toPhoneNumber, string message)
    {
        await MessageResource.CreateAsync(
            body: message,
            from: new PhoneNumber(_phoneNumber),
            to: new PhoneNumber(toPhoneNumber)
        );
    }
}