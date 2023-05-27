using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ddCrypt.EncryptionAlgorithms
{
    internal class DESAlg
    {
        public static void Encrypt(string plaintext, out string ciphertext, out byte[] key, out byte[] iv, string mode)
        {
            // Generate a random 8-byte key and initialization vector (IV)
            using (var des = DES.Create())
            {
                des.GenerateKey();
                des.GenerateIV();
                key = des.Key;
                iv = des.IV;
                des.Mode = CipherMode.CBC;
                switch (mode)
                {
                    case "ECB":
                        des.Mode = CipherMode.ECB;
                        break;
                }

                // Create an encryptor using the key and IV
                ICryptoTransform encryptor = des.CreateEncryptor();

                // Convert the plaintext string to a byte array
                byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);

                // Perform the encryption
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Convert the encrypted bytes to a base64 string
                ciphertext = Convert.ToBase64String(cipherBytes);
            }
        }


        public static string Decrypt(string ciphertext, byte[] key, byte[] iv, string mode)
        {
            // Create a new instance of the DES algorithm
            using (var des = DES.Create())
            {
                // Set the key and IV
                des.Key = key;
                des.IV = iv;

                des.Mode = CipherMode.CBC;
                switch (mode)
                {
                    case "ECB":
                        des.Mode = CipherMode.ECB;
                        break;
                }

                // Create a decryptor using the key and IV
                ICryptoTransform decryptor = des.CreateDecryptor();

                // Convert the ciphertext from base64 to bytes
                byte[] cipherBytes = Convert.FromBase64String(ciphertext);

                // Perform the decryption
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                // Convert the decrypted bytes to a string
                string decryptedText = Encoding.UTF8.GetString(plainBytes);

                return decryptedText;
            }
        }


        static public string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
