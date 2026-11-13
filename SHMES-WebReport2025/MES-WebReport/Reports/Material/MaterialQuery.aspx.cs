using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Report.Library;
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;
/*
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Report.Library;
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Text;
 */


namespace WebReport.Reports
{
    public partial class MaterialQuery : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!this.IsPostBack)
            {

                this.txtFromTime.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtToTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.Setlstpart();
                this.Setlsttype();
                this.Setlstline();
                this.Setlstshift();
                this.SetddlApplication();
                //this.DdlShift.SelectedIndex = 0;
                //SetDdl(this.DdlShift.Items[0].Value.Trim())
            }

            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
        }

        private void Setlstpart()
        {
            StringBuilder sbpart = new StringBuilder();
            sbpart.Append  ("SELECT DISTINCT Partid FROM MPAM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
            if (lsttype.Items.Count >0)
            {
                string strtype="";
                for(int i=0;i<lsttype.Items.Count ;i++)
                {
                    if   (((CheckBoxList)lsttype).Items[i].Selected==true)   
                    {
                       
                        strtype=strtype+"'"+ ((CheckBoxList)lsttype).Items[i].Value+ "'"+",";   
                    }
                      
                }
                if (strtype != "")
                {
                    strtype = strtype.Substring(0, strtype.Length - 1);
                    sbpart.Append(" AND PARTTYPE IN (" + strtype + ")");
                } 

            
            }

            sbpart.Append  (" ORDER BY PARTID");
            DataTable dt = this.dbConn.GetDataSetBySql(sbpart.ToString ()).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch PARTID!");
            }
            else
            {
                this.lstpart.DataSource = dt;
                this.lstpart.DataTextField = "PARTID";
                this.lstpart.DataValueField = "PARTID";
                this.lstpart.DataBind();
                //this.lstpart.Items.Insert(0, "ALL");
            }
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Setlstline()
        {
            string STRSQL = @"SELECT DISTINCT SUBLINEID FROM MSLM WHERE FABID ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY SUBLINEID";
            DataTable dt = this.dbConn.GetDataSetBySql(STRSQL).Tables[0];
            this.lstline.DataSource = dt;
            this.lstline.DataTextField = "SUBLINEID";
            this.lstline.DataValueField = "SUBLINEID";
            this.lstline.DataBind();
            //this.lstline.Items.Insert(0, "ALL");

        }

        protected void Setlsttype()
        {
            string STRSQL = @"SELECT DISTINCT PARTTYPE FROM MPAM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PARTTYPE";
            DataTable dt = this.dbConn.GetDataSetBySql(STRSQL).Tables[0];
            this.lsttype.DataSource = dt;
            this.lsttype.DataValueField = "parttype";
            this.lsttype.DataTextField = "parttype";
            this.lsttype.DataBind();
            //this.lsttype.Items.Insert(0, "ALL");
    
        }

        protected void Setlstshift()
        {
            string strsql = @"select DISTINCT SHIFTID FROM MSHM WHERE FABID='" + ((UserInfo)Session["USERINFO"]).FabId + "'ORDER BY SHIFTID";
            DataTable dt = this.dbConn.GetDataSetBySql(strsql).Tables[0];
            this.lstshift.DataSource = dt;
            this.lstshift.DataValueField = "shiftid";
            this.lstshift.DataTextField = "shiftid";
            this.lstshift.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;
            string strpart = "";
            string strline = "";
            string strshift = "";
            //string sShift = this.DdlShift.Items[this.DdlShift.SelectedIndex].Value.Trim();
            if (this.txtFromTime.Text == string.Empty) 
            {
                this.Response.Write ("<script language=javascript>window.alert('請選擇您需要的開始日期！');</script>");
                return ;
            }
            if (this.txtToTime.Text == string.Empty) 
            {
                this.Response.Write ("<script language=javascript>window.alert('請選擇您需要的結束日期！');</script>");
                return ;
            }
            if (this.txtFromTime.Text.Trim().Replace("'", "") != "" && this.txtToTime.Text.Trim().Replace("'", "") != "")
            {
                DateTime fr = new DateTime();
                DateTime to = new DateTime();
                bool bTimeFormatError = false;

                if (!DateTime.TryParse(this.txtFromTime.Text.Trim().Replace("'", ""), out fr))
                {
                    strErrorMsg += "開始時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!DateTime.TryParse(this.txtToTime.Text.Trim().Replace("'", ""), out to))
                {
                    strErrorMsg += "結束時間格式錯誤！\n";
                    this.lblErrMsg.Text = strErrorMsg;
                    return;
                    bTimeFormatError = true;
                }

                if (!bTimeFormatError)
                {
                    TimeSpan s = to - fr;
                    if (fr > to)
                    {
                        strErrorMsg += "結束時間在開始時間之前！\n";
                        this.lblErrMsg.Text = strErrorMsg;
                        return;
                    }

                }

            }

            if (lstpart.Items.Count > 0)
            {
               
                for (int i = 0; i < lstpart.Items.Count; i++)
                {
                    if (((CheckBoxList)lstpart).Items[i].Selected == true)
                    {

                        strpart = strpart + "'" + ((CheckBoxList)lstpart).Items[i].Value + "'" + ",";
                    }

                }
                if (strpart != "")
                {
                    strpart = strpart.Substring(0, strpart.Length - 1);
                }
            }

            if (lstline.Items.Count > 0)
            {
                
                for (int i = 0; i < lstline.Items.Count; i++)
                {
                    if (((CheckBoxList)lstline).Items[i].Selected == true)
                    {

                        strline = strline + "'" + ((CheckBoxList)lstline).Items[i].Value + "'" + ",";
                    }

                }
                if (strline != "")
                {
                    strline = strline.Substring(0, strline.Length - 1);
                }
                //if (strline == "")
                //{
                //    for (int j = 0; j < lstline.Items.Count; j++)
                //    {
                //        strline = strline + "'" + ((CheckBoxList)lstline).Items[j].Value + "'" + ",";
                //    }
                //    strline = strline.Substring(0, strline.Length - 1);
                //}
            }
            if (lstshift.Items.Count > 0)
            {

                for (int i = 0; i < lstshift.Items.Count; i++)
                {
                    if (((CheckBoxList)lstshift).Items[i].Selected == true)
                    {

                        strshift = strshift + "'" + ((CheckBoxList)lstshift).Items[i].Value + "'" + ",";
                    }

                }
                if (strshift != "")
                {
                    strshift= strshift.Substring(0, strshift.Length - 1);
                }
                //if (strshift == "")
                //{
                //    for (int j = 0; j < lstshift.Items.Count; j++)
                //    {
                //        strshift = strshift + "'" + ((CheckBoxList)lstshift).Items[j].Value + "'" + ",";
                //    }
                //    strshift = strshift.Substring(0, strshift.Length - 1);
                //}
            }

            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataTable dt=new DataTable ();
            StringBuilder sb=new StringBuilder ();
            // HISTORY TRANSACTION BETWEEN TIME PERIOD
            sb.Append("   select typename +case  ");
            sb.Append("   when typename in('mc2whs','mc2whn','mc2whg','rs2whs','mc2ls') then ltrim(pstockid)  ");
            sb.Append("   else ltrim(cstockid) end as typename,partid, ");
            sb.Append("   sum(partqty) as partqty into #vwpax_sum  ");
            sb.Append("   from ( ");
            sb.Append("   SELECT  ");
            sb.Append("   case  ");
            sb.Append("   when   cpartmode='g' and verb='mtra'  AND cstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   AND nstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("    then 'repare' ");
            sb.Append("   when  verb='wres' and cpartmode='s' and pstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   and nstockid in(select stockid from mstockm where stocktype ='wh') ");
            sb.Append("   then 'mc2whs' ");
            sb.Append("   when verb='wres' and cpartmode='s' and pstockid in(select stockid from mstockm where stocktype ='RS') ");
            sb.Append("   and nstockid in(select stockid from mstockm where stocktype ='wh') ");
            sb.Append("   then 'rs2whs'  ");
            sb.Append("   when verb='sflt'  ");
            sb.Append("   then 'rsp2wh'  ");
            sb.Append("   when verb='wrec' and cpartmode='g' ");
            sb.Append("   then 'mc2whg' ");
            sb.Append("   when verb='wrec' and cpartmode='n' ");
            sb.Append("   then 'mc2whn' ");
            sb.Append("   when verb='mrec'  and pstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   and cstockid in(select stockid from mstockm where stocktype ='ls') ");
            if (this.chkAllline.Checked == false &&  strline != "")
            {
                sb.Append(" AND CSTOCKID IN (" + strline + ")");
            }
            sb.Append("   then 'mc2ls' ");
            sb.Append("   when verb='mrec' and cpartmode='g'  and pstockid in(select stockid from mstockm where stocktype ='ls') ");
            if (this.chkAllline.Checked == false && strline != "")
            {
                sb.Append(" AND PSTOCKID IN (" + strline + ")");
            }
            sb.Append("   and cstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   then 'ls2mcg' ");
            sb.Append("   when verb='mrec' and cpartmode='n'  and pstockid in(select stockid from mstockm where stocktype ='ls') ");
            if (this.chkAllline.Checked == false && strline != "")
            {
                sb.Append(" AND PSTOCKID IN (" + strline + ")");
            }
            sb.Append("   and cstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   then 'ls2mcn' ");
            sb.Append("   when verb='mrec' and cpartmode='s'  and pstockid in(select stockid from mstockm where stocktype ='ls') ");
            if (this.chkAllline.Checked == false && strline != "")
            {
                sb.Append(" AND PSTOCKID IN (" + strline + ")");
            }
            sb.Append("   and cstockid in(select stockid from mstockm where stocktype ='mc') ");
            sb.Append("   then 'ls2mcs' ");
            sb.Append("   else '' ");
            sb.Append("    end  as typename,partid,cstockid,pstockid,partqty ");
            sb.Append("   from New_vwpax where transdate between '" + this.txtFromTime.Text.Trim() + "' and '" + this.txtToTime.Text.Trim() + "' ");
            sb.Append("   and fabid='b' and verb in('mtra','wres','sflt','wrec','mrec') ");
            if (this.chkAllpart.Checked == false && strpart !="")
            {
                sb.Append(" AND PARTID IN(" + strpart + ") ");
            }
            if (this.chkAllshift.Checked == false && strshift != "")
            {
                sb.Append(" AND SHIFTID IN(" + strshift + ")");
            }
            sb.Append("   ) bb  where typename <>'' ");
            sb.Append("   group by typename,partid,case when typename in('mc2whs','mc2whn','mc2whg','rs2whs','mc2ls') then LTRIM(pstockid)  ");
            sb.Append("   else LTRIM(cstockid) end   order by typename,partid ");
            sb.Append("    ");
            //USGE INFORMATION ,BETWEEN TIME PERIOD
            sb.Append("    SELECT V.PARTID,M.PRODGROUP,SUM(V.PARTQTY) AS partQTY into #vwpax_usge  ");
            sb.Append("   FROM New_VWPAX V WITH(NOLOCK) ");
            sb.Append("    LEFT JOIN New_SWOM S WITH(NOLOCK) ON(V.FABID=S.FABID AND V.TOWONBR=S.WONBR) ");
            sb.Append("    LEFT JOIN MPIM M WITH(NOLOCK) ON(V.FABID=M.FABID AND S.PRODID=M.PRODID) ");
            sb.Append("   where transdate between '" + this.txtFromTime.Text.Trim() + "' and '" + this.txtToTime.Text.Trim() + "' ");
            sb.Append("   and v.fabid='b' and verb='usge' ");
            if (this.chkAllpart.Checked == false && strpart != "")
            {
                sb.Append(" AND V.PARTID IN(" + strpart + ") ");
            }
            if (this.chkAllshift.Checked == false && strshift != "")
            {
                sb.Append(" AND SHIFTID IN(" + strshift + ")");
            }
            if (this.chkAllline.Checked == false && strline != "")
            {
                sb.Append(" AND CSTOCKID IN (" + strline + ")");
            }
            sb.Append("   group by v.partid,m.prodgroup ");
            sb.Append("    ");
            // ON TIME WPAM STORE
            sb.Append("   select case when partmode='g' then 'mcg' when partmode='n' then 'mcn' when partmode='s' then 'mcs' ");
            sb.Append("   end + ltrim(cstockid) as typename,partid,sum(partqty) as partqty  into #wpam ");
            sb.Append("    from New_wpam where fabid='b' and partqty<>'0' and  ");
            sb.Append("   cstockid in(select stockid from mstockm where stocktype='mc') ");
            if (this.chkAllpart.Checked == false && strpart != "")
            {
                sb.Append(" AND PARTID IN(" + strpart + ") ");
            }

            sb.Append("   group by case when partmode='g' then 'mcg' when partmode='n' then 'mcn' when partmode='s' then 'mcs' ");
            sb.Append("   end,partid,cstockid order by typename ");
            sb.Append("    ");

            sb.Append("   select aa.* into #summary from ( ");
            sb.Append("   select A.* ,ISNULL(B.PRODGROUP,'') as prodgroup from #vwpax_sum A LEFT JOIN #VWPAX_USGE B ON A.PARTID=B.PARTID");
            sb.Append("   union all ");
            sb.Append("   select 'usge' as typename,partid, partqty,prodgroup from #vwpax_usge ");
            sb.Append("   union all ");
            sb.Append("   select A.*,ISNULL(B.PRODGROUP,'')  as prodgroup  from #wpam A LEFT JOIN #VWPAX_USGE B ON A.PARTID=B.PARTID ) aa  ");
        

            sb.Append("    ");
            sb.Append("    ");
            sb.Append("   select partid,prodgroup,[ls2mcg5101],[ls2mcg5103],[ls2mcg5105],[ls2mcg5107],[ls2mcg5109],[ls2mcn5101],");
            sb.Append("   [ls2mcn5103],[ls2mcn5105],[ls2mcn5107],[ls2mcn5109],[ls2mcs5101],[ls2mcs5103],[ls2mcs5105],[ls2mcs5107],[ls2mcs5109],");
            sb.Append("   [mc2ls5101],[mc2ls5103],[mc2ls5105],[mc2ls5107],[mc2ls5109],[mc2whg5101],[mc2whg5103],[mc2whg5105],[mc2whg5107],[mc2whg5109],");
            sb.Append("   [mc2whn5101],[mc2whn5103],[mc2whn5105],[mc2whn5107],[mc2whn5109],[mc2whs5101],[mc2whs5103],[mc2whs5105],[mc2whs5107],[mc2whs5109],");
            sb.Append("   [mcg5101],[mcg5103],[mcg5105],[mcg5107],[mcg5109],[mcn5101],[mcn5103],[mcn5105],[mcn5107],[mcn5109],[mcs5101],[mcs5103],[mcs5105],");
            sb.Append("   [mcs5107],[mcs5109],[repare5101],[repare5103],[repare5105],[repare5107],[repare5109],[rs2whs9995],[rsp2wh],[usge] from");
            sb.Append("   (select typename,partqty,partid ,prodgroup from #summary ");
            if (ddlApplication.Text.Trim() != "ALL")
            {
                sb.Append(" WHERE ProdGroup!=''");
            }
            sb.Append("   ) p ");
            sb.Append("   pivot  ");
            sb.Append("   ( ");
            sb.Append("       max(partqty) ");
            sb.Append("   for typename in([ls2mcg5101],[ls2mcg5103],[ls2mcg5105],[ls2mcg5107],[ls2mcg5109],[ls2mcn5101],[ls2mcn5103],[ls2mcn5105],[ls2mcn5107],[ls2mcn5109],[ls2mcs5101],[ls2mcs5103],[ls2mcs5105],[ls2mcs5107],[ls2mcs5109],[mc2ls5101],[mc2ls5103],[mc2ls5105],[mc2ls5107],[mc2ls5109],[mc2whg5101],[mc2whg5103],[mc2whg5105],[mc2whg5107],[mc2whg5109],[mc2whn5101],[mc2whn5103],[mc2whn5105],[mc2whn5107],[mc2whn5109],[mc2whs5101],[mc2whs5103],[mc2whs5105],[mc2whs5107],[mc2whs5109],[mcg5101],[mcg5103],[mcg5105],[mcg5107],[mcg5109],[mcn5101],[mcn5103],[mcn5105],[mcn5107],[mcn5109],[mcs5101],[mcs5103],[mcs5105],[mcs5107],[mcs5109],[repare5101],[repare5103],[repare5105],[repare5107],[repare5109],[rs2whs9995],[rsp2wh],[usge] ");
            sb.Append("   ) ");
            sb.Append("   ) ");
            sb.Append("   as pvt ");
            sb.Append("   order by partid ");

        

            dt=this.dbConn.GetDataSetBySql(sb.ToString()).Tables[0];
            this.RVMaterial.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.RVMaterial.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\MaterialReport.rdlc";


            ReportDataSource rsd = new ReportDataSource("DataSet1_MPAM", dt);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);

        }

        protected void chkAlltype_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAlltype.Checked)
            {
                foreach (ListItem i in this.lsttype.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lsttype.Items)
                {
                    i.Selected = false;
                }
            }
            Setlstpart();
        }

        protected void chkAllpart_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllpart.Checked)
            {
                foreach (ListItem i in this.lstpart.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstpart.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkAllline_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllline.Checked)
            {
                foreach (ListItem i in this.lstline.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstline.Items)
                {
                    i.Selected = false;
                }
            }
        }
        protected void chkAllshift_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAllshift.Checked)
            {
                foreach (ListItem i in this.lstshift.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in this.lstshift.Items)
                {
                    i.Selected = false;
                }
            }
        }
        protected void lsttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lsttype.Items )
            {
                if (!i.Selected)
                {
                    this.chkAlltype.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAlltype.Checked = true;
            }
            Setlstpart();
        }
        
        protected void lstpart_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstpart.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllpart .Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllpart .Checked = true;
            }
        }

        protected void lstline_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstline.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllline  .Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllline.Checked = true;
            }
        }
        protected void lstshift_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bResult = true;
            foreach (ListItem i in this.lstshift.Items)
            {
                if (!i.Selected)
                {
                    this.chkAllshift.Checked = false;
                    bResult = false;
                    break;
                }
            }

            if (bResult)
            {
                this.chkAllshift.Checked = true;
            }
        }

        protected void SetddlApplication()
        {
            string strSql = @"SELECT DISTINCT PRODGROUP FROM MPIM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ORDER BY PRODGROUP";
            DataTable dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                Response.Write("Cannot Fetch ProdId!");
            }
            else
            {
                this.ddlApplication.DataSource = dt;
                this.ddlApplication.DataTextField = "PRODGROUP";
                this.ddlApplication.DataValueField = "PRODGROUP";
                this.ddlApplication.DataBind();
                this.ddlApplication.Items.Insert(0, "ALL");
            }
        }
        /*protected void DdlShift_textChanged(object sender, EventArgs e)
        {
            string sShift = this.DdlShift.Items[this.DdlShift.SelectedIndex].Value.Trim();
            //SetDdl(sShift);
        }

        protected void DdlShift_SelectedIndexChanged(object sender, EventArgs e)
        {

        }*/


    }
}
