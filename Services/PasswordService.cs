using BCrypt.Net;

namespace Apartments.Services{
    public class PasswordService{
        public string HashPassword(string pass) => BCrypt.Net.BCrypt.HashPassword(pass);
        public bool VerifyPassword(string pass, string hash) => BCrypt.Net.BCrypt.Verify(pass, hash);
    }
}