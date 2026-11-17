using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RoleFunction
{
    public class GetRoleFunction : InnSqlAgent.IApiBase
    {
        public GetRoleFunction()
        {
            parameters["ROLE_ID"] = "角色ID";
            parameters["ROLE_NAME"] = "角色名称";
            parameters["NAME"] = "名称";
            parameters["ROUTER_ID"] = "路由ID";
            parameters["ROUTER"] = "节点路由";
            description = "角色权限管理（获取角色权限）";
        }

        public override void doWork()
        {
            string ROLE_ID = GetParameterValueInString("ROLE_ID");
            string ROLE_NAME = GetParameterValueInString("ROLE_NAME");
            string NAME = GetParameterValueInString("NAME");
            string ROUTER_ID = GetParameterValueInString("ROUTER_ID");
            string ROUTER = GetParameterValueInString("ROUTER");
            string strSql = "";
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            //if (EMP_ID == "" && EMP_NAME == "" && BACKUP == "")
            //{
            //    string sql = "select * from REPORT_LOGIN_LOG ";
            //    System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            //    returnValue = ds;
            //}
            list.Add(ROLE_ID);
            list1.Add("ROLE_ID");
            list.Add(ROLE_NAME);
            list1.Add("ROLE_NAME");
            list.Add(NAME);
            list1.Add("NAME");
            list.Add(ROUTER_ID);
            list1.Add("ROUTER_ID");
            list.Add(ROUTER);
            list1.Add("ROUTER");
            bool temp = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != "" && temp == false)
                {
                    strSql = strSql + " and " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' ";
                }
                if (list[i] != "" && temp)
                {
                    strSql = " and " + list1[i].ToString().Trim() + " like '" + list[i].ToString().Trim() + "%' " + " ";
                    temp = false;
                }
            }
            string sql = $"select ROLE_ID, ROLE_NAME, NAME, ROUTER_ID, ROUTER, CREATE_TIME, UPDATE_TIME, BACKUP from REPORT_ROLEFUNCTUION  where ISDELETE <> 'N'  " + strSql;
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            returnValue = ds;
        }

    }
}
