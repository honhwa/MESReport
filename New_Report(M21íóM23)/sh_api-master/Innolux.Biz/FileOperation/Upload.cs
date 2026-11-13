using ExcelDataReader;
using InnSqlAgent;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using static InnSqlAgent.SqlAgent;

namespace UploadDownloadSample
{
    public class Upload : InnSqlAgent.IApiBase
    {
        public Upload()
        {
            parameters["userId"] = "工號";
            parameters["fileName"] = "上傳檔案(file)";//描述裏有(file)是為了apiWorkList測試用，用以判斷input以type=file方式顯示          
            description = "上傳檔案";
        }

        public override void doWork()
        {
            //Console.WriteLine("----------------------------------------------------------------------");
            //Console.WriteLine("userId = " + GetParameterValueInString("userId"));
            string result = "fail";
            object files = GetParameterValue("fileName");
            var file = files as FileInfo;
            DataSet result1 = null;
            if (file != null)
            {
                result = "";
                var extension = Path.GetExtension(file.FullName).ToLower(); //返回指定的路径字符串的扩展名
                System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                var sw = new Stopwatch();
                sw.Start();
                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(file.OpenRead());
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(file.OpenRead());
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(file.OpenRead());
                }
                if (reader == null)
                {
                    returnValue = "fail";
                    return;
                }
                var openTiming = sw.ElapsedMilliseconds; //获取当前实例测量得出的总运行时间
                                                         // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                using (reader)
                {
                    result1 = reader.AsDataSet();
                    /* for (int i = 0; i < result1.Tables.Count; i++)
                    {
                        //得到其中一张表的数据
                        DataTable table = result1.Tables[i];
                        //得到其中一行的数据
                        //DataRow row = table.Rows[0];
                        //得到行中某一列的信息
                        //Debug.Log(row[1].ToString());
                        DataRow row;
                        row = table.Rows[0];
                        string Sql_Col = $"insert into HMASHIS_ERRC(";
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                            if (row[k].ToString() != "")
                            {
                                Sql_Col += row[k].ToString() + "'";
                            }
                            // Console.WriteLine(row[k].ToString());
                        }
                        Sql_Col = Sql_Col.Replace("'", ",");
                        Sql_Col += ") values(";
                        Sql_Col = Sql_Col.Replace(",)", ")");
                        for (int j = 1; j < table.Rows.Count; j++)
                        {
                            row = table.Rows[j];
                            string Sql_VLU = Sql_Col;
                            //string ins_Sql = $"insert into {DBName}() values('{EMP_ID}','{EMP_NAME}','{ROUTE}','{ID}','{REP_NAME}','{SYSMODULE}', 'Y')";
                            //Console.WriteLine("------------------------------");
                            for (int k = 0; k < table.Columns.Count; k++)
                            {
                                if (row[k].ToString() != "")
                                {
                                    Sql_VLU += $"'{row[k].ToString()}'";
                                }
                                // Console.WriteLine(row[k].ToString());
                            }
                            Sql_VLU = Sql_VLU.Replace("''", "','");
                            Sql_VLU += ")";
                            if (sqlExtension.SqlTxn.ExecuteSQL("THSAPDA1", Sql_VLU) == 1)
                            {
                                result += "工作表" + i + "的第" + j + "行" + "success\n";
                            }
                            else
                            {
                                result += "工作表" + i + "的第" + j + "行" + "fail\n";
                            }
                        }
                    } */
                    // dataGridView1.DataSource = ds;
                    // dataGridView1.DataMember

                    //得到ExcelFile文件的表Sheet
                    //var sheet = ds.Tables[Sheet];

                    //在指定路径中创建或覆盖文件
                    //using (FileStream fs = System.IO.File.Create(filename))
                    //{
                    //    // 复制文件
                    //    file.CopyTo(fs);
                    //    // 清空缓冲区数据
                    //    fs.Flush();
                    //}

                }
            }
            //if (file is FileInfo)//單一檔案
            //    Console.WriteLine("uploadFile = " + (file as FileInfo).FullName + ", orgName = " + getOrgName(file as FileInfo));
            //else if (file is object[])//多個檔案
            //{
            //    foreach (object o in (file as object[]))
            //    {
            //        FileInfo fi = o as FileInfo;
            //        if (fi != null)
            //            Console.WriteLine("uploadFile = " + fi.FullName + ", orgName = " + getOrgName(fi));
            //    }
            //}
            //Console.WriteLine("----------------------------------------------------------------------");
            file.Delete();
            returnValue = result;
        }
        string getOrgName(FileInfo fi)
        {
            return fi.Name.Split("._")[1];//隨機檔名用._分隔，後面為上傳檔名
        }
    }
}
