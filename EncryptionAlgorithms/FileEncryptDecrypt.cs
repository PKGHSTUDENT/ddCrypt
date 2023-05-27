using System;
using System.IO;

namespace ddCrypt.EncryptionAlgorithms
{
    internal class FileEncryptDecrypt
    {
        public static void AESEncryptFile(string inputFile, string encryptedFile, string algorithm, byte[] key, byte[] iv, string mode)
        {
            // var file = File.Create(encryptedFile);
            // file.Close();
            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream encryptedFileStream = new FileStream(encryptedFile, FileMode.Create))
            {
                switch (algorithm)
                {
                    case "AES":
                        AESAlg.AESEncryptFile(inputFileStream, encryptedFileStream, key, iv, mode);
                        break;
                    case "RSA":
                        // AESAlg.AESDecryptFile(inputFileStream, encryptedFileStream, keyFile);
                        break;
                    default:
                        throw new ArgumentException("Неподдерживаемый алгоритм шифрования");
                }
            }
        }

        public static void AESDecryptFile(string encryptedFile, string decryptedFile, string algorithm, byte[] key, byte[] iv, string mode)
        {
            using (FileStream encryptedFileStream = new FileStream(encryptedFile, FileMode.Open))
            using (FileStream decryptedFileStream = new FileStream(decryptedFile, FileMode.Create))
            {
                switch (algorithm)
                {
                    case "AES":
                        AESAlg.AESDecryptFile(encryptedFileStream, decryptedFileStream, key, iv, mode);
                        break;
                    case "RSA":
                        // AESAlg.AESDecryptFile(encryptedFileStream, decryptedFileStream, keyFile);
                        break;
                    // Добавьте дополнительные случаи для других алгоритмов шифрования, если нужно
                    default:
                        throw new ArgumentException("Неподдерживаемый алгоритм шифрования");
                }
            }
        }
    }
}
