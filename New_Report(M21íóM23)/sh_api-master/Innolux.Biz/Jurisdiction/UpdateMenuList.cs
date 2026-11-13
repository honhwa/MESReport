using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Jurisdiction
{
    public class UpdateMenuList : InnSqlAgent.IApiBase
    {
        public UpdateMenuList()
        {
            parameters["id"] = "id";
            parameters["SYSID"] = "SYSID";
            parameters["orderNum"] = "orderNum";
            parameters["perms"] = "perms";
            parameters["type"] = "type";
            parameters["routeName"] = "name";
            parameters["parentId"] = "parentId";
            parameters["router"] = "router";
            parameters["icon"] = "icon";
            parameters["isShow"] = "isShow";
            parameters["viewPath"] = "viewPath";
            parameters["keepalive"] = "keepalive";
            parameters["menuId"] = "menuId";
            parameters["REMARK"] = "REMARK";
            parameters["PERSON"] = "人员信息";
            parameters["Owner"] = "报表Owner";
            parameters["Explain"] = "报表说明";
            description = "权限管理接口(更新菜单)";

        }
        public override void doWork()
        {
            string[] id = GetParameterValueInString("id").Split("','");
            string[] SYSID = GetParameterValueInString("SYSID").Split("','");
            string[] orderNum = GetParameterValueInString("orderNum").Split("','");
            string[] perms = GetParameterValueInString("perms").Split("','");
            string[] type = GetParameterValueInString("type").Split("','");
            string[] name = GetParameterValueInString("routeName").Split("','");
            string[] parentId = GetParameterValueInString("parentId").Split("','");
            string[] router = GetParameterValueInString("router").Split("','");
            string[] icon = GetParameterValueInString("icon").Split("','");
            string[] isShow = GetParameterValueInString("isShow").Split("','");
            string[] viewPath = GetParameterValueInString("viewPath").Split("','");
            string[] keepalive = GetParameterValueInString("keepalive").Split("','");
            string[] menuId = GetParameterValueInString("menuId").Split("','");
            string[] REMARK = GetParameterValueInString("REMARK").Split("','");
            string[] PERSON = GetParameterValueInString("PERSON").Split("','");
            string[] Owner = GetParameterValueInString("Owner").Split("','");
            string Explain = GetParameterValueInString("Explain");
            string sql;
            for (int i = 0; i < id.Length; i++)
            {
                if (type[i] == "0")
                {
                    sql = $@"update REPORT_SYS_MANAGE_PERMISSION set 
                TYPE='{type[i]}',NAME='{name[i]}',PARENTID='{parentId[i]}',ROUTER='{router[i]}',ICON='{icon[i]}',ISSHOW='{isShow[i]}',UPDATETIME=sysdate,REMARK = '{REMARK[i]}', PERSON = '{PERSON[i]}', EXPLAIN = '{Explain}', OWNER = '{Owner[i]}'
                where ID = '{id[i]}' and SYSID='{SYSID[i]}' and ORDERNUM = '{orderNum[i]}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'], inputInfo['type'], inputInfo['name'], inputInfo['parentId'], inputInfo['router'], inputInfo['icon'], inputInfo['isShow'])
                }
                else if (type[i] == "1")
                {
                    sql = $@" update REPORT_SYS_MANAGE_PERMISSION set
                TYPE='{type[i]}',NAME='{name[i]}',PARENTID='{parentId[i]}',ROUTER='{router[i]}',ICON='{icon[i]}',ISSHOW='{isShow[i]}',VIEWPATH='{viewPath[i]}',KEEPALIVE='{keepalive[i]}',UPDATETIME=sysdate,REMARK = '{REMARK[i]}', PERSON = '{PERSON[i]}', EXPLAIN = '{Explain}', OWNER = '{Owner[i]}'
                where ID = '{id[i]}' and SYSID='{SYSID[i]}' and ORDERNUM = '{orderNum[i]}' and ISDELETE <> 'N' ";//.format(inputInfo['id'], inputInfo['SYSID'], inputInfo['orderNum'], inputInfo['perms'], inputInfo['type'], inputInfo['name'], inputInfo['parentId'], inputInfo['router'], inputInfo['icon'], inputInfo['isShow'], inputInfo['viewPath'], inputInfo['keepalive'])
                }
                else
                {
                    sql = $@" update REPORT_SYS_MANAGE_PERMISSION set PERMS ='{perms[i]}',ORDERNUM='{orderNum[i]}',REMARK = '{REMARK[i]}', PERSON = '{PERSON[i]}', UPDATETIME=sysdate, EXPLAIN = '{Explain}', OWNER = '{Owner[i]}'
                where ID = '{menuId[i]}' and PARENTID='{parentId[i]}' and ISDELETE <> 'N' ";//.format(inputInfo['menuId'], inputInfo['parentId'], inputInfo['perms'], inputInfo['orderNum'])
                }
                //sqlExtension.SqlTxn.ExecuteSQL("MENUDB", sql);
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
}
