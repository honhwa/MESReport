using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Diagnostics;
using System.Text;
using InnSqlAgent;

namespace Innolux.Biz.FileOperation
{
    public class ExcelUploadBiz
    {
        public string UploadFilesBiz(List<IFormFile> files, string DBName)
        {
            string result = "fail";
            foreach (var file in files)
            {
                result = "";
                //MSG_Result mSG_Result = new MSG_Result();
                ////打开请求流以获取上传的文件
                //StreamReader reader = new StreamReader(file.OpenReadStream());
                ////从流的当前位置到末尾读取所有字符，如果当前位置位于流的末尾，则返回空字符串“”
                //String content = reader.ReadToEnd();
                ////获取文件的名称
                //String name = file.FileName;
                //保存文件的路径
                //string str1 = "";
                //PlatformID _PlatformID = Environment.OSVersion.Platform;
                //if (_PlatformID == PlatformID.Win32NT)
                //{
                //    str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                //    str1 = str1.Replace("Innolux.Api.exe", "");
                //}
                //str1 = System.AppDomain.CurrentDomain.BaseDirectory;
                //str1 = str1.Replace("Innolux.Api.exe", "");
                //String filename = str1 + "uploadtemp/" + parentId + name;

                ////if (System.IO.File.Exists(filename))
                ////{
                //// System.IO.File.Delete(filename);
                ////}
                //mSG_Result.nodeId = "not use";
                //mSG_Result.id = "not use";
                //mSG_Result.name = name;
                //mSG_Result.type = "F";
                //mSG_Result.parentId = parentId + name;
                //mSG_Result.bytes = file.Length;
                //result.Add(mSG_Result);
                var extension = Path.GetExtension(file.FileName).ToLower(); //返回指定的路径字符串的扩展名
                System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(ppp);
                var sw = new Stopwatch();
                sw.Start();
                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(file.OpenReadStream());
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(file.OpenReadStream());
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(file.OpenReadStream());
                }

                if (reader == null)
                    return "fail";

                var openTiming = sw.ElapsedMilliseconds; //获取当前实例测量得出的总运行时间
                                                         // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                using (reader)
                {
                    DataSet result1 = reader.AsDataSet();
                    for (int i = 0; i < result1.Tables.Count; i++)
                    {
                        //得到其中一张表的数据
                        DataTable table = result1.Tables[i];
                        //得到其中一行的数据
                        //DataRow row = table.Rows[0];
                        //得到行中某一列的信息
                        //Debug.Log(row[1].ToString());
                        DataRow row;
                        row = table.Rows[0];
                        string ins_Sql = $"insert into {DBName}(";
                        for (int k = 0; k < table.Columns.Count; k++)
                        {
                            if (row[k].ToString() != "")
                            {
                                ins_Sql += row[k].ToString() + "'";
                            }
                            // Console.WriteLine(row[k].ToString());
                        }
                        ins_Sql = ins_Sql.Replace("'", ",");
                        ins_Sql += ") values(";
                        ins_Sql = ins_Sql.Replace("')", ")");
                        for (int j = 1; j < table.Rows.Count; j++)
                        {
                            row = table.Rows[j];
                            //string ins_Sql = $"insert into {DBName}() values('{EMP_ID}','{EMP_NAME}','{ROUTE}','{ID}','{REP_NAME}','{SYSMODULE}', 'Y')";
                            //Console.WriteLine("------------------------------");
                            for (int k = 0; k < table.Columns.Count; k++)
                            {
                                if (row[k].ToString() != "")
                                {
                                    ins_Sql += $"'{row[k].ToString()}'";
                                }
                                // Console.WriteLine(row[k].ToString());
                            }
                            ins_Sql = ins_Sql.Replace("''", "','");
                            ins_Sql += ")";
                            if (sqlExtension.SqlTxn.ExecuteSQL(DBName, ins_Sql) == 1)
                            {
                                result += "工作表" + i + "的第" + j + "行" + "success\n";
                            }
                            else
                            {
                                result += "工作表" + i + "的第" + j + "行" + "fail\n";
                            }
                        }
                    }
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
            return result;
        }
    }
}
