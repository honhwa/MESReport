using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Role
{
    public class CreateRole:InnSqlAgent.IApiBase
    {
        public CreateRole()
        {
            parameters["EMP_ID"] = "EMP_ID";
            parameters["EMP_NAME"] = "EMP_NAME";
            parameters["PASSWORD"] = "PASSWORD";
            parameters["COUNT"] = "COUNT";
            parameters["EMP_AD"] = "EMP_AD";
            parameters["EMP_PERMS"] = "EMP_PERMS";
            parameters["BACKUP"] = "BACKUP";
            parameters["DEPT_NAME"] = "DEPT_NAME";
            parameters["REMARK"] = "REMARK";
            parameters["UPDATE_BY"] = "UPDATE_BY";
            description = "账号角色管理（添加账号及其权限）";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            string PASSWORD = GetParameterValueInString("PASSWORD");
            string COUNT = GetParameterValueInString("COUNT");
            string EMP_AD = GetParameterValueInString("EMP_AD");
            string EMP_PERMS = GetParameterValueInString("EMP_PERMS");
            string BACKUP = GetParameterValueInString("BACKUP");
            string DEPT_NAME = GetParameterValueInString("DEPT_NAME");
            string REMARK = GetParameterValueInString("REMARK");
            string UPDATE_BY = GetParameterValueInString("UPDATE_BY");
            string sql1 = "";
            string sql = $"select * from REPORT_LOGIN_LOG where EMP_ID = '{EMP_ID}' ";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count < 1)
            {
                sql1 = $" insert into REPORT_LOGIN_LOG(EMP_ID,EMP_NAME,PASSWORD,COUNT,EMP_AD,EMP_PERMS,BACKUP,DEPT_NAME,REMARK,UPDATE_BY) values('{EMP_ID}','{EMP_NAME}','{PASSWORD}','{COUNT}','{EMP_AD}','{EMP_PERMS}','{BACKUP}','{DEPT_NAME}','{REMARK}','{UPDATE_BY}')";
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
