using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurePass.BLL;

public class LoginLogic
{

    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
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

    public static async Task<string?> VerifyUser(string email, string password)
    {
        return await Task.Run(async () =>
        {
            using (var db = new SecurePassDbContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);

                if (user != null)
                {
                    if (VerifyPassword(password, user.Password))
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

                return null;
            }
        }).ConfigureAwait(false);
    }
}
