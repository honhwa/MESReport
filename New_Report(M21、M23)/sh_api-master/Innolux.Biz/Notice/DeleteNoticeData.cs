using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Notice
{
    public class DeleteNoticeData : InnSqlAgent.IApiBase
    {
        public DeleteNoticeData()
        {
            parameters["NOTICETITLE"] = "標題";
            parameters["SYSID"] = "SYSID";
            parameters["NOTICENUM"] = "NoticeNum";
            description = "刪除公告文件";
        }
        public override void doWork()
        {
            string NoticeTitle = GetParameterValueInString("NOTICETITLE");
            string SYSID = GetParameterValueInString("SYSID");
            string NoticeNum = GetParameterValueInString("NOTICENUM");
            string sql = $" update REPORT_SYS_NOTICE  set ISDELETE = 'N' where NoticeTitle = '{NoticeTitle}' and SYSID='{SYSID}' and NoticeNum = '{NoticeNum}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
            //System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql);
            if (sqlExtension.SqlTxn.ExecuteSQL("TESTDB", sql) == 1)
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
