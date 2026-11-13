using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.FileOperation
{
    public class FileUploadBiz
    {
        public List<MSG_Result> UploadFilesBiz(List<IFormFile> files, string parentId, bool overwrite)
        {
            Console.WriteLine("success1");
            List<MSG_Result> result = new List<MSG_Result>();
            foreach (var file in files)
            {
                MSG_Result mSG_Result = new MSG_Result();
                //打开请求流以获取上传的文件
                StreamReader reader = new StreamReader(file.OpenReadStream());
                //从流的当前位置到末尾读取所有字符，如果当前位置位于流的末尾，则返回空字符串“”
                String content = reader.ReadToEnd();
                //获取文件的名称
                String name = file.FileName;
                //保存文件的路径
                string str1 = "";
                PlatformID _PlatformID = Environment.OSVersion.Platform;
                if (_PlatformID == PlatformID.Win32NT)
                {
                    str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                    str1 = str1.Replace("Innolux.Api.exe", "");
                }
                str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                str1 = str1.Replace("Innolux.Api.exe", "");
                if (!Directory.Exists(str1 + AppConfig.Innodriver_FileAddress))
                {
                    Directory.CreateDirectory(str1 + AppConfig.Innodriver_FileAddress);
                }
                String filename = str1 + AppConfig.Innodriver_FileAddress + parentId + name;
                Console.WriteLine(filename);
                //if (System.IO.File.Exists(filename))
                //{
                // System.IO.File.Delete(filename);
                //}
                mSG_Result.nodeId = "not use";
                mSG_Result.id = "not use";
                mSG_Result.name = name;
                mSG_Result.type = "F";
                mSG_Result.parentId = parentId + name;
                mSG_Result.bytes = file.Length;
                result.Add(mSG_Result);
                //在指定路径中创建或覆盖文件
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
            }
            return result;
        }
    }
}
