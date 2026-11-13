using Report.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Report
{
    public partial class TreeMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //20211012 penny : fix session timeout throw exception 
                if (Session["UserInfo"] == null)
                {
                    Session["IsLogin"] = "N";
                    Response.Write("<script language='javascript'>window.top.location='Login.aspx';</script>");
                }
                else
                {
                    this.lblIp.Text = Request.UserHostAddress.ToString();
                    this.lblUserName.Text = ((UserInfo)Session["UserInfo"]).UserName;
                    //Load Menu
                    this.GetTreeView();
                }

            }            
        }

        private void GetTreeView()
        {
            try
            {
                DataSet dsMenu = new DataSet();
                DataSet dsNode = new DataSet();
                DBConn dbConn = new DBConn();
                string strSql = "select distinct fmenu,fmenu_id from MESReport_UserRole where ROLE IN('" + ((UserInfo)Session["UserInfo"]).Role + "') order by fmenu_id ";                                
                dsMenu = dbConn.GetDataSetBySql(strSql);
                //DataTable dt = dsMenu.Tables[0];
                TreeView1.Nodes.Clear();
                if(dsMenu.Tables[0].Rows.Count == 0)
                {
                    this.lbMsg.Text = "您没有可操作的菜单项，请与管理员联系!";
                }
                //建立根节点，遍历。
                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    TreeNode Root = new TreeNode();
                    Root.Text = dr["FMENU"].ToString();
                    TreeView1.Nodes.Add(Root);//建立根节点

                    string strSqlLayer1 = " select distinct  a.CMENU, a.URL,b.id from MESReport_UserRole a , MESReport_Menu b";
                    strSqlLayer1 = strSqlLayer1 + " where a.FMENU_ID = b.FMENU_ID AND a.cmenu = b.cmenu  AND a.url = b.url";
                    strSqlLayer1 = strSqlLayer1 + " and a.FMENU_ID='" + dr["FMENU_ID"].ToString() + "' and a.ROLE  IN('" + ((UserInfo)Session["UserInfo"]).Role + "')  order by b.id ";
                    dsNode = dbConn.GetDataSetBySql(strSqlLayer1);
                    DataTable dtNode = dsNode.Tables[0];
                    //建立子节点，遍历。
                    foreach (DataRow drNode in dtNode.Rows)
                    {
                        TreeNode Node = new TreeNode();
                        Node.Text = drNode["CMENU"].ToString();
                        Node.Target = "mainbottom";

                        string Path = drNode["URL"].ToString();
                        Node.NavigateUrl = Path;
                        Root.ChildNodes.Add(Node);//建立子节点
                        Node.Target = "main";
                    }
                    Root.Expanded = false;//節點収合
                }                
            }
            catch(Exception ex)
            {
                //Response.Write("您没有可操作的菜单项，请与管理员联系!" + "<br>" + ex.Message.ToString());
                this.lbMsg.Text = ex.Message;
            }
        }

        protected void ibtnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Session["IsLogin"] = "N";
            Session["UserInfo"] = null;
            Response.Write("<script language='javascript'>window.top.location='Login.aspx';</script>");
        }
    }
}