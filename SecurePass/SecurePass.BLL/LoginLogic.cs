using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurePass.BLL;

public class LoginLogic
{
    private readonly UserModel currentUser;

    
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    public async Task<bool> VerifyPasswordAsync(string enteredPassword, string storedHashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error verifying password: {ex.Message}");
            return false;
        }
    }

    public async Task<string> VerifyUser(string email, string password)
    {
        using (SecurePassDbContext db = new SecurePassDbContext())
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                if (await VerifyPasswordAsync(password, user.Password))
                {
                    CurrentUserManager.SetCurrentUser(user);
                }
                else
                {
                    return "NotValidData";
                }
            }
            else
            {
                return "NotValidData";
            }
        }
        return null;
    }
}
