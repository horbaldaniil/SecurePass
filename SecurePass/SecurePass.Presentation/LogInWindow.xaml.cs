// <copyright file="LogInWindow.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using SecurePass.BLL;

    /// <summary>
    /// Interaction logic for LogInWindow.xaml.
    /// </summary>
    public partial class LogInWindow : Window
    {
        private bool loading;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInWindow"/> class.
        /// </summary>
        public LogInWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);

            EmailTextBox.Text = "gorbaldaniil@gmail.com";
            PasswordTextBox.Text = "Gorbal1234!";
        }

        private void Lang_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetLang(((Image)sender).Tag.ToString());
        }

        private void SetLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resdict = new ()
            {
                Source = new Uri($"/Languages/Dictionary-{lang}.xaml", UriKind.Relative),
            };
            Application.Current.Resources.MergedDictionaries.Add(resdict);

            switch (lang)
            {
                case "en-US":
                    LangImage.Source = new BitmapImage(new Uri($"/Images/uk-UA.png", UriKind.Relative));
                    LangImage.Tag = "uk-UA";
                    break;
                case "uk-UA":
                    LangImage.Source = new BitmapImage(new Uri($"/Images/en-US.png", UriKind.Relative));
                    LangImage.Tag = "en-US";
                    break;
                default:
                    break;
            }

            Properties.Settings.Default.lang = lang;
            Properties.Settings.Default.Save();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!loading)
            {
                loading = true;
                LoginButton.Style = (Style)FindResource("LoginButtonLoaded");
                try
                {
                    log.Info("User is trying to log in.");
                    ValidErrorLabel.Content = string.Empty;
                    EmailErrorLabel.Content = string.Empty;

                    string email = EmailTextBox.Text;
                    string password = PasswordTextBox.Text;

                    if (!LoginLogic.IsValidEmail(email))
                    {
                        log.Warn($"Invalid email format entered: {email}");
                        EmailErrorLabel.SetResourceReference(ContentProperty, "InvalidFormatEmail");
                        return;
                    }

                    var loginResult = await LoginLogic.VerifyUserAsync(email, password);

                    if (loginResult != null)
                    {
                        log.Warn($"Failed login attempt for user: {email}. Reason: {loginResult}");
                        ValidErrorLabel.SetResourceReference(ContentProperty, loginResult);
                    }
                    else
                    {
                        log.Info($"User {email} successfully logged in.");
                        var window = new MainWindow();
                        window.Show();
                        Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Exception during login: {ex}");
                }
                finally
                {
                    LoginButton.Style = (Style)FindResource("LoginButton");
                    loading = false;
                }
            }
        }

        private void SignUpLabel_Click(object sender, RoutedEventArgs e)
        {
            log.Info("User clicked on Sign Up label.");
            SignUpWindow window = new ();
            window.Show();
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
