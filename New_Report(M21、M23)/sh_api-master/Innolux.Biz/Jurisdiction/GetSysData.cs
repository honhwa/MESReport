using Innolux.Core.Extensions.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Innolux.Biz.Jurisdiction
{
    public class GetSysData : InnSqlAgent.IApiBase
    {
        public GetSysData()
        {
            parameters["EMP_ID"] = "EMP_ID";
            parameters["SYSID"] = "SYSID";
            parameters["EMP_NAME"] = "用户名称";
            description = "权限管理接口(获取菜单)";
        }
        public override void doWork()
        {
            string EMP_ID = GetParameterValueInString("EMP_ID");
            string SYSID = GetParameterValueInString("SYSID");
            string EMP_NAME = GetParameterValueInString("EMP_NAME");
            //string sql1 = $@" select EMP_PERMS from REPORT_LOGIN_LOG a  
            //left join (select  ROLE_NAME from REPORT_ROLEFUNCTUION) b
            //on a.EMP_PERMS = b.ROLE_NAME
            //where EMP_ID = '{EMP_ID}' ";//.format(inputInfo['EMP_ID'])
            //string ticket = loginInnodriver("test", "test");
            string ticket = "Yes";
            if (EMP_ID == "")
            {
                returnValue = "请输入用户！";
                return;
            }
            string sql1 = $"select EMP_PERMS from REPORT_LOGIN_LOG where EMP_ID = '{EMP_ID}'";
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql1);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count < 1)
            {
                string sql2 = $" insert into REPORT_LOGIN_LOG(EMP_ID, EMP_PERMS, BACKUP, EMP_NAME) values('{EMP_ID}','User', 'Y', '{EMP_NAME}')";
                sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql2);
                ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql1);
            }
            string sql = $@" select CREATETIME, UPDATETIME, ID, PARENTID, NAME, TYPE, ROUTER, PERMS, ORDERNUM, VIEWPATH, KEEPALIVE, ISSHOW, ICON, SYSID, REMARK, COLUMN1, PERSON, OWNER, ISDELETE, EXPLAIN  from  REPORT_SYS_MANAGE_PERMISSION a
            RiGHT join (select ROLE_ID, ROUTER_ID from REPORT_ROLEFUNCTUION WHERE
		    ISDELETE <> 'N') b
            on a.ID = b.ROUTER_ID
            where SYSID = '{SYSID}' and a.ISDELETE <> 'N' AND b.ROLE_ID = '{ds.Tables[0].Rows[0][0]}' 
            order by ORDERNUM ";// format(inputInfo['SYSID'], res1[0][0])
            sqlExtension.SqlTxn.IgnoreDataSetCache = true;
            System.Data.DataSet ds1 = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql, "lot001");//這一句會忽略緩存
            List<object> list = new List<object>();
            for (int i = 0; i < ds1.Tables.Count; i++)
            {
                for (int j = 0; j < ds1.Tables[i].Rows.Count; j++)
                {
                    object ob = new
                    {
                        createTime = ds1.Tables[i].Rows[j][0],
                        updateTime = ds1.Tables[i].Rows[j][1],
                        id = ds1.Tables[i].Rows[j][2],
                        parentId = ds1.Tables[i].Rows[j][3],
                        name = ds1.Tables[i].Rows[j][4],
                        type = ds1.Tables[i].Rows[j][5],
                        router = ds1.Tables[i].Rows[j][6],
                        perms = ds1.Tables[i].Rows[j][7],
                        orderNum = ds1.Tables[i].Rows[j][8],
                        viewPath = ds1.Tables[i].Rows[j][9],
                        keepalive = ds1.Tables[i].Rows[j][10],
                        isShow = ds1.Tables[i].Rows[j][11],
                        icon = ds1.Tables[i].Rows[j][12],
                        SYSID = ds1.Tables[i].Rows[j][13],
                        REMARK = ds1.Tables[i].Rows[j][14],
                        PERSIN = ds1.Tables[i].Rows[j][16],
                        Owner = ds1.Tables[i].Rows[j][17],
                        Explain = ds1.Tables[i].Rows[j][19],
                        ticket
                    };
                    list.Add(ob);
                }
            }
            returnValue = list;
        }
        static string loginInnodriver(string uid, string pass)
        {
            //string url = "http://10.189.127.62:22000/api/Interface/GetAPTicket";
            //string url = "http://localhost:8000/api/Interface/GetAPTicket";
            string url = AppConfig.Innodriver_Url;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Account", "test");
            dic.Add("Password", "test");
            //dic["Account"] = "test";
            //dic["Password"] = "test";
            return invokeWebApi(url, dic);

        }
        static string invokeWebApi(string url, Dictionary<string, string> argu)
        {
            System.Net.HttpWebRequest client = System.Net.HttpWebRequest.Create(new Uri(url)) as System.Net.HttpWebRequest;
            client.Method = "POST";
            client.ContentType = "application/json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(argu);
            byte[] bs = Encoding.UTF8.GetBytes(json);
            client.ContentLength = bs.Length;

            using (System.IO.Stream reqStream = client.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (System.Net.WebResponse wr = client.GetResponse())
            {
                //在這裡對接收到的頁面內容進行處理
                using (System.IO.Stream resStream = wr.GetResponseStream())
                {
                    List<byte> lstByte = new List<byte>();
                    while (true)
                    {
                        int b = resStream.ReadByte();
                        if (b == -1) break;
                        lstByte.Add((byte)b);
                    }
                    string sResponse = Encoding.UTF8.GetString(lstByte.ToArray());
                    return sResponse;
                }
            }
        }

    }
}
