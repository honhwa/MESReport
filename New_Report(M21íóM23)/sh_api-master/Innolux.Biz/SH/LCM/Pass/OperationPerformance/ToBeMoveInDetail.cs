using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.SH.LCM.Pass.OperationPerformance
{
    public class ToBeMoveInDetail:InnSqlAgent.IApiBase
    {
        public ToBeMoveInDetail()
        {
            parameters["row1"] = "记录用户选择的站点";
            parameters["Ver"] = "记录用户选择的机种版本";
            parameters["prod_nbr"] = "记录用户选择的站点";

            parameters["FactoryType"] = "厂别 SH/LCM1";
            parameters["G_FAC"] = "C/E";

            parameters["FromDate"] = "开始时间(B)";
            parameters["ToDate"] = "结束时间(B)";

            parameters["lstr_Shift"] = "";
            parameters["lstr_ShopCD"] = "";
            parameters["lstr_Floor"] = "";
            parameters["lstr_WorkOrder"] = "";
            parameters["lstr_Size"] = "";
            parameters["lstr_BigProdProduct"] = "";
            parameters["lstr_Product"] = "";
            parameters["lstr_Wotype"] = "";

            description = "对应OperPerfDetail 2文件";
        }

        public override void doWork()
        {
            string fab = GetParameterValueInString("FactoryType");
            switch (fab)
            {
                case "SH":
                case "LCM1":
                    Get_Data_SH(fab);
                    break;
                default:
                    returnValue = "请填写厂别(FactoryType)！";
                    return;
            }
        }

        public void Get_Data_SH(string fab)
        {
            #region 获取前一阶页面传递条件
            //前一阶用户选择条件
            string row1 = GetParameterValueInString("row1");
            string S_Ver = GetParameterValueInString("Ver");
            string S_Prod_Nbr = GetParameterValueInString("Prod_Nbr");


            //dataset传递条件
            string lstr_G_FAC = GetParameterValueInString("G_FAC");
            string lstr_FromDate = GetParameterValueInString("FromDate");
            string lstr_ToDate = GetParameterValueInString("ToDate");

            string sql_Shift = GetParameterValueInString("sql_Shift");
            string sql_IncludeBL = GetParameterValueInString("sql_IncludeBL");
            string sql_Floor = GetParameterValueInString("sql_Floor");
            string sql_WorkOrder = GetParameterValueInString("sql_WorkOrder");
            string sql_Product = GetParameterValueInString("sql_Product");
            string sql_LcmOwner = GetParameterValueInString("sql_LcmOwner");
            string sql_LcdOwner = GetParameterValueInString("sql_LcdOwner");


            string connectDB = string.Empty;
            string table_name1 = string.Empty;
            string sql01 = string.Empty;
            string sql_Ver = string.Empty;
            #endregion


            #region TABLE + DB连接
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

            #region 条件
            if (S_Prod_Nbr != "合計" )
            {
                sql_Product = "  and  prod_nbr =  '" + S_Prod_Nbr + "'  ";
                sql_Ver = "  and ver  =  '" + S_Ver + "'  ";
            }
            #endregion

            #region SQL
            sql01 = $@"   select prod_nbr, last_trans_dt,pnl_id,cart_id,last_eqp_nbr,work_ctr,STIMES_TTL,'1' reserved_21,HOLD_CODE,hold_flag,1 as qty
            from  M{lstr_G_FAC}RDA1.M{lstr_G_FAC}RWIP_STAY1         
            where fac_id = '{lstr_G_FAC}'  and  mvin_flag = 'Y' 
             and acct_date= '{lstr_ToDate}'
            {sql_Product} {sql_Ver} {sql_Floor} {sql_LcdOwner} {sql_LcmOwner} {sql_WorkOrder}
             and work_ctr= '{row1}'
             order by prod_nbr
            ";
            #endregion

            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet(connectDB, sql01);
            #region 去除SQL语句中的特殊符号
            sql01 = sql01.Replace("\r\n", " ");
            sql01 = sql01.Replace("\n", " ");
            sql01 = sql01.Replace("\t", " ");
            sql01 = sql01.Replace("\t\t", " ");
            sql01 = sql01.Replace("\t\t\t", " ");
            sql01 = sql01.Replace("\t\t\t\t", " ");
            #endregion

            #region datatable格式准备
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (string.Compare(ds.Tables[0].Rows[i]["RESERVED_21"].ToString(), "0") == 1)
                {
                    ds.Tables[0].Rows[i]["RESERVED_21"] = "模組";
                }
                else
                {
                    ds.Tables[0].Rows[i]["RESERVED_21"] = "單玻";
                }
            }
            #endregion

            #region 回传前端data
            object ob = new
            {
                Data = ds,
                SQL01 = sql01,
                FactoryType = fab,
                G_FAC = lstr_G_FAC,
            };
            returnValue = ob;
            #endregion

        }

    }
}
