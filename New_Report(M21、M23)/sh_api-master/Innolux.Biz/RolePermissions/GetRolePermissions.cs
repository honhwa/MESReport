using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RolePermissions
{
    public class GetRolePermissions : InnSqlAgent.IApiBase
    {
        public GetRolePermissions()
        {
            parameters["ROLE"] = "角色";
            parameters["ROLELEVEL"] = "角色等级";
            parameters["BACKUP"] = "备注";
            description = "角色管理（添加角色）";
        }
        public override void doWork()
        {
            string ROLE = GetParameterValueInString("ROLE");
            string ROLELEVEL = GetParameterValueInString("ROLELEVEL");
            string BACKUP = GetParameterValueInString("BACKUP");
            string strSql = "";
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            //if (EMP_ID == "" && EMP_NAME == "" && BACKUP == "")
            //{
            //    string sql = "select * from REPORT_LOGIN_LOG ";
            //    System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            //    returnValue = ds;
            //}
            list.Add(ROLE);
            list1.Add("ROLE");
            list.Add(ROLELEVEL);
            list1.Add("ROLELEVEL");
            list.Add(BACKUP);
            list1.Add("BACKUP");
            bool temp = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != "" && temp == false)
                {
                    strSql = strSql + " and " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' ";
                }
                if (list[i] != "" && temp)
                {
                    strSql = " and " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' " + "  ";
                    temp = false;
                }
            }
            string sql = $"select ROLE, ROLELEVEL, BACKUP from REPORT_PERMISSION_MAPPING where ISDELETE <> 'N'" + strSql;
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            returnValue = ds;
        }

    }
}
