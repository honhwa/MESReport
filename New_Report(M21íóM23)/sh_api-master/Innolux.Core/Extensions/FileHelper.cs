using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Http;

namespace Innolux.Core.Extensions
{
    public static class FileHelper
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="paths"></param>
        public static async Task<string> SaveFileAsync(IFormFile file, List<string> paths)
        {
            if (file.Length <= 0)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.Fail, "文件大小不能為0"));
            }
            string FileName = file.FileName; //文件名

            foreach (string path in paths)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    //创建文件夹
                    System.IO.Directory.CreateDirectory(path);
                }
                var filePath = path + FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))

                {

                    await file.CopyToAsync(stream);
                }
            }

            return FileName;



        }
    }
}
