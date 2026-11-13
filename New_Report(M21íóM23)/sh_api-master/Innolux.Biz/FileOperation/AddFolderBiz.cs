using Innolux.Core.Extensions.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Innolux.Core.Extensions.MsgAlarm.Utils;

namespace Innolux.Biz.FileOperation
{
    public class AddFolderBiz
    {
        public List<MSG_Result> AddFolder(string? parentId, string FolderName)
        {
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
            if (parentId != null && parentId.Length > 0)
            {
                parentId += "/";
            }
            if (!Directory.Exists(str1 + AppConfig.Innodriver_FileAddress + parentId + FolderName))
            {
                Directory.CreateDirectory(str1 + AppConfig.Innodriver_FileAddress + parentId + FolderName);
            }
            MSG_Result mSG_Result = new MSG_Result();
            mSG_Result.nodeId = "not use";
            mSG_Result.id = "not use";
            mSG_Result.name = FolderName;
            mSG_Result.type = "D";
            mSG_Result.parentId = parentId + FolderName;
            mSG_Result.bytes = 0;
            result.Add(mSG_Result);
            return result;
        }
    }
}
