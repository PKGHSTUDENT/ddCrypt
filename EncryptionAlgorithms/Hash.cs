using System;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace ddCrypt.EncryptionAlgorithms
{
    internal class Hash
    {
        static public string GetHash(string algHash, string? filePath = null, string? inputText = null)
        {
            if (filePath != null)
            {
                try
                {
                    switch (algHash)
                    {
                        case "SHA1":
                            return GetSHA1FileHash(filePath);
                        case "SHA256":
                            return GetSHA256FileHash(filePath);
                        case "SHA384":
                            return GetSHA384FileHash(filePath);
                        case "SHA512":
                            return GetSHA512FileHash(filePath);
                        case "MD5":
                            return GetMD5FileHash(filePath);
                        default:
                            return GetMD5FileHash(filePath);
                    }
                } catch (FileNotFoundException ex)
                {
                    MessageBox.Show("Файл не найден.", "Ваш файл не найден. Пожалуйста, проверье его наличие.");
                }
            }

            switch (algHash)
            {
                case "SHA1":
                    return GetSHA1Hash(inputText);
                case "SHA256":
                    return GetSHA256Hash(inputText);
                case "SHA384":
                    return GetSHA384Hash(inputText);
                case "SHA512":
                    return GetSHA512Hash(inputText);
                case "MD5":
                    return GetMD5Hash(inputText);
                default:
                    return GetMD5Hash(inputText);
            }
        }

        static public string GetSHA1Hash(string inputText)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA1FileHash(string filePath)
        {
            using (SHA1 sha1 = SHA1.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha1.ComputeHash(fileStream);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA256Hash(string inputText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA256FileHash(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha256.ComputeHash(fileStream);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA384Hash(string inputText)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
                byte[] hashBytes = sha384.ComputeHash(inputBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA384FileHash(string filePath)
        {
            using (SHA384 sha384 = SHA384.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha384.ComputeHash(fileStream);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }


        static public string GetSHA512Hash(string inputText)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetSHA512FileHash(string filePath)
        {
            using (SHA512 sha512 = SHA512.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha512.ComputeHash(fileStream);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetMD5Hash(string inputText)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }

        static public string GetMD5FileHash(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = md5.ComputeHash(fileStream);
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashString;
            }
        }
    }
}
