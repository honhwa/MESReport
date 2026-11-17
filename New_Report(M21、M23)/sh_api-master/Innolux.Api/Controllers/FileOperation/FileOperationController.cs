using Innolux.Biz.Common;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using NetTaste;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.FileOperation
{
    [ApiExplorerSettings(GroupName = "Demo")]
    [Route("api/Interface")]
    [ApiController]
    public class FileOperationController : ControllerBase
    {
        private readonly FileUploadBiz _FileUploadBiz;
        private readonly ShowFileBiz _ShowFileBiz;
        private readonly FileDownloadBiz _FileDownloadBiz;
        private readonly ExcelUploadBiz _ExcelUploadBiz;
        private readonly DeleteFileBiz _DeleteFileBiz;
        private readonly AddFolderBiz _AddFolderBiz;
        public FileOperationController(FileUploadBiz fileUploadBiz, ShowFileBiz showFileBiz, FileDownloadBiz fileDownloadBiz, ExcelUploadBiz excelUploadBiz, DeleteFileBiz deleteFileBiz, AddFolderBiz addFolderBiz)
        {
            _FileUploadBiz = fileUploadBiz;
            _ShowFileBiz = showFileBiz;
            _FileDownloadBiz = fileDownloadBiz;
            _ExcelUploadBiz = excelUploadBiz;
            _DeleteFileBiz = deleteFileBiz;
            _AddFolderBiz = addFolderBiz;
        }
        //[Authorize("checkToken")]
        [HttpPost]
        [Route("UploadFiles")]
        public List<MSG_Result> UploadFiles([FromHeader] string ticket, [FromForm] List<IFormFile> files, string? parentId, bool overwrite = true)
        {
            List<MSG_Result> result = new List<MSG_Result>();

            //string xtoken = Request.Headers["token"].ToString();
            if (ticket == "Yes")
            {
                Console.WriteLine("success1");
                result = _FileUploadBiz.UploadFilesBiz(files, parentId, overwrite);
                return result;
            }
            else
            {
                return result;
            }
        }
        [HttpPost]
        [Route("GetItems")]
        public List<MSG_Result> GetItems([FromHeader] string ticket, [FromForm] string? parentId)
        {
            List<MSG_Result> result = new List<MSG_Result>();
            //string xtoken = Request.Headers["token"].ToString();
            if (ticket == "Yes")
            {
                result = _ShowFileBiz.ShowFile(parentId);
                return result;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DownloadTicket"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownloadFile")]
        public IActionResult DownloadFile([Required] string DownloadTicket)
        {
            //string xtoken = Request.Headers["token"].ToString();
            List<MSG_Result> result = new List<MSG_Result>();
            FileInfo[] bytes;
            var (fileType, archiveData, archiveName) = _FileDownloadBiz.FileDownload(DownloadTicket);
            return File(archiveData, fileType, archiveName);
        }
        [HttpPost]
        [Route("GetDownloadFileURL")]
        public ActionResult<string> GetDownloadFileURL([FromHeader] string ticket, [FromForm] string FileID)
        {
            //string xtoken = Request.Headers["token"].ToString();
            if (ticket == "Yes")
            {
                List<MSG_Result> result = new List<MSG_Result>();
                FileInfo[] bytes;
                var url = _FileDownloadBiz.GetDownLoadUrl(Request.Host.ToString(), FileID);
                return url;
            }
            else
            {
                return BadRequest("身份验证失败");
            }
        }
        [HttpPost]
        [Route("DeleteFile")]
        public ActionResult<string> DeleteFile([FromHeader] string ticket, [FromForm] string FileID)
        {
            //string xtoken = Request.Headers["token"].ToString();
            if (ticket == "Yes")
            {
                List<MSG_Result> result = new List<MSG_Result>();
                FileInfo[] bytes;
                var url = _DeleteFileBiz.DeleteFile(FileID);
                return url;
            }
            else
            {
                return BadRequest("身份验证失败");
            }
        }
        [HttpPost]
        [Route("AddFolder")]
        public List<MSG_Result> AddFolder([FromHeader] string ticket, [FromForm] string? parentId, [FromForm] string FolderName)
        {
            List<MSG_Result> result = new List<MSG_Result>();
            //string xtoken = Request.Headers["token"].ToString();
            if (ticket == "Yes")
            {
                FileInfo[] bytes;
                result = _AddFolderBiz.AddFolder(parentId, FolderName);
                return result;
            }
            else
            {
                return result;
            }
        }
        [HttpPost]
        [Route("GetAPTicket")]
        public ActionResult<string> GetAPTicket([FromBody] Tiket tiket)
        {
            //string xtoken = Request.Headers["token"].ToString();
            return "Yes";
        }
        //[HttpPost]
        //[Route("UploadExcel")]
        //public ActionResult<ApiResult> UploadExcel([FromHeader] string Token, [FromForm] List<IFormFile> files, string? parentId)
        //{
        //    //string xtoken = Request.Headers["token"].ToString();
        //    if (Token == "Yes")
        //    {
        //        string result = _ExcelUploadBiz.UploadFilesBiz(files, parentId);
        //        return result.Length > 0 ? ApiResult.Ok(result) : ApiResult.Fail("上传失败");
        //    }
        //    else
        //    {
        //        return ApiResult.Fail("身份验证失败");
        //    }
        //}
    }
}
