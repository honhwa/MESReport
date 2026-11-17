using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.FileOperation
{
    public class MSG_Result
    {
        public string nodeId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string parentId { get; set; }
        public long bytes { get; set; }
    }
    public class FileUpload_Get
    {
        public string parentId { get; set; }
        public bool overwrite { get; set; }
        public object file { get; set; }
    }
    public class Items_Get
    {
        public string parentId { get; set; }
    }
    public class FileURL_Get
    {
        public string FileID { get; set; }
    }
    public class DownLoad_Get
    {
        public string DownloadTicket { get; set; }
    }
    public class Tiket
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
