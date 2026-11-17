using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Click
{
    public class SHAP_Number_Of_Hits_Api : InnSqlAgent.IApiBase
    {
        public SHAP_Number_Of_Hits_Api()
        {
            parameters["SysName"] = "SysName";
            parameters["SysModule"] = "SysModule";
            parameters["SysFunc"] = "SysFunc";
            parameters["EmpName"] = "EmpName";
            parameters["EmpNo"] = "EmpNo";
            parameters["requestip"] = "requestip";
            parameters["HostName"] = "HostName";
            parameters["REPORT_ID"] = "報表ID";
            parameters["MODULE"] = "模塊名稱";
            description = "点击率";
     
        }
        public override void doWork()
        {
            string SysName = GetParameterValueInString("SysName");
            string SysModule = GetParameterValueInString("SysModule");
            string SysFunc = GetParameterValueInString("SysFunc");
            string EmpName = GetParameterValueInString("EmpName");
            string EmpNo = GetParameterValueInString("EmpNo");
            string requestip = GetParameterValueInString("requestip");
            string HostName = GetParameterValueInString("HostName");
            string REPORT_ID = GetParameterValueInString("REPORT_ID");
            string MODULE = GetParameterValueInString("MODULE");
            string LoginLogSql = $" insert into REPORT_NUMBER_OF_HIT(sysname,sysmodule,sysfun,empname, empno,ip,pcname, WRITE_DATE,REPORT_ID,MODULE)values('{SysName}','{SysModule}','{SysFunc}','{EmpName}','{EmpNo}','{requestip}','{HostName}', sysdate,'{REPORT_ID}','{MODULE}')";//.format(inputInfo['SysName'], inputInfo['SysModule'], inputInfo['SysFunc'], inputInfo['EmpName'], inputInfo['EmpNo'], requestip, inputInfo['HostName'])
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", LoginLogSql) == 1)
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
