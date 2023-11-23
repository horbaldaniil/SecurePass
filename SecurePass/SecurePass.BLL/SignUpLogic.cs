// <copyright file="SignUpLogic.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SecurePass.DAL.Model;

    /// <summary>
    /// The <c>SignUpLogic</c> class provides methods for validating email and password formats,
    /// hashing passwords, and performing user registration with email and password.
    /// </summary>
    public class SignUpLogic
    {
        /// <summary>
        /// Validates whether the given string is a valid email address.
        /// </summary>
        /// <param name="email">The email address to be validated.</param>
        /// <returns>True if the email is valid; otherwise, false.</returns>
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Validates whether the given string is a valid password.
        /// </summary>
        /// <param name="password">The password to be validated.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public static bool IsValidPassword(string password)
        {
            if (password.Length < 11)
            {
                return false;
            }

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]+$";
            return Regex.IsMatch(password, pattern);
        }

        /// <summary>
        /// Hashes the given password using the BCrypt hashing algorithm.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password.</returns>
        public static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            return hashedPassword;
        }

        /// <summary>
        /// Registers a new user with the provided email and hashed password.
        /// </summary>
        /// <param name="email">The email address of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <returns>
        /// Null if the user is successfully registered; otherwise, a string indicating
        /// "InUseEmail" if the provided email is already in use.
        /// </returns>
        public static async Task<string?> UserRegistration(string email, string password)
        {
            return await Task.Run(async () =>
            {
                using var db = new SecurePassDbContext();
                if (await db.Users.AnyAsync(u => u.Email == email).ConfigureAwait(false))
                {
                    return "InUseEmail";
                }

                string hashedPassword = HashPassword(password);

                var newUser = new UserModel
                {
                    Email = email,
                    Password = hashedPassword,
                };

                await db.Users.AddAsync(newUser).ConfigureAwait(false);
                await db.SaveChangesAsync().ConfigureAwait(false);

                return null;
            }).ConfigureAwait(false);
        }
    }
}