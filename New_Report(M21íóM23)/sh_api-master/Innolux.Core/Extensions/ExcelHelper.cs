using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Data;

namespace Innolux.Core.Extensions
{
    public sealed class ExcelHelper
    {
        //参考：https://www.jb51.net/article/100509.htm

        private ExcelHelper() { }

        /// <summary>
        /// Excel文件 Content-Type
        /// </summary>
        private const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="keyValuePairs">字典表【名称，数据】</param>
        /// <param name="tuple">item1:The virtual path of the file to be returned.|item2:The Content-Type of the file</param>
        public static void Export(Dictionary<string, DataTable> keyValuePairs, out Tuple<MemoryStream, string> tuple)
        {
            var stream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                foreach (var item in keyValuePairs)
                {
                    string worksheetTitle = item.Key; //表名称
                    var dt = item.Value; //数据表

                    // 添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                //添加表头
                                worksheet.Cells[1, j + 1].Value = dt.Columns[j].ColumnName;
                                worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                            }
                            //添加值
                            worksheet.Cells[i + 2, j + 1].Value = dt.Rows[i][j].ToString();
                        }
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            tuple = Tuple.Create(stream, Excel);
        }

        /// <summary>
        /// EXCEL保存到本地路徑
        /// </summary>
        public static void Export(DataTable dataTable, string fileName)
        {
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            dic.Add("sheet1", dataTable);
            ExcelHelper.Export(dic, out Tuple<MemoryStream, string> t);
            ExcelPackage package = new ExcelPackage(t.Item1);
            package.SaveAs(new FileInfo(fileName));
        }

        /// <summary>
        /// EXCEL輸出到瀏覽器,在Controller中使用return File(t.Item1, t.Item2, fileName + ".xlsx");
        /// </summary>
        public static void ExportByWeb(DataTable dataTable, out MemoryStream stream, out string contentType)
        {
            Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>();
            dic.Add("sheet1", dataTable);
            ExcelHelper.Export(dic, out Tuple<MemoryStream, string> t);
            stream = t.Item1;
            contentType = t.Item2;
        }

        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="excelFile">Excel文件</param>
        /// <param name="sWebRootFolder">文件存储路径</param>
        ///  <param name="content">显示内容</param>
        /// <param name="isShow">是否显示内容</param>
        //        public static void Import<T>(IFormFile excelFile, string sWebRootFolder, out string content, bool isShow = false) where T : new()
        //        {
        //            if (string.IsNullOrWhiteSpace(sWebRootFolder))
        //                content = string.Empty;
        //
        //            string sFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
        //            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
        //
        //            using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
        //            {
        //                excelFile.CopyToAsync(fs);
        //                fs.Flush();
        //            }
        //
        //            if (isShow)
        //            {
        //                //导出单个工作表sheet
        //                using (ExcelPackage package = new ExcelPackage(file))
        //                {
        //                   
        //                    List<T> list =new List<T>();
        //                    T t = new T();
        //                    PropertyInfo[] propertys = t.GetType().GetProperties();
        //                    StringBuilder sb = new StringBuilder();
        //                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //                    int rowCount = worksheet.Dimension.Rows;
        //                    int colCount = worksheet.Dimension.Columns;
        //                    bool bHeaderRow = true;
        //                    for (int row = 1; row <= rowCount; row++)
        //                    {
        //                        for (int col = 1; col <= colCount; col++)
        //                        {
        //                           
        //
        //                            if (bHeaderRow)
        //                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
        //                            else
        //                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
        //                        }
        //                        //sb.Append(Environment.NewLine);
        //                    }
        //                    content = sb.ToString();
        //                }
        //            }
        //            else
        //            {
        //                content = string.Empty;
        //            }
        //        }

        /// <summary>
        /// Excel导入,保存文件+轉成LIST
        /// </summary>
        /// <param name="excelFile">Excel文件</param>
        /// <param name="sWebRootFolder">文件存储路径</param>
        ///  <param name="content">显示内容</param>
        /// <param name="isShow">是否显示内容</param>
        public static void Import<T>(IFormFile excelFile, string sWebRootFolder, out List<T> list) where T : new()
        {
            string sFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
            {
                excelFile.CopyToAsync(fs);
                fs.Flush();
            }
            //导出单个工作表sheet
            DataTable dt = new DataTable();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                dt.TableName = worksheet.Name;
                //默认将第一行设置为datatable的标题
                for (int col = 0; col < colCount; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Value.ToString()); ;
                }

                for (int row = 0; row < rowCount; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = 0; col < colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            dataRow[row] = worksheet.Cells[row, col].Value.ToString();
                        }
                    }
                }
            }
            list = ModelConvertHelper.ConvertToModel<T>(dt);
        }

        /// <summary>
        /// Excel导入 文件轉成LIST
        /// </summary>
        /// <param name="sWebRootFolder">文件存储路径</param>
        ///  <param name="content">显示内容</param>
        /// <param name="isShow">是否显示内容</param>
        public static void Import<T>(string sWebRootFileName, out List<T> list) where T : new()
        {
            FileInfo file = new FileInfo(Path.Combine(sWebRootFileName));

            //导出单个工作表sheet
            DataTable dt = new DataTable();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                dt.TableName = worksheet.Name;
                //默认将第一行设置为datatable的标题
                for (int col = 1; col <= colCount; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Value.ToString()); ;
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            dataRow[col - 1] = worksheet.Cells[row, col].Value.ToString();
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
            }
            list = ModelConvertHelper.ConvertToModel<T>(dt);
        }
    }
}
