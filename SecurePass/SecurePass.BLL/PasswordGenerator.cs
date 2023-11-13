using System;
using System.Text;

namespace SecurePass.BLL
{
    public class PasswordGenerator
    {
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumberChars = "0123456789";
        private const string SymbolChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        public static string GeneratePassword(int length, bool includeUppercase, bool includeNumbers, bool includeSymbols)
        {
            string validChars = LowercaseChars;
            if (includeUppercase)
                validChars += UppercaseChars;
            if (includeNumbers)
                validChars += NumberChars;
            if (includeSymbols)
                validChars += SymbolChars;

            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, validChars.Length);
                password.Append(validChars[randomIndex]);
            }

            return password.ToString();
        }
    }
}