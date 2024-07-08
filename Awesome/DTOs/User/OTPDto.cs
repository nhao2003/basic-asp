using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Awesome.DTOs.User;

public partial class OtpValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("OTP is required");
        }

        return !MyRegex().IsMatch(value.ToString() ?? string.Empty) ? new ValidationResult("OTP must be 6 digits") : ValidationResult.Success;
    }

    [GeneratedRegex(@"^\d{6}$")]
    private static Regex MyRegex() => new Regex(@"^\d{6}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
}

public class OtpDto
{
    [OtpValidator]
    public required string OTP { get; set; }
}