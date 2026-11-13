using System.Security.Cryptography;
using System.Text;

namespace Innolux.Core.Extensions.EncryptionDecrypt
{
    /// <summary>
    /// 加密
    /// </summary>
    internal class Encryption
    {
        public string encryption(string PlainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                //加密金鑰(32 Byte)
                aesAlg.Key = Encoding.Unicode.GetBytes("我是金鑰我是機密別和人說我是金鑰");
                //初始向量(Initial Vector, iv) 類似雜湊演算法中的加密鹽(16 Byte)
                aesAlg.IV = Encoding.Unicode.GetBytes("台塩高級精鹽加碘");
                //加密器
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                //執行加密
                byte[] encrypted = encryptor.TransformFinalBlock(Encoding.Unicode.GetBytes(PlainText), 0,
        Encoding.Unicode.GetBytes(PlainText).Length);
                return Convert.ToBase64String(encrypted);
            }
        }
    }
}
