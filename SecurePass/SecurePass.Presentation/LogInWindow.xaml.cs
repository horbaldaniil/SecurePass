using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordErrorLabel.Content = "";
            EmailErrorLabel.Content = "";

            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            if (!IsValidEmail(email))
            {
                EmailErrorLabel.Content = "Invalid email format";
                return;
            }

            using (var db = new SecurePassDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user != null)
                {
                    if (VerifyPassword(password, user.Password))
                    {
                        PasswordsWindow window = new PasswordsWindow();
                        window.Show();
                        Close();
                    }
                    else
                    {
                        PasswordErrorLabel.Content = "Invalid password";
                    }
                }
                else
                {
                    EmailErrorLabel.Content = "Email not found";
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (Regex.IsMatch(email, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return enteredPassword == storedHashedPassword;
            // Перевірка паролю збереженого в базі даних з введеним паролем
            // Використовуйте бібліотеку для перевірки паролів, як BCrypt.NET.
        }

        private void SignUpLabel_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow window = new SignUpWindow();
            window.Show();
            Close();
        }
        public void RemoveText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            instance.Foreground = Brushes.Black;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }

        public void AddText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            Color color = (Color)ColorConverter.ConvertFromString("#A9B1B8");

            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                instance.Text = instance.Tag.ToString();
                instance.Foreground = new SolidColorBrush(color);
            }
        }
    }
}
