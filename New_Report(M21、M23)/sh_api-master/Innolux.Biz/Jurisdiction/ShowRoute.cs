using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innolux.Biz.Jurisdiction
{
    public class ShowRoute : InnSqlAgent.IApiBase
    {
        public ShowRoute()
        {
            description = "展示所有路由节点";
        }

        public override void doWork()
        {
            string sql = "SELECT * FROM REPORT_SYS_MANAGE_PERMISSION WHERE ISDELETE <> 'N' ";
            System.Data.DataSet ds1 = sqlExtension.SqlTxn.GetDataSet("MENUDB", sql);
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
                        Explain = ds1.Tables[i].Rows[j][19]
                    };
                    list.Add(ob);
                }
            }
            returnValue = list;
        }


    }
}
