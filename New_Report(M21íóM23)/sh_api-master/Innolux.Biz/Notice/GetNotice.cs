using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Notice
{
    public class GetNotice : InnSqlAgent.IApiBase
    {
        public GetNotice()
        {
            parameters["NoticeTitle"] = "標題";
            parameters["SYSID"] = "SYSID";
            parameters["NoticeNum"] = "NoticeNum";
            description = "獲取公告文件";
        }
        public override void doWork()
        {
            string NoticeTitle = GetParameterValueInString("NoticeTitle");
            string SYSID = GetParameterValueInString("SYSID");
            string NoticeNum = GetParameterValueInString("NoticeNum");
            string sql = $" Select NOTICETYPE,NOTICEPEOPLE,NOTICETITLE,NOTICETIME,NOTICENUM,SYSID,NOTICEDATA,ISDELETE from REPORT_SYS_NOTICE where ISDELETE <> 'N' and SYSID = '{SYSID}' order by NOTICENUM desc";
            //System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql);
            System.Data.DataSet ds1 = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql);
            returnValue = ds1;
        }
    }
}
