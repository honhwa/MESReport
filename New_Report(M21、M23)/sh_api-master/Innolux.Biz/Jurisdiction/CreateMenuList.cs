using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Jurisdiction
{
    public class CreateMenuList : InnSqlAgent.IApiBase
    {
        public CreateMenuList()
        {
            parameters["id"] = "id";
            parameters["sysid"] = "sysid";
            parameters["ordernum"] = "ordernum";
            parameters["perms"] = "perms";
            parameters["type"] = "type";
            parameters["routename"] = "name";
            parameters["parentid"] = "parentid";
            parameters["router"] = "router";
            parameters["icon"] = "icon";
            parameters["isshow"] = "isshow";
            parameters["viewpath"] = "viewpath";
            parameters["keepalive"] = "keepalive";
            parameters["menuid"] = "menuid";
            parameters["remark"] = "remark";
            parameters["person"] = "人员信息";
            parameters["owner"] = "报表owner";
            parameters["explain"] = "报表说明";
            description = "权限管理接口(创建菜单)";

        }
        public override void doWork()
        {
            
            string id = GetParameterValueInString("id");
            string SYSID = GetParameterValueInString("SYSID");
            string orderNum = GetParameterValueInString("orderNum");
            string perms = GetParameterValueInString("perms");
            double type = GetParameterValueInDouble("type");
            string name = GetParameterValueInString("routeName");
            string parentId = GetParameterValueInString("parentId");
            string router = GetParameterValueInString("router");
            string icon = GetParameterValueInString("icon");
            string isShow = GetParameterValueInString("isShow");
            string viewPath = GetParameterValueInString("viewPath");
            string keepalive = GetParameterValueInString("keepalive");
            string menuId = GetParameterValueInString("menuId");
            string REMARK = GetParameterValueInString("REMARK");
            string PERSON = GetParameterValueInString("PERSON");
            string Owner = GetParameterValueInString("Owner");
            string Explain = GetParameterValueInString("Explain");
            string sql = "";
            string sql1;
            sql1 = $@" select * from  REPORT_SYS_MANAGE_PERMISSION where ISDELETE <> 'N' and ID = '{id}' and SYSID='{SYSID}' OR ORDERNUM = '{orderNum}'";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'])
            System.Data.DataSet ds = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql1);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                if (type == 0)
                {
                    sql = $@" insert into REPORT_SYS_MANAGE_PERMISSION(TYPE,NAME,PARENTID,ROUTER,ICON,ISSHOW,ORDERNUM,ID,SYSID,CREATETIME,UPDATETIME,PERMS,REMARK,PERSON,Owner,EXPLAIN,ISDELETE)values('{type}','{name}','{parentId}','{router}','{icon}','{isShow}','{orderNum}','{id}','{SYSID}',sysdate,sysdate,'{perms}','{REMARK}','{PERSON}','{Owner}','{Explain}','Y')";//.format(inputInfo['type'], inputInfo['name'], inputInfo['parentId'], inputInfo['router'], inputInfo['icon'], inputInfo['isShow'], inputInfo['orderNum'], inputInfo['id'], inputInfo['SYSID'], inputInfo['perms'])
                }
                else if (type == 1)
                {
                    sql = @$" insert into REPORT_SYS_MANAGE_PERMISSION(TYPE,NAME,PARENTID,ROUTER,ICON,ISSHOW,ORDERNUM,ID,VIEWPATH,KEEPALIVE,SYSID,CREATETIME,UPDATETIME,PERMS,REMARK,PERSON,Owner,EXPLAIN,ISDELETE)values('{type}', '{name}','{parentId}', '{router}','{icon}','{isShow}','{orderNum}','{id}','{viewPath}','{keepalive}','{SYSID}',sysdate,sysdate,'{perms}','{REMARK}','{PERSON}','{Owner}','{Explain}','Y')";//.format(inputInfo['type'], inputInfo['name'], inputInfo['parentId'], inputInfo['router'], inputInfo['icon'], inputInfo['isShow'], inputInfo['orderNum'], inputInfo['id'], inputInfo['viewPath'], inputInfo['keepalive'], inputInfo['SYSID'], inputInfo['perms'])
                }
            }

            //添加外部 URL  轉義 &
            // ? 也不能处理

            sql = sql.Replace("&", "'|| chr(38) || '").Replace("?", "'|| chr(63) || '");
            if (sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql) == 1)
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
