﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurePass.BLL;

public class LoginLogic
{
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
}