using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.RolePermissions
{
    public class CreateRolePermissions : InnSqlAgent.IApiBase
    {
        public CreateRolePermissions()
        {
            parameters["ROLE"] = "角色";
            parameters["ROLELEVEL"] = "角色等级";
            parameters["BACKUP"] = "备注";
            parameters["PERSON"] = "最后更新者";
            description = "角色管理（创建角色）";
        }
        public override void doWork()
        {
            string ROLE = GetParameterValueInString("ROLE");
            string ROLELEVEL = GetParameterValueInString("ROLELEVEL");
            string BACKUP = GetParameterValueInString("BACKUP");
            string PERSON = GetParameterValueInString("PERSON");
            if (ROLE == "")
            {
                returnValue = "请输入角色和角色等级！";
            }
            else
            {
                string sql1 = "";
                string sql = $"select * from REPORT_PERMISSION_MAPPING where ROLE = '{ROLE}' and ISDELETE <> 'N' ";
                System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count < 1)
                {
                    sql1 = $" insert into REPORT_PERMISSION_MAPPING(ROLE, ROLELEVEL, BACKUP, PERSON, CREATE_TIME, UPDATE_TIME, ISDELETE) values('{ROLE}','{ROLELEVEL}','{BACKUP}','{PERSON}',sysdate,sysdate,'Y')";
                    //$@" insert into REPORT_SYS_MANAGE_PERMISSION(TYPE,NAME,PARENTID,ROUTER,ICON,ISSHOW,ORDERNUM,ID,SYSID,CREATETIME,UPDATETIME,PERMS)values('{type}','{name}','{parentId                
                }
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
