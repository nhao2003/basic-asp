using Awesome.Data;
using Awesome.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Awesome.DTOs.User;
using Awesome.Services.FileUploadService;
using Awesome.Services.SmsService;
using Awesome.Utils;

namespace Awesome.Services.UserService
{
    public class UserService(
        ApplicationDbContext context,
        IEmailSender emailSender,
        IFileUploadService fileUploadService,
        CryptoUtils cryptoUtils,
        ISmsService smsService)
        : IUserService
    {
        private string GenerateRandomString(int length)
        {
            const string chars = "0123456789";
            var randomGenerator = RandomNumberGenerator.Create();
            var data = new byte[length];
            randomGenerator.GetBytes(data);
            return new string(data.Select(x => chars[x % chars.Length]).ToArray());
        }

        private Task SendVerificationEmail(string email, string otp)
        {
            var subject = "Email Verification";
            var message = $"Your OTP is {otp}";
            return emailSender.SendEmailAsync(email, subject, message);
        }

        private Task SendVerificationSms(string phoneNumber, string otp)
        {
            var message = $"Your OTP is {otp}";
            return smsService.SendSmsAsync(phoneNumber, message);
        }

        public async Task<User?> GetUserAsync(Guid userId)
        {
            return await context.Users.FindAsync(userId);
        }
        
        private async Task<User> FindUserByIdAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return user;
        }


        public async Task<User> UpdateUserEmailAsync(Guid userId, string email)
        {
            var user = await FindUserByIdAsync(userId);

            user.Email = email;
            var otp = this.GenerateRandomString(6);
            user.EmailVerificationToken = cryptoUtils.Hash(user.Id.ToString(), otp);
            user.EmailVerifiedAt = null;
            context.Users.Update(user);
            await this.SendVerificationEmail(email, otp);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAvatarAsync(Guid userId, Stream stream)
        {
            var user = await FindUserByIdAsync(userId);
            const string folder = "profile-pictures";
            var fileName = $"{userId}.jpg";
            var url = await fileUploadService.UploadFileAsync(folder, fileName, stream);
            user.Avatar = url;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber)
        {
            var user = await FindUserByIdAsync(userId);

            user.PhoneNumber = phoneNumber;
            var otp = GenerateRandomString(6);
            var hashedOtp = cryptoUtils.Hash(user.Id.ToString(), otp);
            user.PhoneNumberVerificationToken = hashedOtp;
            user.PhoneNumberVerifiedAt = null;
            context.Users.Update(user);
            await this.SendVerificationSms(phoneNumber, otp);
            await context.SaveChangesAsync();


            return user;
        }

        public async Task<User> UpdateUserProfileAsync(Guid userId, UpdateProfileDto updateProfileDto)
        {
            var user = await FindUserByIdAsync(userId);

            user.FullName = updateProfileDto.FullName;
            user.DateOfBirth = updateProfileDto.DateOfBirth;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User> VerifyEmailAsync(Guid userId, string otp)
        {
            var user = await FindUserByIdAsync(userId);

            if (user.EmailVerifiedAt != null)
            {
                throw new InvalidOperationException("Email already verified");
            }

            var compareResult = cryptoUtils.Verify(user.Id.ToString(), user.EmailVerificationToken, otp);
            if (compareResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid OTP");
            }

            user.EmailVerifiedAt = DateTime.Now;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User> VerifyPhoneNumberAsync(Guid userId, string otp)
        {
            var user = await FindUserByIdAsync(userId);

            if (user.PhoneNumberVerifiedAt != null)
            {
                throw new InvalidOperationException("Phone number already verified");
            }
            
            var compareResult = cryptoUtils.Verify(user.Id.ToString(), user.PhoneNumberVerificationToken, otp);
            if (compareResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid OTP");
            }

            user.PhoneNumberVerifiedAt = DateTime.Now;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}