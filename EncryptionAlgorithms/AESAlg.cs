using System.IO;
using System.Security.Cryptography;

namespace ddCrypt.EncryptionAlgorithms
{
    class AESAlg
    {
        public static void AESEncryptFile(Stream input, Stream output, byte[] key, byte[] iv, string mode)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;

                aes.Mode = CipherMode.CBC;
                switch (mode)
                {
                    case "CFB":
                        aes.Mode = CipherMode.CFB;
                        break;
                    case "CTS":
                        aes.Mode = CipherMode.CTS;
                        break;
                    case "ECB":
                        aes.Mode = CipherMode.ECB;
                        break;
                }

                using (CryptoStream cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    output.Write(aes.IV, 0, aes.IV.Length);
                    output.Write(aes.Key, 0, aes.Key.Length);

                    input.CopyTo(cryptoStream);
                }
            }
        }

        public static void AESDecryptFile(Stream input, Stream output, byte[] key, byte[] iv, string mode)
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;

                aes.Padding = PaddingMode.PKCS7;


                aes.Mode = CipherMode.CBC;
                switch (mode)
                {
                    case "CFB":
                        aes.Mode = CipherMode.CFB;
                        break;
                    case "CTS":
                        aes.Mode = CipherMode.CTS;
                        break;
                    case "ECB":
                        aes.Mode = CipherMode.ECB;
                        break;
                }

                using (CryptoStream cryptoStream = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(output);
                }
            }
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV, string mode)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\aes-encrypt-temp.dat"))
            {
                var file = File.Create("input.txt");
                file.Close();

                File.WriteAllText("input.txt", plainText);

                FileEncryptDecrypt.AESEncryptFile("input.txt", "aes-encrypt-temp.dat", "AES", Key, IV, mode);
            }
            
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;

                aesAlg.Padding = PaddingMode.PKCS7;
                switch (mode)
                {
                    case "CFB":
                        aesAlg.Mode = CipherMode.CFB;
                        break;
                    case "CTS":
                        aesAlg.Mode = CipherMode.CTS;
                        break;
                    case "ECB":
                        aesAlg.Mode = CipherMode.ECB;
                        break;
                }

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV, string mode)
        {
            string? plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.PKCS7;

                aesAlg.Mode = CipherMode.CBC;
                switch (mode)
                {
                    case "CFB":
                        aesAlg.Mode = CipherMode.CFB;
                        break;
                    case "CTS":
                        aesAlg.Mode = CipherMode.CTS;
                        break;
                    case "ECB":
                        aesAlg.Mode = CipherMode.ECB;
                        break;
                }

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
