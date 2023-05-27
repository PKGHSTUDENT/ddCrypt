using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ddCrypt.Pages
{
    /// <summary>
    /// Interaction logic for Asymmetric.xaml
    /// </summary>
    public partial class Asymmetric : Page
    {
        public Asymmetric()
        {
            InitializeComponent();
        }

        private void RSAEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var cryptoServiceProvider = new RSACryptoServiceProvider(Convert.ToInt32(RSAKeySizeSlider.Value));
            var privateKey = cryptoServiceProvider.ExportParameters(true);
            var publicKey = cryptoServiceProvider.ExportParameters(false);

            string publicKeyString = RSAGetKeyString(publicKey);
            string privateKeyString = RSAGetKeyString(privateKey);

            RSAPublicKeyTextBox.Text = publicKeyString;
            RSAPrivateKeyTextBox.Text = privateKeyString;

            try
            {
                string encryptedText = RSAEncrypt(RSAInputString.Text, publicKeyString);
                RSAResultStrings.Text = encryptedText;

            } catch
            {
                MessageBox.Show("Введите сообщение меньшей длины или увеличьте длину ключа.");
            }

        }

        private void RSADecrypt_Click(object sender, RoutedEventArgs e)
        {
            string decryptedText = RSADecrypt(RSAInputString.Text, RSAPrivateKeyTextBox.Text);

            RSAResultStrings.Text = decryptedText;
        }

        public static string RSAGetKeyString(RSAParameters publicKey)
        {

            var stringWriter = new System.IO.StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }

        public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(publicKeyString.ToString());
                    var encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string RSADecrypt(string textToDecrypt, string privateKeyString)
        {
            var bytesToDescrypt = Encoding.UTF8.GetBytes(textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {

                    rsa.FromXmlString(privateKeyString);

                    var resultBytes = Convert.FromBase64String(textToDecrypt);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        private void RSAKeyPromt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The larger the key size, the longer the encryption time.");
        }
    }
}
