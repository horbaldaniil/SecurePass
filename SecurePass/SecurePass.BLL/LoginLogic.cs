﻿// <copyright file="LoginLogic.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using log4net;
    using Microsoft.EntityFrameworkCore;
    using SecurePass.DAL.Model;

    /// <summary>
    /// The <c>LoginLogic</c> class provides logic for email and password validation,
    /// as well as user verification during the login process.
    /// </summary>
    public class LoginLogic
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        /// Verifies a given password against a stored hashed password.
        /// </summary>
        /// <param name="enteredPassword">The entered password to be verified.</param>
        /// <param name="storedHashedPassword">The stored hashed password for comparison.</param>
        /// <returns>True if the password is verified; otherwise, false.</returns>
        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
            }
            catch (Exception ex)
            {
                log.Error($"Exception in VerifyPassword: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Verifies a user's credentials during the login process and sets the current user if successful.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's entered password.</param>
        /// <returns>
        /// Null if the user is successfully verified and the current user is set; otherwise, a string indicating
        /// "NotValidData" if the provided credentials are not valid.
        /// </returns>
        public static async Task<string?> VerifyUserAsync(string email, string password)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    log.Info($"Verifying user with email: {email}");

                    using var db = new SecurePassDbContext();
                    var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);

                    if (user != null)
                    {
                        if (VerifyPassword(password, user.Password))
                        {
                            CurrentUserManager.SetCurrentUser(user);
                            log.Info($"User verified and set as the current user: {user.Email}");
                        }
                        else
                        {
                            log.Warn($"User verification failed for email: {user.Email}. Invalid password.");
                            return "NotValidData";
                        }
                    }
                    else
                    {
                        log.Warn($"User not found for email: {email}. Verification failed.");
                        return "NotValidData";
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    log.Error($"Exception in VerifyUserAsync: {ex}");
                    return "NotValidData";
                }
            }).ConfigureAwait(false);
        }
    }
}