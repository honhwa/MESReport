using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Jurisdiction
{
    public class VerifyName : InnSqlAgent.IApiBase
    {
        public VerifyName()
        {
            parameters["UserID"] = "UserID";
            parameters["Password"] = "Password";
            description = "登录认证";
        }
        public override void doWork()
        {
            string UserID = GetParameterValueInString("UserID");
            string Password = GetParameterValueInString("Password");
            string sql = $@"select * from REPORT_LOGIN_LOG where EMP_ID='{UserID}' and PASSWORD = '{Password}' and BACKUP ='Y' ";//.format(inputInfo['UserID'], inputInfo['Password'])
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            Random random = new Random();
            int token = random.Next(100000);
            List<object> list = new List<object>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    object ob = new
                    {
                        MemID = ds.Tables[i].Rows[j][0],
                        MemName = ds.Tables[i].Rows[j][1],
                    };
                    list.Add(ob);
                }
            }
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count < 2)
            {
                object ob = new
                {
                    token,
                    Success = "True",
                    Data = list,
                };
                returnValue = ob;
            }
            else
            {
                object ob = new
                {
                    token = "",
                    Success = "false",
                    Data = "",
                };
                returnValue = ob;
            }
        }

    }
}
