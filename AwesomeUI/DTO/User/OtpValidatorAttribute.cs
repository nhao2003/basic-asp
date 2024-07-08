using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AwesomeUI.DTO.User;

public partial class OtpValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("OTP is required");
        }

        return !MyRegex().IsMatch(value.ToString()) ? new ValidationResult("OTP must be 6 digits") : ValidationResult.Success;
    }

    [GeneratedRegex(@"^\d{6}$")]
    private static Regex MyRegex() => new Regex(@"^\d{6}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
}
