using System.Windows;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ddCrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Pages.Symmetric symmetric = new Pages.Symmetric();
        private Pages.Asymmetric asymmetric = new Pages.Asymmetric();
        private Pages.Hash hash = new Pages.Hash();

        public MainWindow()
        {
            InitializeComponent();
            PagesFrame.Content = symmetric;
        }

        
        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            Window1 about = new Window1();
            about.ShowDialog();
        }

        private void Developer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developer: dobrodelete\nTelegram: @dobrodelete\nGithub: dobrodelete", "About developer", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Asymmetric_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.NavigationService.Navigate(asymmetric);
        }

        private void Symmetric_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.NavigationService.Navigate(symmetric);
        }

        private void HashButton_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.NavigationService.Navigate(hash);
        }
    }
}
