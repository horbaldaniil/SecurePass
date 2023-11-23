// <copyright file="PasswordGenerator.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using System;
    using System.Text;

    /// <summary>
    /// The <c>PasswordGenerator</c> class provides methods to generate random passwords
    /// with customizable criteria such as length, inclusion of uppercase letters, numbers, and symbols.
    /// </summary>
    public class PasswordGenerator
    {
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumberChars = "0123456789";
        private const string SymbolChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        /// <summary>
        /// Generates a random password based on specified criteria.
        /// </summary>
        /// <param name="length">The length of the generated password.</param>
        /// <param name="includeUppercase">Include uppercase letters in the password.</param>
        /// <param name="includeNumbers">Include numbers in the password.</param>
        /// <param name="includeSymbols">Include symbols in the password.</param>
        /// <returns>The generated random password.</returns>
        public static string GeneratePassword(int length, bool includeUppercase, bool includeNumbers, bool includeSymbols)
        {
            string validChars = LowercaseChars;
            if (includeUppercase)
            {
                validChars += UppercaseChars;
            }

            if (includeNumbers)
            {
                validChars += NumberChars;
            }

            if (includeSymbols)
            {
                validChars += SymbolChars;
            }

            Random random = new ();
            StringBuilder password = new ();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, validChars.Length);
                password.Append(validChars[randomIndex]);
            }

            return password.ToString();
        }
    }
}