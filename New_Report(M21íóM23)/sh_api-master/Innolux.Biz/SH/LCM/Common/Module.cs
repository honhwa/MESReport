using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.LayoutRenderers.Wrappers;
using StackExchange.Redis;

namespace Innolux.Biz.SH.LCM.Common
{
    public class Module : InnSqlAgent.IApiBase
    {
        public Module()
        {
            parameters["FactoryType"] = "厂别 LCM1/LCM2/LCM3/LCM4";
            parameters["G_FAC"] = "G_FAC: C / B / E / D ";
     
            description = "左侧 Module 共用接口";
        }
        public override void doWork()
        {

            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "LCM1":
                case "LCM2":
                case "LCM3":
                case "LCM4":
                    Get_Module(fab);
                    return;
                default:
                    returnValue = "请填写厂别！";
                    return;
            }

        }

        public void Get_Module(string fab)
        {
            #region 变量获取
            //string fab = GetParameterValueInString("FactoryType");
            string lstr_G_FAC = GetParameterValueInString("G_FAC");
            string connectDB = string.Empty;
            #endregion

            #region DB连接
            if (fab.Trim() == "LCM1")
            {
                connectDB = "MESHIS";
            }
            else if (fab.Trim() == "LCM3")
            {
                connectDB = "MESDA1";
            }
            else if (fab.Trim() == "LCM2")
            {
                connectDB = "MBSDA1";
            }
            else if (fab.Trim() == "LCM4")
            {
                connectDB = "MDSDA1";
            }
            #endregion

            #region SQL
            string sql = $@"
                    select  distinct prod_type
                    from m{lstr_G_FAC}sda1.m{lstr_G_FAC}shis_ipms@pn{lstr_G_FAC}mr
                    where prod_type is not null 
                    AND prod_type <> ' '
                    order by prod_type
                ";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql);
            object ob = new { Data = ds, sql = sql };
            returnValue = ob;
        }

    }
}
