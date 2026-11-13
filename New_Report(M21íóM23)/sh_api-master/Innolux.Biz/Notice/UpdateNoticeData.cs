using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Notice
{
    public class UpdateNoticeData : InnSqlAgent.IApiBase
    {
        public UpdateNoticeData()
        {
            parameters["NOTICETYPE"] = "公告類型";
            parameters["NOTICEPEOPLE"] = "發佈人員";
            parameters["NoticeTime"] = "發佈時間";
            parameters["NOTICETITLE"] = "標題";
            parameters["NOTICENUM"] = "排序";
            parameters["NOTICEDATA"] = "公告内容";
            parameters["SYSID"] = "SYSID";
        }
        public override void doWork()
        {
            string NoticeType = GetParameterValueInString("NOTICETYPE");
            string SYSID = GetParameterValueInString("SYSID");
            string NoticeNum = GetParameterValueInString("NOTICENUM");
            string NoticePeople = GetParameterValueInString("NOTICEPEOPLE");
            string NoticeData = GetParameterValueInString("NOTICEDATA");
            string NoticeTitle = GetParameterValueInString("NOTICETITLE");


            string sql = $" update REPORT_SYS_NOTICE  set NoticeData = '{NoticeData}', NoticeType='{NoticeType}' , NoticePeople = '{NoticePeople}'  where NoticeTitle = '{NoticeTitle}' and SYSID='{SYSID}' and NoticeNum = '{NoticeNum}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'])
            //System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql);
            try
            {
                sqlExtension.SqlTxn.ExecuteSQL("TESTDB", sql);
                returnValue = "Success!";
            }
            catch
            {
                returnValue = "False!";
            };

        }
    }
}
