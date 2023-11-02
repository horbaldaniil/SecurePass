using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }
        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            EmailErrorLabel.Content = "";
            PasswordErrorLabel.Content = "";

            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            if (!IsValidEmail(email))
            {
                EmailErrorLabel.Content = "Invalid email format";
                return;
            }

            if (!IsValidPassword(password))
            {
                PasswordErrorLabel.Content = "Invalid password format";
                return;
            }

            using (var db = new SecurePassDbContext())
            {
                if (await db.Users.AnyAsync(u => u.Email == email))
                {
                    EmailErrorLabel.Content = "Email already in use";
                    return;
                }

                string hashedPassword = HashPassword(password);

                var newUser = new UserModel
                {
                    Email = email,
                    Password = hashedPassword
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                MessageBox.Show("Registration successful. You can now log in.");

                LogInWindow loginWindow = new LogInWindow();
                loginWindow.Show();
                Close();
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

        private bool IsValidPassword(string password)
        {
            if (password.Length < 11)
            {
                return false;
            }

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]+$";
            return Regex.IsMatch(password, pattern);
        }

        private string HashPassword(string password)
        {
            return password;
        }
        private void LoginLabel_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow window = new LogInWindow();
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
