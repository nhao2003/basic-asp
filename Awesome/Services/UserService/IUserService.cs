using Awesome.Models.Entities;

namespace Awesome.Services.UserService;

public interface IUserService
{
    Task<User> GetUserAsync(Guid userId);
    Task<User> UpdateUserEmailAsync(Guid userId, string email);
    Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber);
    Task<User> UpdateUserProfileAsync(Guid userId, string fullName);
    
    Task<User> VerifyEmailAsync(Guid userId, string otp);
    Task<User> VerifyPhoneNumberAsync(Guid userId, string otp);
    
}