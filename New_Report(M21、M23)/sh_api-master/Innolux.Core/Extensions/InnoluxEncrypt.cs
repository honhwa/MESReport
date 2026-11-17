using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using System.Security.Cryptography;
using System.Text;

namespace Innolux.Core.Extensions
{

    public class InnoluxEncrypt
    {
        private static readonly object _instanceLock = new object();
        private static InnoluxEncrypt _instance = null;
        private static SymmetricAlgorithm mobjCryptoService;
        private static string _key;
        private static readonly object _cacheLock = new object();
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();
        /// <summary>
        /// 设置最大缓存条数,最小值为5(默认值：10000)
        /// </summary>
        /// <param name="i"></param>
        public void SetMaxCacheNum(int i = 5)
        {
            if (i < 5) i = 5;
            _maxCacheNum = 0;
        }
        /// <summary>
        /// 最大缓存条数
        /// </summary>
        private static int _maxCacheNum = 10000;

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public static InnoluxEncrypt GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        mobjCryptoService = new RijndaelManaged();
                        _key = "Guz(%&H7";
                        _instance = new InnoluxEncrypt();
                    }
                }
            }
            return _instance;
        }



        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public string Encrypto(string source)
        {
            lock (_cache)
            {
                if (_cache.ContainsKey(source))
                {
                    _cache.Remove(source);
                    //return _cache[source];
                }

                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);

                MemoryStream ms = new MemoryStream();

                mobjCryptoService.Key = GetLegalKey();

                mobjCryptoService.IV = GetLegalIV();

                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

                cs.Write(bytIn, 0, bytIn.Length);

                cs.FlushFinalBlock();

                ms.Close();

                byte[] bytOut = ms.ToArray();
                string reval = Convert.ToBase64String(bytOut);
                lock (_cacheLock)
                {
                    if (_cache.Count > _maxCacheNum)
                    {
                        foreach (var it in _cache.Take(_maxCacheNum / 5))
                        {

                            _cache.Remove(it.Key);

                        }
                    }
                    _cache.Add(source, reval);
                }
                return reval; ;
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public string Decrypto(string source)
        {
            try
            {
                lock (_cache)
                {
                    lock (_cacheLock)
                    {
                        if (_cache.Any(it => it.Value == source))
                        {
                            return _cache.Single(it => it.Value == source).Key;
                        }
                    }

                    byte[] bytIn = Convert.FromBase64String(source);

                    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

                    mobjCryptoService.Key = GetLegalKey();

                    mobjCryptoService.IV = GetLegalIV();

                    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

                    StreamReader sr = new StreamReader(cs);

                    return sr.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {


                throw new InnoluxException(new ErrResult(ApiCodeEnum.InvalidToken));

            }
        }

        /// <summary>
        /// MD5加密,不可逆
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string MD5(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(source);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }

        #region 私有函数
        /// <summary>
        /// 获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private byte[] GetLegalKey()
        {

            string sTemp = _key + GenerateCheckCode();

            mobjCryptoService.GenerateKey();

            byte[] bytTemp = mobjCryptoService.Key;

            int KeyLength = bytTemp.Length;

            if (sTemp.Length > KeyLength)

                sTemp = sTemp.Substring(0, KeyLength);

            else if (sTemp.Length < KeyLength)

                sTemp = sTemp.PadRight(KeyLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);

        }

        /// <summary>
        /// 获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {

            string sTemp = "asdfas&&dfg*$#";

            mobjCryptoService.GenerateIV();

            byte[] bytTemp = mobjCryptoService.IV;

            int IVLength = bytTemp.Length;

            if (sTemp.Length > IVLength)

                sTemp = sTemp.Substring(0, IVLength);

            else if (sTemp.Length < IVLength)

                sTemp = sTemp.PadRight(IVLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(sTemp);

        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        private string GenerateCheckCode()
        {
            char[] constant =
            {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }
        #endregion
    }

}