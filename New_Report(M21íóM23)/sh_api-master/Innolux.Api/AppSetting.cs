using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Text;
using System;
using static System.Net.WebRequestMethods;
using System.IO;

namespace Innolux.Api
{
    /// <summary>
    /// appsettings.json操作类(获取等)
    /// </summary>
    public class AppSetting
    {
        public static void Delete(string Path)
        {
            //获取操作系统平台的PlatformID枚举值
            string str1 = "";
            PlatformID _PlatformID = Environment.OSVersion.Platform;
            if (_PlatformID == PlatformID.Win32NT)
            {
                str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                str1 = str1.Replace("Innolux.Api.exe", "");
            }
            str1 = System.AppDomain.CurrentDomain.BaseDirectory;
            str1 = str1.Replace("Innolux.Api.exe", "");
            if (Directory.Exists(str1 + Path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(str1 + Path);
                directoryInfo.Delete(true);
            }
        }

        static IConfiguration Configuration { get; set; }
        static string ContentPath { get; set; }
        
        public AppSetting()
        {
            string path = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(ContentPath).Add(new JsonConfigurationSource
            {
                Path = path,
                Optional = false,
                ReloadOnChange = true
            }).Build();
        }
        public AppSetting(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获取指定属性值
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    if (Configuration != null)
                    {
#pragma warning disable CS8603 // 可能返回 null 引用。
                        return Configuration[string.Join(":", sections)];
#pragma warning restore CS8603 // 可能返回 null 引用。
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        /// <summary>
        /// 获取指定对象值
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
        /// <summary>
        /// 生成html文件
        /// </summary>
        /// <param name="filePath">保存的目录</param>
        /// <param name="fileName">保存的html文件名</param>
        /// <param name="content">html内容</param>
        public static void WriteToHtml(string filePath, string fileName, string content)
        {
            Directory.Exists(filePath);
            string path = Path.Combine(filePath, fileName);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(content);
                }
            }
        }
        public static void ChangeFile()
        {
            //var filepath = $@"{Environment.CurrentDirectory}/Resource/System//".Replace("\\", " / ");
            string Path = Environment.ProcessPath;
            Path = Path.Replace(@"\", "/");
            Path = Path.Replace("/Innolux.Api.exe", "");
            string toPath = Path + "/AppConfig/apiWorkList.html";
            //string toPath = Path.Replace(@"\", "/");
            //toPath = toPath.Replace("/bin/Debug/net6.0/Innolux.Api.exe", "/documents/apiWorkList.html");
            //string toPath1 = Path.Replace(@"\", "/");
            //toPath1 = toPath1.Replace("/bin/Debug/net6.0/Innolux.Api.exe", "/documents/sqlAgentTest.html");
            //Path = Path.Replace(@"\", "/");
            //Path = Path.Replace("/Innolux.Api.exe", "");
            saveFile(toPath, Path);
        }
        /// <summary>
        /// 复制文件到指定目录
        /// </summary>
        /// <param name="filePathName">将要被复制的文件的完整路径</param>
        /// <param name="toFilesPath">复制到所指定的文件夹的完整路径</param>
        public static void saveFile(string filePathName, string toFilesPath)
        {
            try
            {
                System.IO.File.Move(toFilesPath + "/AppConfig/apiWorkList.json", toFilesPath + "/AppConfig/apiWorkList.html");
                FileInfo file = new FileInfo(filePathName);
                string newName = file.Name;
                file.CopyTo(toFilesPath + @"/" + newName, true);
            }
            catch(Exception ex)
            {
                return;
            }    
        }
    }
}