using Innolux.Core.Extensions.ApiModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.FileOperation
{
    public class DeleteFileBiz
    {
        public string DeleteFile(string parentId)
        {
            string result = "fail";
            string str1 = "";
            PlatformID _PlatformID = Environment.OSVersion.Platform;
            if (_PlatformID == PlatformID.Win32NT)
            {
                str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                str1 = str1.Replace("Innolux.Api.exe", "");
            }
            str1 = System.AppDomain.CurrentDomain.BaseDirectory;
            str1 = str1.Replace("Innolux.Api.exe", "") + AppConfig.Innodriver_FileAddress;
            if (File.Exists(str1 + parentId))
            {
                // 创建要写入的文件.
                File.Delete(str1 + parentId);
                result = "success";
            }
            return result;
        }
    }
}
