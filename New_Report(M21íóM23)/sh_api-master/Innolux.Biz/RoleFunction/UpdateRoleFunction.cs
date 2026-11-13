using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RoleFunction
{
    public class UpdateRoleFunction : InnSqlAgent.IApiBase
    {
        public UpdateRoleFunction()
        {
            parameters["ROLE_ID"] = "角色ID";
            parameters["ROLE_NAME"] = "角色名称";
            parameters["NAME"] = "名称";
            parameters["ROUTER_ID"] = "路由ID";
            parameters["ROUTER"] = "节点路由";
            parameters["BACKUP"] = "备注";
            parameters["PERSON"] = "最后更新者";
            description = "角色权限管理（修改角色权限）";
        }
        public override void doWork()
        {
            string ROLE_ID = GetParameterValueInString("ROLE_ID");
            string ROLE_NAME = GetParameterValueInString("ROLE_NAME");
            string[] NAME = GetParameterValueInString("NAME").Split("','");
            string[] ROUTER_ID = GetParameterValueInString("ROUTER_ID").Split("','");
            string[] ROUTER = GetParameterValueInString("ROUTER").Split("','");
            string[] BACKUP = GetParameterValueInString("BACKUP").Split("','");
            string[] PERSON = GetParameterValueInString("PERSON").Split("','");
            List<int> list = new List<int>();
            List<string> list1 = new List<string>();
            bool temp = true;
            string sql_Close = $" update REPORT_ROLEFUNCTUION  set ISDELETE = 'N', UPDATE_TIME = sysdate  where ROLE_ID = '{ROLE_ID}' and ROLE_NAME='{ROLE_NAME}' and ISDELETE = 'Y' ";
            string sql_Open = $" update REPORT_ROLEFUNCTUION  set ISDELETE = 'Y', UPDATE_TIME = sysdate  where ROLE_ID = '{ROLE_ID}' and ROLE_NAME='{ROLE_NAME}' and ISDELETE = 'N' ";
            for (int i = 0; i < ROUTER_ID.Length; i++)
            {
                string sql_Show = $"select * from REPORT_ROLEFUNCTUION where ROLE_ID = '{ROLE_ID}' ";
                System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql_Show);
                bool signo = true;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (ROUTER_ID[i] == ds.Tables[0].Rows[j][3].ToString())
                    {
                        signo = false;
                        if (temp)
                        {
                            sql_Open += $" and ROUTER_ID in ('{ROUTER_ID[i]}'";
                            temp = false;
                        }
                        else
                        {
                            sql_Open += $",'{ROUTER_ID[i]}'";
                        }
                    }
                }
                if (signo)
                {
                    list.Add(i);
                }
                sql_Close += $" and ROUTER_ID <> '{ROUTER_ID[i]}' ";
            }
            if (sql_Open.IndexOf("in ('") > -1)
            {
                sql_Open += ")";
            }
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql_Open) >= 1)
            {
                list1.Add("Open Success!");
            }
            else
            {
                list1.Add("Open False!");
            }
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql_Close) >= 1)
            {
                list1.Add("Close Success!");
            }
            else
            {
                list1.Add("Close False!");
            }
            foreach (int i in list)
            {
                string sql_Insert = $" insert into REPORT_ROLEFUNCTUION(ROLE_ID,ROLE_NAME,NAME,ROUTER_ID,ROUTER,CREATE_TIME,UPDATE_TIME,BACKUP,PERSON,ISDELETE) values('{ROLE_ID}','{ROLE_NAME}','{NAME[i]}','{ROUTER_ID[i]}','{ROUTER[i]}', sysdate, sysdate,'{BACKUP[i]}','{PERSON[i]}','Y')";
                if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql_Insert) == 1)
                {
                    list1.Add("Insert Success!" + i);
                }
                else
                {
                    list1.Add("Insert False!" + i);
                }
            }
            returnValue = list1;
        }
    }
}
