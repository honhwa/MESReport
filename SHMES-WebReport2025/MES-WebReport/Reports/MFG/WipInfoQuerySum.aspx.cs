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
using Microsoft.Reporting.WebForms;
using System.Text;
using Report.Library;
using Report.Sources;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
/*20120522      V1.00       Anday       SQL優化*/
namespace Report.Reports.MFG
{
    public partial class WipInfoQuerySum : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
          if (!IsPostBack)
            {
                bounddlN904();// add by vivian
                foreach (ListItem i in lstProductType.Items)
                {
                    i.Selected = true;
                }
                lstGrade.Items[0].Selected = true;
            }
        }

        private StringBuilder theSameSql(string strcode, string stropt1, string strtype)    // for 4 個 GridView 的DataSource.
        {
            StringBuilder stb = new StringBuilder();
            string strgrade;
            string s1 = string.Empty;
            /*V1.00 Start*/
            /*
stb.Append(@"select cworkctr,workname,sum(qty_wait) as 'W',sum(qty_run)  as 'R',sum(qty_hold) as 'H',sum(qty_wait+qty_run+qty_hold) as 'Total' from (select a.fabid,a.cworkctr,a.cshopcd,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD'from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[wltm] a left join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] b on a.prodid=b.prodid inner join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mwcm] c on a.cworkctr=c.workctr where 1=1 and a.Fabid='B' and a.hideflg='0' and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem])  group by a.cshopcd,a.cworkctr,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade");
stb.Append(@"  union all");
stb.Append(@"  select  a.fabid,a.cworkctr,a.cshopcd,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD' from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[wltm] a left join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem] b on a.prodid=b.prodid inner join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mwcm] c on a.cworkctr=c.workctr where 1=1 and a.Fabid='A' and a.hideflg='0' and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem])  group by a.cworkctr,a.cshopcd,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade) F where 1=1  ");
*/
            stb.Append(@"select cworkctr,workname,sum(qty_wait) as 'W',sum(qty_run)  as 'R',sum(qty_hold) as 'H',sum(qty_wait+qty_run+qty_hold) as 'Total' from (select a.fabid,a.cworkctr,a.cshopcd,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD'from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[wltm] a with(nolock) left join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] b with(nolock) on a.prodid=b.prodid inner join [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mwcm] c with(nolock) on a.cworkctr=c.workctr where 1=1 and a.Fabid='B' and a.hideflg='0' and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem]  with(nolock) where project_code='" + strcode + "' and opt1='" + stropt1 + "' and prodtype='" + strtype + "')  group by a.cshopcd,a.cworkctr,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade");
            stb.Append(@"  union all");
            stb.Append(@"  select  a.fabid,a.cworkctr,a.cshopcd,c.workname,b.prodtype,b.project_code,a.prodid,a.wonbr,a.lotgrade,sum(case a.entitysts when 'W' then entitytqty else 0 end) as 'QTY_WAIT',sum(case a.entitysts when 'R' then entitytqty else 0 end) as 'QTY_RUN',sum(case when (a.entitysts='W' or a.entitysts='R') then entitytqty else 0 end) as 'QTY_W+R',sum(case a.entitysts when 'H' then entitytqty else 0 end) as 'QTY_HOLD' from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[wltm] a left join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem] b with(nolock) on a.prodid=b.prodid inner join [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mwcm] c with(nolock) on a.cworkctr=c.workctr where 1=1 and a.Fabid='A' and a.hideflg='0' and substring((case when a.cartid is null or a.cartid = null then '0' when a.cartid = '' then '1' else a.cartid end ),1,1)<> 'F' and a.prodid in (select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem] with(nolock) where project_code='" + strcode + "' and opt1='" + stropt1 + "' and prodtype='" + strtype + "')  group by a.cworkctr,a.cshopcd,c.workname,b.prodtype,a.fabid,b.project_code,a.prodid,a.wonbr,a.lotgrade) F where 1=1  ");
            /*V1.00 End*/
            if (ddlFabid.SelectedValue == "LCM1")
            {
                stb.Append(@" and  f.fabid='A'");
            }
            else if (ddlFabid.SelectedValue == "LCM2")
            {
                stb.Append(@" and  f.fabid='B'");
            }
            else
            {
            }
            strgrade = rgrade(lstGrade);
            if (strgrade == "")
            {
            }
            else
            { stb.Append(@" and f.lotgrade in (" + strgrade + ") "); }
            return stb;
        }  

        protected void ButQuery_Click(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            DataSet rs;
            rs = DataBoundsummary(ddln904.SelectedValue,ddlFabid.SelectedValue );
            gvsummary.DataSource = rs;
            gvsummary.DataBind();
            showdata(lstProductType);
           
        }

        protected void chkGrade_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGrade.Checked)
            {
                foreach (ListItem i in lstGrade.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstGrade.Items)
                {
                    i.Selected = false;
                }
            }
        }

        protected void chkProductType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProductType.Checked)
            {
                foreach (ListItem i in lstProductType.Items)
                {
                    i.Selected = true;
                }
            }
            else
            {
                foreach (ListItem i in lstProductType.Items)
                {
                    i.Selected = false;
                }
            }
        }
       
        protected void lstProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkProductType.Checked = false;
        }

        protected void lstGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkGrade.Checked = false;
        }

        protected void ddln904_SelectedIndexChanged1(object sender, EventArgs e)//add by vivian
        {            
            string strtype;
            strtype = "select distinct project_code,opt1+' '+ prodtype as opt1 from mattributem  where project_code=" + "'" + ddln904.SelectedValue.Trim() + "' and prodtype!='' order by project_code,opt1";
            DataSet   dttype904 = this.dbConn.GetDataSetBySql(strtype);
            lstProductType.DataTextField = "opt1";
            lstProductType.DataValueField = "project_code";
            this.lstProductType.DataSource = dttype904;
            this.lstProductType.DataBind();
            chkProductType.Checked = false;
        }
        private void bounddlN904() // add by vivian
        {
            int i;
            string  strn904;
            strn904 = "select distinct project_code from mattributem where prodtype!=''";
            DataSet dtN904 = this.dbConn.GetDataSetBySql(strn904);
            for (i = 0; i<= dtN904.Tables[0].Rows.Count - 1; i++) 
            {
                ddln904.Items.Add(new ListItem(dtN904.Tables[0].Rows[i][0].ToString())); 
            }
            string strtype;
            strtype = "select distinct project_code,opt1+' '+ prodtype as opt1 from mattributem  where project_code=" + "'" + ddln904.SelectedValue.Trim() + "' and prodtype!='' order by project_code,opt1";
            DataSet dttype904 = this.dbConn.GetDataSetBySql(strtype);
            lstProductType.DataTextField = "opt1";
            lstProductType.DataValueField = "project_code";
            this.lstProductType.DataSource = dttype904;
            this.lstProductType.DataBind();  
        }
        private StringBuilder   DataBoundtype(string strcode,string stropt1,string strtype)          
        {
            StringBuilder stb_1 = theSameSql(strcode, stropt1, strtype);
            /*V1.00 Start*/
            /*
stb_1.Append("  AND f.prodid in ");
stb_1.Append(@"  (select prodid from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mattributem] where project_code='"+ strcode +"' and opt1='"+ stropt1 +"' and prodtype='"+ strtype +"'");
stb_1.Append(@" union select prodid from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mattributem] where project_code='"+ strcode +"' and opt1='"+ stropt1 +"' and prodtype='"+ strtype +"')");
             */
            /*V1.00 End*/
            //stb_1.Append(@" and f.cworkctr in (select workctr from [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[dbo].[mwcm] where shopcd='"+ stropt1 +"'" );
            //stb_1.Append(@" union select workctr from [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[dbo].[mwcm] where shopcd='" + stropt1 + "')");
            stb_1.Append(" group by cworkctr,workname order by cworkctr  compute sum(sum(qty_wait)),sum(sum(qty_run)),sum(sum(qty_hold)),sum(sum(qty_wait+qty_run+qty_hold));");
            return stb_1;
        }
        private DataSet DataBoundsummary(string strcode, string fabid)            // add by vivian ,show summary data
        {
            string strgrade;
            StringBuilder stb_1=new StringBuilder ();
            stb_1.Append(@"SELECT CSHOPCD,SUM(CASE WHEN CWORKCTR IN('8010','8015','8020') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS '待切割',");
            stb_1.Append (@"SUM(CASE WHEN CWORKCTR IN('8088','808D','808E','8090','8800') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS 'LOT1 OK',SUM(CASE WHEN CWORKCTR NOT IN('8010','8015','8020','8088','808D','808E','8090','8800') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS 'Cut~LOT',");
            stb_1.Append (@"SUM(CASE WHEN CWORKCTR IN('8200') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS '待COG',SUM(CASE WHEN CWORKCTR IN('8280') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS '待Bonding',SUM(CASE WHEN CWORKCTR IN('8290','8291','8360','8370') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS '待Ass',");
            stb_1.Append (@"SUM(CASE WHEN CWORKCTR IN('8380') AND ENTITYSTS IN('W','R') THEN ENTITYTQTY ELSE 0 END) AS 'M3Running' FROM(SELECT A.FABID,A.ENTITYID,A.CWORKCTR,A.ENTITYSTS,A.LOTGRADE,A.LOTLEVEL,A.ENTITYTQTY,A.CSHOPCD");
	        stb_1.Append (@",A.PRODID,A.N9NCNO,A.WOTYPE,A.WONBR,A.LOTTYPE,A.TRANSDATE,A.CARTID,B.PROJECT_CODE,B.PRODTYPE,C.WORKNAME FROM [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[DBO].[WLTM] A WITH(NOLOCK) INNER JOIN [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[DBO].[MATTRIBUTEM] B WITH(NOLOCK) ON(A.PRODID = B.PRODID)");
            stb_1.Append (@"LEFT JOIN [LCM1MESWIP\NJLCM1WIP].[NJPLCM1WIP].[DBO].[MWCM] C WITH(NOLOCK) ON(A.FABID = C.FABID AND A.CWORKCTR = C.WORKCTR) WHERE A.FABID = 'A' AND A.HIDEFLG = '0' UNION ALL ");
            stb_1.Append (@"SELECT A.FABID,A.ENTITYID,A.CWORKCTR,A.ENTITYSTS,A.LOTGRADE,A.LOTLEVEL,A.ENTITYTQTY,A.CSHOPCD,	A.PRODID,A.N9NCNO,A.WOTYPE,A.WONBR,A.LOTTYPE,A.TRANSDATE,A.CARTID,B.PROJECT_CODE,B.PRODTYPE,C.WORKNAME");
            stb_1.Append (@" FROM [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[DBO].[WLTM] A WITH(NOLOCK) INNER JOIN [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[DBO].[MATTRIBUTEM] B WITH(NOLOCK) ON(A.PRODID = B.PRODID) LEFT JOIN [NJLCM2MES01\NJLCM2WIP].[NJPLCM2WIP].[DBO].[MWCM] C WITH(NOLOCK) ON(A.FABID = C.FABID AND A.CWORKCTR = C.WORKCTR)");
            stb_1.Append (@" WHERE A.FABID = 'B' AND A.HIDEFLG = '0')A");
            stb_1.Append(@" WHERE SUBSTRING(ISNULL(CARTID,''),1,1)!='F' ");
            if (ddlFabid.SelectedValue=="LCM1" )
               {
                   stb_1.Append(@" and  a.fabid='A'");
               }
            else if (ddlFabid.SelectedValue == "LCM2")
                {
                    stb_1.Append(@" and  a.fabid='B'");
                }
            else
                { 
                }
            strgrade=rgrade(lstGrade);
                if ( strgrade== "")
                {                    
                }
                else
                { stb_1.Append(@" and a.lotgrade in (" + strgrade + ")"); }
            stb_1.Append(" and a.project_code='" + strcode +"'  ");
            stb_1.Append (@"GROUP BY CSHOPCD");     
            DataSet dtN90 = this.dbConn.GetDataSetBySql(stb_1.ToString());
            return dtN90;
        }
        private string rgrade(CheckBoxList cbl)
        {
            int i;
            string strgrade;
            strgrade = "";
            for (i = 0; i <= cbl.Items.Count - 1; i++)
            {
                if (cbl.Items[i].Selected)
                {
                    strgrade += "'" +cbl.Items[i].Value + "',";
                }
            }
            strgrade = strgrade.Substring(0, strgrade.Length - 1);
            return strgrade;
        }
        private void showdata(CheckBoxList lstprodtype) //add by vivian,show detail data
        {
            int i,j;
            string[] strtype;
            string[] strtitle;
            string strcode;
            DataSet dtdata;
            object [] drdata;            
            StringBuilder strsql= new StringBuilder ();
            pnln94.Controls.Clear();
            j = 0;
            strtitle = new string[lstprodtype.Items.Count];
            strtype = new string[lstprodtype.Items.Count];           
            for (i=0;i<=lstprodtype.Items.Count -1;i++)
            {                
                if (lstprodtype.Items[i].Selected) //show data
                {                    
                 strtype=lstprodtype.Items[i].Text.Split();
                 strcode=lstprodtype.Items[i].Value.ToString ().Trim ();
                 strtitle[j] = strcode + "  " + strtype[0].ToString() + "  " + strtype[1].ToString();
                 strsql.Append (DataBoundtype(strcode,strtype[0].ToString().Trim(),strtype[1].ToString().Trim()));
                 j = j + 1;
                 
                }
            }
            dtdata  = this.dbConn.GetDataSetBySql(strsql.ToString());
            for (i = 0; i <= dtdata.Tables.Count - 1; i++)
            { 
              if ((i%2)==0 && dtdata.Tables[i].Rows.Count >0)// detail info ,else is summary info
                {
                    drdata = new object[] { "Total", "", dtdata.Tables[i + 1].Rows[0][0].ToString().Trim(), dtdata.Tables[i + 1].Rows[0][1].ToString().Trim(), dtdata.Tables[i + 1].Rows[0][2].ToString().Trim(), dtdata.Tables[i + 1].Rows[0][3].ToString().Trim() };
                    dtdata.Tables[i].Rows.Add(drdata);
                    Label lbltxt = new Label();
                    GridView gvdata = new GridView();
                    lbltxt.Width = pnln94.Width;
                    gvdata.Width = pnln94.Width;
                    lbltxt.ForeColor = System.Drawing.Color.Red;
                    gvdata.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
                    lbltxt.ID = "lbltxt" + i.ToString();
                    gvdata.ID = "gvdata" + i.ToString();
                    pnln94.Controls.Add(lbltxt);
                    pnln94.Controls.Add(gvdata);
                    lbltxt.Text = strtitle[i / 2];
                    gvdata.DataSource = dtdata; 
                    gvdata.DataMember  = dtdata.Tables[i].ToString();                   
                    gvdata.DataBind();
                }
            }
        }


    }
}