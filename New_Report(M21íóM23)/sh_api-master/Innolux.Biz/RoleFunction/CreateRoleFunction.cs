using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RoleFunction
{
    public class CreateRoleFunction : InnSqlAgent.IApiBase
    {
        public CreateRoleFunction()
        {
            parameters["ROLE_ID"] = "角色ID";
            parameters["ROLE_NAME"] = "角色名称";
            parameters["NAME"] = "名称";
            parameters["ROUTER_ID"] = "路由ID";
            parameters["ROUTER"] = "节点路由";
            parameters["BACKUP"] = "备注";
            parameters["PERSON"] = "最后更新者";
            description = "角色权限管理（添加角色权限）";
        }

        public override void doWork()
        {
            string[] ROLE_ID = GetParameterValueInString("ROLE_ID").Split("','");
            string[] ROLE_NAME = GetParameterValueInString("ROLE_NAME").Split("','");
            string[] NAME = GetParameterValueInString("NAME").Split("','");
            string[] ROUTER_ID = GetParameterValueInString("ROUTER_ID").Split("','");
            string[] ROUTER = GetParameterValueInString("ROUTER").Split("','");
            string[] BACKUP = GetParameterValueInString("BACKUP").Split("','");
            string[] PERSON = GetParameterValueInString("PERSON").Split("','");
            string sql1 = "";
            string sql = $"select * ";
            for (int i = 0; i < ROLE_ID.Length; i++)
            {
                sql1 = $" insert into REPORT_ROLEFUNCTUION(ROLE_ID,ROLE_NAME,NAME,ROUTER_ID,ROUTER,CREATE_TIME,UPDATE_TIME,BACKUP,PERSON,ISDELETE) values('{ROLE_ID}','{ROLE_NAME}','{NAME}','{ROUTER_ID}','{ROUTER}', sysdate, sysdate,'{BACKUP}','{PERSON}','Y')";
                //$@" insert into REPORT_SYS_MANAGE_PERMISSION(TYPE,NAME,PARENTID,ROUTER,ICON,ISSHOW,ORDERNUM,ID,SYSID,CREATETIME,UPDATETIME,PERMS)values('{type}','{name}','{parentId                
                if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql1) == 1)
                {
                    returnValue = "Success!";
                }
                else
                {
                    returnValue = "False!";
                }
            }
        }

    }
}
