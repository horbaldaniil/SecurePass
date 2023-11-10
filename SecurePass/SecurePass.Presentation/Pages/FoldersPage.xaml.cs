using SecurePass.DAL.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for FoldersPage.xaml
/// </summary>
public partial class FoldersPage : Page
{
    public int loggedInUserId { get; set; }
    public FoldersPage(int Id)
    {
        InitializeComponent();
        loggedInUserId = Id;

        using (var db = new SecurePassDbContext())
        {
            var folders = db.Folders.Where(f => f.UserId == Id).ToList();
            DataBinding.ItemsSource = folders;
        }

    }
    
    private void AddNewFolder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        NewFolderPanel.Visibility = Visibility.Visible;
        GenaratePasswordBackground.Visibility = Visibility.Visible;
    }

    private void AddText(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).AddText(sender, e);
    }

    private void RemoveText(object sender, RoutedEventArgs e)
    {
        ((App)Application.Current).RemoveText(sender, e);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string newFolderName = NewFolderTextBox.Text;
        FolderNameError.Visibility = Visibility.Collapsed;

        if (!string.IsNullOrEmpty(newFolderName))
        {
            using (var db = new SecurePassDbContext())
            {
                var existingFolder = db.Folders.FirstOrDefault(f => f.UserId == loggedInUserId && f.Title == newFolderName);

                if (existingFolder == null)
                {
                    var newFolder = new FolderModel
                    {
                        Title = newFolderName,
                        UserId = loggedInUserId
                    };

                    db.Folders.Add(newFolder);
                    db.SaveChanges();

                    var folders = db.Folders.Where(f => f.UserId == loggedInUserId).ToList();
                    DataBinding.ItemsSource = folders;

                    Color color = (Color)ColorConverter.ConvertFromString("#A9B1B8");
                    NewFolderTextBox.Text = NewFolderTextBox.Tag.ToString();
                    NewFolderTextBox.Foreground = new SolidColorBrush(color);

                    NewFolderPanel.Visibility = Visibility.Collapsed;
                    GenaratePasswordBackground.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FolderNameError.Visibility = Visibility.Visible;
                }
            }
        }
    }

    private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!NewFolderPanel.IsMouseOver)
        {
            NewFolderPanel.Visibility = Visibility.Collapsed;
            GenaratePasswordBackground.Visibility = Visibility.Collapsed;
        }
    }

}
