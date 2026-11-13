using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Common
{
    public class Product : InnSqlAgent.IApiBase
    {
        public Product()
        {
            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "G_FAC";
            parameters["BigProduct"] = "用户选择大机种";
            parameters["Project_Name"] = "用户选择型號";
            description = "尺寸选择Product 共用接口";
        }
        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Product(fab);
                    return;
                default:
                    returnValue = "请填写厂别！";
                    return;
            }

        }

        public void Get_Product(string fab)
        {
            #region 变量获取
            //string fab = GetParameterValueInString("FactoryType");
            string lstr_G_FAC = GetParameterValueInString("G_FAC");
            string lstr_BigProduct = GetParameterValueInString("BigProduct");
            string lstr_Project_Name = GetParameterValueInString("Project_Name");

            string sql_BigProduct = string.Empty;
            string sql_Project_Name = string.Empty;
            string connectDB = string.Empty;
            string sql = string.Empty;
            #endregion

            #region DB连接
            if (fab.Trim() == "LCM1")
                connectDB = "MESHIS";
            #endregion

            #region 不同条件获取 SQL
            //if (lstr_BigProduct != "ALL" && lstr_BigProduct.Trim() != "")
            //    sql_BigProduct = "  AND SUBSTRING(PRODID,1,8) IN ( '" + lstr_BigProduct + "' )  ";
            var products = lstr_BigProduct.TrimEnd(',').Split(',');
            var likeConditions = new List<string>();

            foreach (var prod in products.Where(p => !string.IsNullOrWhiteSpace(p)))
            {
                // 先去掉数据本身可能带的前后单引号和空格，再转义
                var clean = prod.Trim().Trim('\'', ' ', '\t');
                var escaped = clean.Replace("'", "''");
                likeConditions.Add($"PRODID like '{escaped}%'");
            }

            sql_BigProduct = likeConditions.Count > 0
                ? $" and ({string.Join(" or ", likeConditions)}) "
                : "";

            if (lstr_Project_Name != "ALL" && lstr_Project_Name.Trim() != "")
                sql_Project_Name = "  AND PROJECT_NAME IN ( '" + lstr_Project_Name + "' )  ";
            #endregion

            #region sql
            sql = $@"Select Distinct PRODID SOPTION
                        From MPIM Where FABID='B'
                        {sql_BigProduct}{sql_Project_Name}
                         Order by PRODID";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            object ob = new { Data = ds, sql = sql };
            returnValue = ob;
        }

    }
}
