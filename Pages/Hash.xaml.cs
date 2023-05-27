using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace ddCrypt.Pages
{
    /// <summary>
    /// Interaction logic for Hash.xaml
    /// </summary>
    public partial class Hash : Page
    {
        public Hash()
        {
            InitializeComponent();
        }

        private void TextHashButton_Click(object sender, RoutedEventArgs e)
        {
            hashResultTextBlock.Text = EncryptionAlgorithms.Hash.GetHash(AlgorithmHash.Text, inputText: textInput.Text);
        }

        private void FileHashButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                hashResultTextBlock.Text = EncryptionAlgorithms.Hash.GetHash(AlgorithmHash.Text, filePath: filePath);
            }
        }

    }
}
