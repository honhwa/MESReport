using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using ExcelDataReader;
using System.Reflection;

namespace Innolux.Biz.Common
{
    public class Utils
    {
        /// <summary>
        /// 計算兩個日期之間的日期
        /// </summary>
        /// <param name="starttime">開始時間</param>
        /// <param name="endtime">結束時間</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static List<string> getdiffdate(string starttime, string endtime, string format = "yyyy-MM-dd")
        {

            //初始化日期列表，数组
            // Console.WriteLine("输出开始日期到结束日期区间的所有日期");
            var DateSum = new List<string>();
            for (var date = Convert.ToDateTime(starttime); date <= Convert.ToDateTime(endtime); date = date.AddDays(1))
            {
                // Console.WriteLine(date.ToString("yyyy-MM-dd"));
                DateSum.Add(date.ToString(format));
            }
            return DateSum;
        }
        /// <summary>
        /// 文件上傳
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        public static DataSet Uploadresult(FileInfo file)
        {
            DataSet result1 = null;
            if (file != null)
            {
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
                    return null;
                }
                var openTiming = sw.ElapsedMilliseconds; //获取当前实例测量得出的总运行时间
                // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                using (reader)
                {
                    result1 = reader.AsDataSet();
                }
                file.Delete();
                return result1;
            }
            return result1;
        }
        /// <summary> 
        /// 数据表转键值对集合
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 ，键/值相同
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        public static List<object> DataTableToList(DataTable dt)
        {
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }


    }
}
