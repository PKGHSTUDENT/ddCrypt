using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;


using Crypt = ddCrypt.EncryptionAlgorithms;

namespace ddCrypt.Pages
{
    /// <summary>
    /// Interaction logic for Symmetric.xaml
    /// </summary>
    public partial class Symmetric : Page
    {
        private string _inputFileAES = null;
        private string _inputKeyFileAES = null;
        private string _inputIvFileAES = null;
        private string _inputFileDES;
        private string _inputKeyFileDES;
        private string _inputIvFileDES;
        private string _path;

        


        public Symmetric()
        {
            InitializeComponent();
        }

        private static string ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден.", filePath);
            }

            string data = File.ReadAllText(filePath);

            return data;
        }

        
        private static void SaveFile(string fileName, string filePath, string data)
        {
                Random _rnd = new Random();

            if (!File.Exists($"{filePath}\\{fileName}.txt"))
            {
                
                File.WriteAllText($"{filePath}\\{fileName}.txt", data);
            } else
            {
                int rand = _rnd.Next(1, 500);
                var file = File.Create($"{filePath}\\{fileName}{rand}.txt");
                file.Close();
                File.WriteAllText($"{filePath}\\{fileName}{rand}.txt", data);
            }
            // MessageBox.Show($"Похоже программа не смогла сохранить файл: {fileName}");

        }


        private void AESEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if ((!String.IsNullOrEmpty(_inputFileAES) || !String.IsNullOrEmpty(AESInputString.Text)))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = Convert.ToInt16(((ComboBoxItem)AESKeySize.SelectedItem).Content.ToString());
                    try
                    {
                        if (!String.IsNullOrEmpty(_inputFileAES))
                        {
                            Crypt.FileEncryptDecrypt.AESEncryptFile(_inputFileAES, "aes-encrypt-temp.dat", "AES", aes.Key, aes.IV, ((ComboBoxItem)AESCryptMode.SelectedItem).Content.ToString());
                            AESResultString.Text = Convert.ToBase64String(Crypt.AESAlg.EncryptStringToBytes_Aes(AESInputString.Text,
                                aes.Key, aes.IV, ((ComboBoxItem)AESCryptMode.SelectedItem).Content.ToString()));
                        }
                        else
                        {
                            AESResultString.Text = Convert.ToBase64String(Crypt.AESAlg.EncryptStringToBytes_Aes(AESInputString.Text,
                                aes.Key, aes.IV, ((ComboBoxItem)AESCryptMode.SelectedItem).Content.ToString()));

                        }
                        string key = Convert.ToBase64String(aes.Key);
                        string iv = Convert.ToBase64String(aes.IV);

                        AESKeyTextBox.Text = key;
                        AESIvTextBox.Text = iv;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла ошибка {ex}.", "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля данными, или добавьте файлы с данными.", "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /* if (MessageBox.Show("Хотите сохранить данные? Если да, то сейчас откроется окно, в котором вы выберите папку, куда сохранить ключ, вектор инициализации и зашифрованный текст.",
                            "Сохранение данных", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                if (dialog.ShowDialog().GetValueOrDefault() == true)
                {
                    _path = dialog.SelectedPath;
                    SaveFile("key", _path, AESKeyTextBox.Text);
                    SaveFile("iv", _path, AESIvTextBox.Text);
                    if (!File.Exists(_path + "\\aes-encrypt.dat"))
                        File.Move(Directory.GetCurrentDirectory() + "\\aes-encrypt-temp.dat", _path + "\\aes-encrypt.dat");
                    else MessageBox.Show("В вашей выбранной папке уже есть файл aes-encrypt.dat");
                    // SaveFile("encrypted_text", _path, AESResultString.Text);
                    // File.Move("aes-encrypt-temp.dat", _path + "text.enc");
                }
            }*/

            File.Delete(Directory.GetCurrentDirectory() + "\\aes-encrypt-temp.dat");
        }

        private void AESDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if ((!String.IsNullOrEmpty(_inputFileAES) || !String.IsNullOrEmpty(AESInputString.Text)) &&
                (!String.IsNullOrEmpty(_inputKeyFileAES) || !String.IsNullOrEmpty(AESKeyTextBox.Text))&&
                (!String.IsNullOrEmpty(_inputIvFileAES) || !String.IsNullOrEmpty(AESIvTextBox.Text)))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = null;
                    byte[] iv = null;

                    try
                    {
                        if (!String.IsNullOrEmpty(_inputKeyFileAES))
                            key = Convert.FromBase64String(ReadFromFile(_inputKeyFileAES));
                        else key = Convert.FromBase64String(AESKeyTextBox.Text);

                        // MessageBox.Show(Convert.ToBase64String(key));

                        if (!String.IsNullOrEmpty(_inputIvFileAES))
                            iv = Convert.FromBase64String(ReadFromFile(_inputIvFileAES));
                        else iv = Convert.FromBase64String(AESIvTextBox.Text);
                        // MessageBox.Show(Convert.ToBase64String(iv));
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при попытке импорта ключа и вектора инициализации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(_inputFileAES))
                        {
                            Crypt.FileEncryptDecrypt.AESDecryptFile(_inputFileAES, "aes-decrypt-temp.txt", "AES", key, iv, ((ComboBoxItem)AESCryptMode.SelectedItem).Content.ToString());
                        } else
                        {
                            byte[] cipherText = Convert.FromBase64String(AESInputString.Text);
                            string result = Crypt.AESAlg.DecryptStringFromBytes_Aes(cipherText, key, iv, ((ComboBoxItem)AESCryptMode.SelectedItem).Content.ToString());
                            AESResultString.Text = result;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Seems like something went wrong.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля данными, или добавьте файлы с данными.", "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DESEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (DESInputString.Text.Length != 0)
            {
                using (DES des = DES.Create())
                {
                    try
                    {
                        string ciphertext;
                        byte[] key;
                        byte[] iv;
                        Crypt.DESAlg.Encrypt(DESInputString.Text, out ciphertext, out key, out iv, ((ComboBoxItem)DESCryptoMode.SelectedItem).Content.ToString());
                        DESresultString.Text = ciphertext;
                        DESKeyTextBox.Text = Convert.ToBase64String(key);
                        DESIvTextBox.Text = Convert.ToBase64String(iv);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Seems like something went wrong");
                    }
                }
            }
        }

        private void DESDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (DESInputString.Text.Length != 0 &&
                DESKeyTextBox.Text.Length != 0)
            {
                using (DES des = DES.Create())
                {
                    // byte[] key = Convert.FromBase64String(DESKeyTextBox.Text);
                    
                    try
                    {
                        byte[] key = Convert.FromBase64String(DESKeyTextBox.Text);
                        byte[] iv = Convert.FromBase64String(DESIvTextBox.Text);
                        string ciphertext = DESInputString.Text;
                        string decryptedText = Crypt.DESAlg.Decrypt(ciphertext, key, iv, ((ComboBoxItem)DESCryptoMode.SelectedItem).Content.ToString());
                        DESresultString.Text = decryptedText;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Seems like something went wrong");
                    }
                }
            }
        }

        private void AESuploadInputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _inputFileAES = openFileDialog.FileName;
                AESInputString.Text = openFileDialog.FileName;
                AESInputString.IsEnabled = false;
                AESDeleteFile.Visibility = Visibility.Visible;
            }
        }

        private void AESuploadKeyFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _inputKeyFileAES = openFileDialog.FileName;
                AESKeyTextBox.Text = openFileDialog.FileName;
                AESKeyTextBox.IsEnabled = false;
                AESKeyDeleteFile.Visibility = Visibility.Visible;
            }
        }

        private void AESuploadIvFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _inputIvFileAES = openFileDialog.FileName;
                AESIvTextBox.Text = openFileDialog.FileName;
                AESIvTextBox.IsEnabled = false;
                AESIvDeleteFile.Visibility = Visibility.Visible;
            }
        }

        private void AEScopyResultBox_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(AESResultString.Text);
        }

        private void DEScopyResultBox_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DESresultString.Text);
        }

        private void AESKeyDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            AESKeyDeleteFile.Visibility = Visibility.Hidden;
            AESKeyTextBox.Text = "";
            AESKeyTextBox.IsEnabled = true;
            _inputKeyFileAES = null;
        }

        private void AESIvDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            AESIvDeleteFile.Visibility = Visibility.Hidden;
            AESIvTextBox.Text = "";
            AESIvTextBox.IsEnabled = true;
            _inputIvFileAES = null;
        }

        private void AESDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            AESDeleteFile.Visibility = Visibility.Hidden;
            AESInputString.Text = "";
            AESInputString.IsEnabled = true;
            _inputFileAES = null;
        }
    }
}