using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.Runtime.InteropServices;   // 因需使用DllImport 
using System.Security.Permissions;      // 因需使用SecurityPermissionAttribute PermissionSetAttribute

namespace WebReport
{
    /// <summary>
    /// Summary description for getImage
    /// </summary>
    public class getImage : IHttpHandler //,System.Web.SessionState.IReadOnlySessionState
    {
        /// <summary>
        /// 1.如需要在ashx裡讀取Session，所以必須實現IReadOnlySessionState介面。如果要寫Session，就要實現IRequiresSessionState介面。
        /// 2.ProcessRequest的參數是HttpContext，所以使用Reponse或者Session，請必須使用context來訪問。
        /// </summary>
        /// <param name="context"></param>
       

        public void ProcessRequest(HttpContext context)
        {
            
            string strId=context.Request.QueryString["Id"].ToString();
            string strLine = context.Request.QueryString["Line"].ToString();
            //string strServer = @"P:\";
            string strServer = @"\\10.119.8.93\bmw_assly_aoi\";
            string path = @"2F\" + strLine + @"\"+strId+@"\";
            
            string[] lst = Directory.GetFiles(strServer + path, "*.jpg");
            List<string> lstFile = lst.OrderBy(ss => new FileInfo(ss).CreationTime).ToList();
            string lastFileName = "";
            if (lstFile.Count > 0)
            {
                lastFileName = lstFile[lstFile.Count - 1].ToString();
            }
            if (File.Exists(lastFileName))
            {
                context.Response.ClearContent();
                context.Response.ContentType = "image/Jpeg";
                context.Response.WriteFile(lastFileName);
            }
            else
            {
                context.Response.WriteFile(@".\Image\FileNotExist.JPG");
            }
               
            

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        
    }
}