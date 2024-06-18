using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Awesome.DTOs.User;

public class OtpValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("OTP is required");
        }

        return !Regex.IsMatch(value.ToString()!, @"^\d{6}$") ? new ValidationResult("OTP must be 6 digits") : ValidationResult.Success;
    }
}

public class OtpDto
{
    [OtpValidator]
    public string OTP { get; set; }
}