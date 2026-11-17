using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System.Runtime.InteropServices;

namespace Innolux.Core.Extensions
{
    /// <summary>
    /// 中文字符工具类
    /// </summary>
    public static class ChineseStringUtility
    {
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToSimplified(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target, source.Length);
            return target;
        }
        /// <summary>
        /// 使用套件進行繁體轉簡體
        /// </summary>
        /// <param name="s">繁體</param>
        /// <returns>簡體</returns>
        public static string Simplify(string s)
        {
            string t = ChineseConverter.Convert(s, ChineseConversionDirection.TraditionalToSimplified);
            return t;
        }

        /// <summary>
        /// 将字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToTraditional(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
            return target;
        }
        /// <summary>
        /// 使用套件進行簡體轉繁體
        /// </summary>
        /// <param name="s">簡體</param>
        /// <returns>繁體</returns>
        public static string Traditional(string s)
        {
            string t = ChineseConverter.Convert(s, ChineseConversionDirection.SimplifiedToTraditional);
            return t;
        }
    }
}