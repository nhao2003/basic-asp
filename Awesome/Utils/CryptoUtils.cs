using Microsoft.AspNetCore.Identity;

namespace Awesome.Utils
{
    public class CryptoUtils
    {
        private PasswordHasher<string> _passwordHasher = new();

        public string Hash(string user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }
        
        public PasswordVerificationResult Verify(string user, string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
