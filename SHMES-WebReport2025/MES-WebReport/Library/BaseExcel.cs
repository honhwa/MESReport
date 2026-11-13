using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using Microsoft.Office.Interop.Excel;
using Excel;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Report.Library
{
    public class BaseExcel
    {
        private Application excel;
        private Workbook exlWork;
        private Worksheet exlSheet;
        //private const string strPath = System.Web.HttpContext.Current.Server.MapPath(
        //private const string strPath = 


        /// <summary>
        /// 取得Excel
        /// </summary>
        public Excel.Application ExcelApp
        {
            get
            {
                return this.excel;
            }
        }

        /// <summary>
        /// 取得Workbook
        /// </summary>
        public Excel.Workbook ExcelWork
        {
            get
            {
                return this.exlWork;
            }
        }

        /// <summary>
        /// 取得Worksheet
        /// </summary>
        public Excel.Worksheet ExcelSheet
        {
            get
            {
                return this.exlSheet;
            }
        }

        public BaseExcel(string fileName, string sheetName)
        {
            this.excel = new ApplicationClass();
            this.exlWork = (Workbook)this.excel.Workbooks.Open(System.Web.HttpContext.Current.Server.MapPath(fileName), Missing.Value, Missing.Value
            , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
            , Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            if (sheetName == "")
            {
                this.exlSheet = (Worksheet)this.exlWork.Worksheets[1];
            }
            else
            {
                this.exlSheet = (Worksheet)this.exlWork.Worksheets[sheetName];
            }
        }

        public Boolean ExcelSave(string fileName)
        {
            try
            {
                Boolean blExcelSave = false;

                this.exlWork.SaveAs(System.Web.HttpContext.Current.Server.MapPath(fileName), Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                blExcelSave = true;
                return blExcelSave;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ExcelClose()
        {
            try
            {
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(exlSheet);
                exlSheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(exlWork);
                exlWork = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteExcelFile(string strFineName)
        {
            try
            {
                int j;
                string[] strFileName;
                int iLen;
                int ibegin;
                iLen = strFineName.ToString().Trim().Length + 8;

                strFileName = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath(@"../../ExportFile"));
                for (j = 0; j < strFileName.Length; j++)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string strdelete = "";
                        strdelete = strFineName + Convert.ToString(System.DateTime.Now.AddDays(-i).ToString("yyyyMMdd"));
                        ibegin = strFileName[j].ToString().Trim().Length - 18 - strFineName.ToString().Trim().Length;
                        if (strdelete == strFileName[j].ToString().Trim().Substring(ibegin, iLen))
                        {
                            strdelete = strFileName[j].ToString().Trim();
                            //System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(@"../../ExportFile/" + strdelete.ToString()));
                            System.IO.File.Delete(strdelete);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void ExportDataToExcel(HttpResponse response, System.Data.DataTable dtTemp, IDictionary<string, string> Boundfields, string fileName)
        {
            if (Boundfields == null || Boundfields.Count == 0) return;
            if (dtTemp == null) return;

            try
            {
                TextWriter tWriter = new StringWriter();
                HtmlTextWriter htWriter = new HtmlTextWriter(tWriter);
                response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlPathEncode(fileName) + ".xls");
                response.ContentType = "application/vnd.ms-excel";
                response.ContentEncoding = System.Text.Encoding.UTF8;

                htWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\">");
                GridView gridView = new GridView();
                gridView.AllowPaging = false;
                gridView.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gridView.RowStyle.HorizontalAlign = HorizontalAlign.Center;
                gridView.HeaderStyle.Font.Bold = true;

                System.Web.UI.WebControls.BoundField field;

                if (Boundfields.Values == null)
                    gridView.AutoGenerateColumns = true;
                else
                    gridView.AutoGenerateColumns = false;
                foreach (KeyValuePair<string, string> kv in Boundfields)
                {
                    field = new BoundField();
                    field.HeaderText = kv.Key;
                    field.DataField = kv.Value;
                    gridView.Columns.Add(field);
                }
                gridView.DataSource = dtTemp;
                gridView.DataBind();
                gridView.RenderControl(htWriter);
                response.Write(tWriter.ToString());
                response.End();
            }
            catch
            {

            }
        }

        public static void ExportDataToCsv(HttpResponse response, System.Data.DataTable dtTemp, IDictionary<string, string> boundFields, string fileName)
        {
            if (dtTemp == null) return;
            if (boundFields == null || boundFields.Count == 0) return;
            try
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<string, string> kv in boundFields)
                {
                    builder.Append(kv.Key + ",");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("\r\n");

                foreach (DataRow dr in dtTemp.Rows)
                {
                    foreach (KeyValuePair<string, string> kv in boundFields)
                    {
                        builder.Append(dr[kv.Value].ToString() + ",");
                    }
                    builder.Append("\r\n");
                }
                builder.Remove(builder.Length - 2, 2);
                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv");
                response.ContentType = "application/vnd.ms-excel";
                response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//
                response.Write(builder.ToString());
                response.End();
            }
            catch
            {

            }
        }
    }
}
