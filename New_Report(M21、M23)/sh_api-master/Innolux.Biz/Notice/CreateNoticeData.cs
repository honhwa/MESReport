using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Notice
{
    public class CreateNoticeData : InnSqlAgent.IApiBase
    {
        public CreateNoticeData()
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


            string sql = "";
            string sql1;
            string sql2;
            sql1 = $@" select * from  REPORT_SYS_NOTICE where ISDELETE <> 'N' and NoticeTitle = '{NoticeTitle}' and SYSID='{SYSID}'";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'])
            sql2 = $@" select NVL(max(NoticeNum+1),1) NUM from  REPORT_SYS_NOTICE";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql1);
            System.Data.DataSet ds2 = sqlExtension.SqlTxn.GetDataSet("TESTDB", sql2);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {

                sql = @$" insert into REPORT_SYS_NOTICE(NoticeType,SYSID,NoticeNum,NoticePeople,NoticeTime,NoticeTitle,ISDELETE,NoticeData)values('{NoticeType}', '{SYSID}','{ds2.Tables[0].Rows[0][0]}', '{NoticePeople}',sysdate,'{NoticeTitle}','Y','{NoticeData}')";

            }
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
