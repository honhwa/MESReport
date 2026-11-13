using Innolux.Core.Extensions.ApiModel;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.FileOperation
{
    public class FileDownloadBiz
    {
        //多文件压缩成zip档
        //public (string fileType, byte[] archiveData, string archiveName) FileDownload(string FileID)
        //{
        //    string str1 = "";
        //    PlatformID _PlatformID = Environment.OSVersion.Platform;
        //    if (_PlatformID == PlatformID.Win32NT)
        //    {
        //        str1 = System.AppDomain.CurrentDomain.BaseDirectory;
        //        str1 = str1.Replace("Innolux.Api.exe", "");
        //    }
        //    str1 = System.AppDomain.CurrentDomain.BaseDirectory;
        //    str1 = str1.Replace("Innolux.Api.exe", "") + "uploadtemp/";
        //    //var root = new DirectoryInfo(str1 + FileID);
        //    //return root.GetFiles();
        //    var zipName = $"archive-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.zip";
        //    if(FileID != null)
        //    {

        //    }
        //    var files = Directory.GetFiles(Path.Combine(str1, FileID)).ToList();
        //    //DirectoryInfo root = new DirectoryInfo(str1 + FileID);
        //    //var files = root.GetFiles().ToList();
        //    using var memoryStream = new MemoryStream();
        //    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //    {
        //        files.ForEach(file =>
        //        {
        //            var theFile = archive.CreateEntry(Path.GetFileName(file));
        //            using var binaryWriter = new BinaryWriter(theFile.Open());
        //            binaryWriter.Write(File.ReadAllBytes(file));
        //        });
        //    }

        //    return ("application/zip", memoryStream.ToArray(), zipName);
        //}
        /// <summary>
        /// 单个文件下载
        /// </summary>
        /// <param name="FileID">文件的id（本地路由）</param>
        /// <returns></returns>
        public (string fileType, FileStream archiveData, string archiveName) FileDownload(string FileID)
        {
            string str1 = "";
            PlatformID _PlatformID = Environment.OSVersion.Platform;
            if (_PlatformID == PlatformID.Win32NT)
            {
                str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                str1 = str1.Replace("Innolux.Api.exe", "");
            }
            str1 = System.AppDomain.CurrentDomain.BaseDirectory;
            str1 = str1.Replace("Innolux.Api.exe", "") + AppConfig.Innodriver_FileAddress;
            //var root = new DirectoryInfo(str1 + FileID);
            //return root.GetFiles();
            //var zipName = $"archive-{DateTime.Now:yyyy_MM_dd-HH_mm_ss}.zip";
            //if (FileID != null)
            //{

            //}
            //var files = Directory.GetFiles(Path.Combine(str1, FileID)).ToList();
            ////DirectoryInfo root = new DirectoryInfo(str1 + FileID);
            ////var files = root.GetFiles().ToList();
            using var memoryStream = new MemoryStream();
            //using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            //{
            //    files.ForEach(file =>
            //    {
            //        var theFile = archive.CreateEntry(Path.GetFileName(file));
            //        using var binaryWriter = new BinaryWriter(theFile.Open());
            //        binaryWriter.Write(File.ReadAllBytes(file));
            //    });
            //}
            //using var binaryWriter = new BinaryWriter(File.Open(str1+ FileID , FileMode.OpenOrCreate));
            //binaryWriter.Write(File.ReadAllBytes(str1 + FileID));
            FileStream stream = new FileStream(str1 + FileID, FileMode.Open);
            return ("application/octet-stream", stream, FileID);
        }
        public string GetDownLoadUrl(string Host, string DownloadTicket)
        {
            string url = "http://" + Host + "/api/Interface/DownloadFile?DownloadTicket=" + DownloadTicket;
            return url;
        }
    }
}
