using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Report.Library
{
    //AES(進階加密標準) : Advanced Encryption Standard
    //CBC(密碼塊連結) : Cipher Block Chaining (在CBC模式中，每個明文塊先與前一個密文塊進行互斥或後，再進行加密)
    //PBKDF2(基於密碼的金鑰延伸函式) : Password-Based Key Derivation Function 2 (RFC2898)
    public class AESCbcPbkdf2
    {
        //https://stackoverflow.com/questions/76508136/c-sharp-encrypt-decrypt-aes-256-cbc-with-pbkdf2-from-openssl

        /// <summary>
        /// Encrypt with AES-256-CBC , PBKDF2
        /// equivalent to "openssl aes256 -pbkdf2 -md sha512 -pass pass:{sPassword} -in {sText}.file -a"
        /// </summary>
        /// <param name="plaintext">欲加密字串</param>
        /// <param name="passphrase">明文密鑰</param>
        /// <returns></returns>
        public static string EncryptAesCbc(string plaintext, string passphrase)
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] salt = GenerateRandomBytes(8);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(passphrase);

            byte[] keyBytes = new byte[32];
            //初始化向量
            byte[] ivBytes = new byte[16];


            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 10000, HashAlgorithmName.SHA512))
            {
                keyBytes = rfc2898DeriveBytes.GetBytes(32);
                ivBytes = rfc2898DeriveBytes.GetBytes(16);
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plaintextBytes, 0, plaintextBytes.Length);
                    }

                    byte[] encryptedBytes = memoryStream.ToArray();

                    byte[] encryptedData = new byte[8 + salt.Length + encryptedBytes.Length];
                    //for openssl compatibility : Salted__
                    Buffer.BlockCopy(Encoding.UTF8.GetBytes("Salted__"), 0, encryptedData, 0, 8);
                    Buffer.BlockCopy(salt, 0, encryptedData, 8, salt.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, encryptedData, 8 + salt.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(encryptedData);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateRandomBytes(int length)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                return randomBytes;
            }
        }

        //https://stackoverflow.com/questions/68391070/decrypt-aes-256-cbc-with-pbkdf2-from-openssl-in-c-sharp
        //equivalent to "openssl aes256 -pbkdf2 -md sha512 -pass pass:{sPassword} -in {sEncr}.file -a -d"
        /// <summary>
        ///  Decrypt with AES-256-CBC , PBKDF2
        /// </summary>
        /// <param name="cipherData">解Base64後加密byte</param>
        /// <param name="passphrase">明文密鑰</param>
        /// <returns></returns>
        public static string DecryptAesCBC(byte[] cipherData, byte[] passphrase)
        {
            string decrypted = null;
            using (var ms = new MemoryStream(cipherData))
            {
                // Get salt
                var salt = new byte[8];
                ms.Seek(8, SeekOrigin.Begin);
                ms.Read(salt, 0, 8);

                // Derive key and IV
                var pbkdf2 = new Rfc2898DeriveBytes(passphrase, salt, 10000, HashAlgorithmName.SHA512);
                byte[] key = pbkdf2.GetBytes(32);
                byte[] iv = pbkdf2.GetBytes(16);

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key;
                    aes.IV = iv;

                    // Decrypt
                    ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs, Encoding.UTF8))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }

        /// <summary>
        ///  Decrypt with AES-256-CBC , PBKDF2
        /// </summary>
        /// <param name="cipherBase64String">Base64加密字串</param>
        /// <param name="passphrase">明文密鑰</param>
        /// <returns></returns>
        public static string DecryptAesCBC(string cipherBase64String, string passphrase)
        {
            string decrypted = null;
            byte[] input = Convert.FromBase64String(cipherBase64String);
            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            using (var ms = new MemoryStream(input))
            {
                // Get salt
                var salt = new byte[8];
                ms.Seek(8, SeekOrigin.Begin);
                ms.Read(salt, 0, 8);

                // Derive key and IV
                var pbkdf2 = new Rfc2898DeriveBytes(passphrase, salt, 10000, HashAlgorithmName.SHA512); //Rfc2898 = PBKDF2
                byte[] key = pbkdf2.GetBytes(32);
                byte[] iv = pbkdf2.GetBytes(16);

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key;
                    aes.IV = iv;

                    // Decrypt
                    ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs, Encoding.UTF8))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }
    }
}