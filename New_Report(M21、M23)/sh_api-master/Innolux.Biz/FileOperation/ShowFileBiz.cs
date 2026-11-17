using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using Innolux.Core.Extensions.ApiModel;

namespace Innolux.Biz.FileOperation
{
    public class ShowFileBiz
    {
        public List<MSG_Result> ShowFile(string? parentId)
        {
            //Console.WriteLine("success1");
            List<MSG_Result> result = new List<MSG_Result>();
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
            DirectoryInfo root = new DirectoryInfo(str1 + AppConfig.Innodriver_FileAddress + parentId);
            DirectoryInfo[] dics = root.GetDirectories();
            foreach (DirectoryInfo dic in dics)
            {
                MSG_Result mSG_Result = new MSG_Result();
                string[] nametemp = dic.ToString().Split("/");
                string[] nametemp1 = dic.ToString().Split("\\");
                string name = "";
                if (nametemp.Length > 1)
                {
                    name = nametemp[nametemp.Length - 1];
                }
                else if (nametemp1.Length > 1)
                {
                    name = nametemp1[nametemp1.Length - 1];
                }
                mSG_Result.nodeId = "not use";
                mSG_Result.id = "not use";
                mSG_Result.name = name;
                mSG_Result.type = "D";
                if (parentId != null)
                {
                    if (parentId.Length > 0)
                    {
                        parentId += "/";
                    }
                }
                mSG_Result.parentId = parentId + name;
                mSG_Result.bytes = 0;
                result.Add(mSG_Result);
                //Console.WriteLine(dic);
            }
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                MSG_Result mSG_Result = new MSG_Result();
                string[] nametemp = file.ToString().Split("/");
                string[] nametemp1 = file.ToString().Split("\\");
                string name = "";
                if (nametemp.Length > 1)
                {
                    name = nametemp[nametemp.Length - 1];
                }
                else if (nametemp1.Length > 1)
                {
                    name = nametemp1[nametemp1.Length - 1];
                }
                //Console.WriteLine(name);
                mSG_Result.nodeId = "not use";
                mSG_Result.id = "not use";
                mSG_Result.name = name;
                mSG_Result.type = "F";
                if (parentId != null)
                {
                    if (parentId.Length > 0)
                    {
                        parentId += "/";
                    }
                }
                mSG_Result.parentId = parentId + name;
                mSG_Result.bytes = 0;
                result.Add(mSG_Result);
                //Console.WriteLine(file);
            }
            return result;
        }
    }
}
