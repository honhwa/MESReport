using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Role
{
    public class GetRole:InnSqlAgent.IApiBase
    {
        public GetRole()
        {
            parameters["EMP_ID"] = "EMP_ID";
            parameters["EMP_NAME"] = "用户名称";
            parameters["BACKUP"] = "BACKUP";
            parameters["EMP_PERMS"] = "EMP_PERMS";
            description = "账号角色管理（获取账号角色信息）";

        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            string BACKUP = GetParameterValueInString("BACKUP");
            string EMP_PERMS = GetParameterValueInString("EMP_PERMS");
            string strSql = "";
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
    
            list.Add(EMP_ID);
            list1.Add("EMP_ID");
            list.Add(EMP_NAME);
            list1.Add("EMP_NAME");
            list.Add(BACKUP);
            list1.Add("BACKUP");
            list.Add(EMP_PERMS);
            list1.Add("EMP_PERMS");
            bool temp = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != "" && temp == false)
                {
                    strSql = strSql + " and " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' ";
                }
                if (list[i] != "" && temp)
                {
                    strSql = " where " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' ";
                    temp = false;
                }
            }
            string sql = $"select * from REPORT_LOGIN_LOG " + strSql;
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            returnValue = ds;

        }

    }
}
