using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurePass.BLL;

public class SignUpLogic
{
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    public bool IsValidPassword(string password)
    {
        if (password.Length < 11)
        {
            return false;
        }

        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]+$";
        return Regex.IsMatch(password, pattern);
    }

    public string HashPassword(string password)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        return hashedPassword;
    }

    public async Task<string> UserRegistration(string email, string password)
    {
        using (var db = new SecurePassDbContext())
        {
            if (await db.Users.AnyAsync(u => u.Email == email))
            {
                return "InUseEmail";
            }

            string hashedPassword = HashPassword(password);

            var newUser = new UserModel
            {
                Email = email,
                Password = hashedPassword
            };

            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();
        }
        return null;
    }
}
