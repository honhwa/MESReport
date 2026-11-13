using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using System.Web.Hosting;
using System.Data.SqlClient;
using Report.Library;
using Microsoft.Reporting.WebForms;

namespace WebReport.Reports
{
	/// <summary>
	/// FrmWipSummary 的摘要描述。
	/// </summary>
	public partial class FrmEQSMMSummary : System.Web.UI.Page
	{

        DBConn dbConn = new DBConn();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在這裡放置使用者程式碼以初始化網頁
            if (!this.IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    this.Response.Write("<script language=javascript>window.alert('請先登陸,再使用,謝謝');</script>");
                    Response.Redirect("../../Login.aspx");
                }
                this.TextBox1.Text = this.Request.QueryString["FrTime"].ToString();
                this.TextBox2.Text = this.Request.QueryString["ToTime"].ToString();
                this.TextBox3.Text = this.Request.QueryString["Process"].ToString();

                BindDataToDataGrid();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
            }
		}
		#region Web Form 設計工具產生的程式碼
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
        //    //
        //    InitializeComponent();
        //    base.OnInit(e);
        //}
		
		/// <summary>
		/// 此為設計工具支援所必須的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
        //private void InitializeComponent()
        //{    
        //    this.ImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButton2_Click);
        //    this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
        //}
		#endregion
		private void BindDataToDataGrid()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
			StringBuilder strsql=new StringBuilder();
			StringBuilder sql=new StringBuilder();
			StringBuilder sql0=new StringBuilder();
			/*跑所有的資料*/
            strsql.Append(" select a.fabid,a.sublineid as eqid,");
            strsql.Append(" round(sum(case when a.eqccd='R' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as Runtime ,");
            strsql.Append(" round(sum(case when a.eqccd='I' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as IDELTime,");
            strsql.Append(" round(sum(case when a.eqccd='D' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as DOWNTime,");
            strsql.Append(" round(sum(case when a.eqccd='S' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as SETUPTime,");
            strsql.Append(" round(sum(case when a.eqccd='W' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as WAITTime,");
            strsql.Append(" round(sum(case when a.eqccd='P' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as PMTime,");
            strsql.Append(" round(sum(case when a.eqccd='N' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as NonOperationTime, ");
            strsql.Append(" sum(case when a.eqccd in('W') then 1 else 0 end) as RCount, ");
            strsql.Append(" round(sum(case when a.eqccd='E' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as ENGTime,");
            strsql.Append(" round(sum(case when a.eqccd='O' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as OtherTime,");
            strsql.Append(" round(sum(case when a.eqccd='T' then convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end))) else 0 end)/60,2) as PartialDownTime ");
            strsql.Append(" from ");
            strsql.Append(" (");
            strsql.Append(" SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
            strsql.Append(" FROM rep.ESLSH WHERE  FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            strsql.Append(" UNION ALL");
            strsql.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,GETDATE()  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
            strsql.Append(" FROM rep.ESLS WHERE   FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            strsql.Append(" ) A");
            strsql.Append(" INNER JOIN MSLM D ON (A.FABID=D.FABID AND D.SUBLINEID=A.SUBLINEID)");
            strsql.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND D.WORKCTR=B.WORKCTR) ");
            strsql.Append(" where a.fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'    ");
            strsql.Append(" AND (case when a.enddate is null then getdate() else a.enddate end)>convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) AND A.STARTDATE<=convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) "); ;
            strsql.Append(" AND CASE WHEN A.SHOPCD = 'CELL2' THEN B.WORKCTR ELSE  b.procid  END='" + this.TextBox3.Text.Trim() + "'");
            strsql.Append(" group by a.fabid,a.sublineid ");
            strsql.Append(" ORDER by a.fabid,a.sublineid ");

            
            SqlConnection myConn4 = new SqlConnection(dbConn.ConnectionString);
            SqlDataAdapter strdf = new SqlDataAdapter(strsql.ToString(), myConn4);
            DataSet dt = new DataSet();
            myConn4.Open();
            strdf.Fill(dt, "A");
            myConn4.Close();

            /*跑DownTime的資料*/
       /*     sql.Append(" select a.fabid,a.sublineid as eqid,");
            sql.Append(" round(convert(int,datediff(minute,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end)))/60,0) as DOWNTime,");
            sql.Append(" round(convert(int,datediff(hour,convert(datetime,'" + this.TextBox1.Text.Trim() + "',20),convert(datetime,'" + this.TextBox2.Text.Trim() + "',20))*20) ,0)");
            sql.Append(" from ");
            sql.Append(" (");
            sql.Append(" SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
            sql.Append(" FROM ESLSH WHERE  FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PREVEQCCD in ('D','W','R')");
            sql.Append(" UNION ALL ");
            sql.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,GETDATE()  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
            sql.Append(" FROM ESLS WHERE   FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQCCD in ('D','W','R')");
            sql.Append(" ) A");
            sql.Append(" INNER JOIN MSLM D ON (A.FABID=D.FABID AND D.SUBLINEID=A.SUBLINEID)");
            sql.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND D.WORKCTR=B.WORKCTR) ");
            sql.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD)");
            sql.Append(" where a.fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND (C.EQCD='EDO01') ");
            sql.Append(" AND (case when a.enddate is null then getdate() else a.enddate end)<=convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) AND A.STARTDATE>=convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) ");
            sql.Append(" AND CASE WHEN A.SHOPCD = 'CELL2' THEN B.WORKCTR ELSE  b.procid  END='" + this.TextBox3.Text.Trim() + "'");
            sql.Append(" ORDER by a.fabid,a.sublineid ");
*/

     sql.Append(" select a.fabid,a.sublineid as eqid,");
     sql.Append(" round(convert(int,datediff(second,(case when (case when a.startdate is null then getdate() else a.startdate end)> convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) then (case when a.startdate is null then getdate() else a.startdate end) else convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) end),(case when (case when a.enddate is null then getdate() else a.enddate end)< convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) then (case when a.enddate is null then getdate() else a.enddate end) else convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) end)))/60,2) as DOWNTime,");
     sql.Append(" round(convert(int,datediff(hour,convert(datetime,'" + this.TextBox1.Text.Trim() + "',20),convert(datetime,'" + this.TextBox2.Text.Trim() + "',20))*20) ,0)");
     sql.Append(" ,A.EQCCD,A.WORKCTR,a.startdate");
     sql.Append(" from ");
     sql.Append(" (");
     sql.Append(" SELECT FABID,SUBLINEID,SHOPCD,WORKCTR,PREVSTATUS AS SUBLINESTATUS,PREVEQCCD AS EQCCD,STARTDATE,ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
   //  sql.Append(" FROM ESLSH WHERE  FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PREVEQCCD in ('D','W','R','I')");
     sql.Append(" FROM rep.ESLSH WHERE  FABID='" + ((UserInfo)Session["UserInfo"]).FabId+"'" );
     sql.Append(" UNION ALL ");
     sql.Append(" SELECT  FABID,SUBLINEID,SHOPCD,WORKCTR,SUBLINESTATUS,EQCCD, TRANSDATE AS STARTDATE,GETDATE()  AS ENDDATE,TRANSDATE,USERID,SHIFTID,CMT,PRODID ");
    // sql.Append(" FROM ESLS WHERE   FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "' AND EQCCD in ('D','W','R','I')");
     sql.Append(" FROM rep.ESLS WHERE   FABID='" + ((UserInfo)Session["UserInfo"]).FabId +"'");
     sql.Append(" ) A");
     sql.Append(" INNER JOIN MWCM B ON(A.FABID=B.FABID AND A.WORKCTR=B.WORKCTR) ");
     sql.Append(" LEFT JOIN MECM C ON(A.FABID = C.FABID AND A.EQCCD = C.EQCCD AND A.SUBLINESTATUS = C.EQCD)");
     sql.Append(" INNER JOIN MSLM D ON (A.FABID=D.FABID AND D.SUBLINEID=A.SUBLINEID)");
     sql.Append(" where a.fabid='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
  //   sql.Append(" AND (C.EQCD='EDO01') ");
     sql.Append(" AND (case when a.enddate is null then getdate() else a.enddate end)>convert(datetime,'" + this.TextBox1.Text.Trim() + "',20) AND A.STARTDATE<=convert(datetime,'" + this.TextBox2.Text.Trim() + "',20) "); ;
     sql.Append(" AND CASE WHEN A.SHOPCD = 'CELL2' THEN B.WORKCTR ELSE  b.procid  END='" + this.TextBox3.Text.Trim() + "'");
     sql.Append(" ORDER by a.fabid,a.sublineid,A.WORKCTR,a.startdate ");


     SqlConnection myConn3 = new SqlConnection(dbConn.ConnectionString);
     SqlDataAdapter dfsql = new SqlDataAdapter(sql.ToString(), myConn3);
     DataSet dtsql = new DataSet();
     myConn3.Open();
     dfsql.Fill(dtsql, "A");
     myConn3.Close();

     /*跑DownTime的資料*/
			//sql0.Append(" Select trunc(to_number(ts2d(to_timestamp('"+this.TextBox2.Text.Trim()+"','yyyy/mm/dd hh24:mi:ss.ff6'))-ts2d(to_timestamp('"+this.TextBox1.Text.Trim()+"','yyyy/mm/dd hh24:mi:ss.ff6')))*20,0)");
			//sql0.Append(" from dual");

            sql0.Append("select round(convert(int,datediff(hour,convert(datetime,'" + this.TextBox1.Text.Trim() + "',20),convert(datetime,'" + this.TextBox2.Text.Trim() + "',20))*20) ,0) as dual");

            SqlConnection myConn2 = new SqlConnection(dbConn.ConnectionString);
            SqlDataAdapter dfsql0 = new SqlDataAdapter(sql0.ToString(), myConn2);
            DataSet dtsql0 = new DataSet();
            myConn2.Open();
            dfsql0.Fill(dtsql0, "dual");
            myConn2.Close();


            //------填充DataTable設定格式-------------------------------------------------------------
			DataTable dtm1 = new DataTable();

			dtm1.Columns.Add(new DataColumn("EQLine",typeof(string)));
			dtm1.Columns.Add(new DataColumn("RunTime" ,typeof(string)));
            dtm1.Columns.Add(new DataColumn("IdelTime", typeof(string)));

            dtm1.Columns.Add(new DataColumn("ENGTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("OtherTime", typeof(string)));
            dtm1.Columns.Add(new DataColumn("PartialDownTime", typeof(string)));

			dtm1.Columns.Add(new DataColumn("DownTime" ,typeof(string)));
			dtm1.Columns.Add(new DataColumn("SetupTime" ,typeof(string)));
			dtm1.Columns.Add(new DataColumn("WaitTime" ,typeof(string)));
			dtm1.Columns.Add(new DataColumn("PMTime" ,typeof(string)));
            dtm1.Columns.Add(new DataColumn("NonOperationTime", typeof(string)));
			dtm1.Columns.Add(new DataColumn("Availability" ,typeof(string)));
			dtm1.Columns.Add(new DataColumn("Utilization" ,typeof(string)));
			dtm1.Columns.Add(new DataColumn("SetupRate",typeof(string)));
			dtm1.Columns.Add(new DataColumn("MTTR",typeof(string)));
			dtm1.Columns.Add(new DataColumn("MTBF",typeof(string)));

            double Srun = 0, Sidel = 0, Sdown = 0, Ssetup = 0, Swait = 0, Spm = 0, Snprd = 0, SEng = 0, SOther = 0, SPartial = 0; 
			double TTime=0;
            double MTTRTotleTime = 0, MTBFTotleTime = 0;
            int MTTRTotleK = 0, MTBFTotleK = 0;
			if(dtsql.Tables[0].Rows.Count==0)
			{
				TTime=Convert.ToDouble(dtsql0.Tables[0].Rows[0][0].ToString());
			}
			else
			{
				TTime=Convert.ToDouble(dtsql.Tables[0].Rows[0][3].ToString());
			}

			for(int i=0;i<dt.Tables[0].Rows.Count;i++)
			{
                double Trun = 0, Tidel = 0, Tdown = 0, Tsetup = 0, Twait = 0, Tpm = 0, Tnprd = 0, TEng = 0, TOther = 0, TPartial = 0;

				DataRow drm1=dtm1.NewRow();
				drm1.Table.Columns.Add();

				drm1["EQLine"]=dt.Tables[0].Rows[i][1].ToString();
				drm1["RunTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][2].ToString())/60,2);
				Trun=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][2].ToString())/60,2);
				Srun=Srun+Trun;
				drm1["IdelTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][3].ToString())/60,2);
				Tidel=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][3].ToString())/60,2);
				Sidel=Sidel+Tidel;
				drm1["DownTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString())/60,2);
				Tdown=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString())/60,2);
				Sdown=Sdown+Tdown;
				drm1["SetupTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString())/60,2);
				Tsetup=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString())/60,2);
				Ssetup=Ssetup+Tsetup;
				drm1["WaitTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][6].ToString())/60,2);
				Twait=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][6].ToString())/60,2);
				Swait=Swait+Twait;
				drm1["PMTime"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString())/60,2);
				Tpm=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString())/60,2);
				Spm=Spm+Tpm;
                drm1["NonOperationTime"] = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString()) / 60, 2);
				Tnprd=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString())/60,2);
				Snprd=Snprd+Tnprd;
                drm1["ENGTime"] = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][10].ToString()) / 60, 2);
                TEng = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][10].ToString()) / 60, 2);
                SEng = SEng + TEng;
                drm1["OtherTime"] = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][11].ToString()) / 60, 2);
                TOther = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][11].ToString()) / 60, 2);
                SOther = SOther + TOther;
                drm1["PartialDownTime"] = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][12].ToString()) / 60, 2);
                TPartial = Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][12].ToString()) / 60, 2);
                SPartial = SPartial + TPartial;


        if((Trun+Tidel+Tdown+Tsetup+Twait+Tpm)!=0)
        {
            drm1["Availability"] = Math.Round((Trun + Tidel + Tsetup) / (Trun + Tidel + Tdown + Tsetup + Twait + Tpm + TPartial) * 100, 2) + "%";
        }
        else
        {
            drm1["Availability"]="100%";
        }
        if((Trun+Tidel+Tsetup)!=0)
        {
            drm1["Utilization"]=Math.Round((Trun)/(Trun+Tidel+Tsetup)*100,2)+"%";
            drm1["SetupRate"]=Math.Round((Tsetup)/(Trun+Tidel+Tsetup)*100,2)+"%";
        }
        else
        {
            drm1["Utilization"]="100%";
            drm1["SetupRate"]="100%";
        }
    /*	if(Convert.ToDouble(dt.Tables[0].Rows[i][9].ToString())==0)
        {
            drm1["MTTR"]="0";
        }
        else //MTTR=WAIT+DOWN時間和/Wait次數
        {
            drm1["MTTR"]=Math.Round(Convert.ToDouble(dt.Tables[0].Rows[i][4]+dt.Tables[0].Rows[i][6].ToString())/Convert.ToDouble(dt.Tables[0].Rows[i][9].ToString()),2);
        }
    */
                /*MTTR 計次的方式: 
                1.如果只有Wait ,計一次 
                2.如果只有down ,計一次 
                3.wait-->down,計一次 
                 */
                double dbdifftime = 0, dbtime = 0,sumtimeWD=0;
                int k = 0,j=0;


        /*        for (j = 0; j < dtsql.Tables[0].Rows.Count-1; j++)
                {
                    if (dtsql.Tables[0].Rows[j][1].ToString() == dt.Tables[0].Rows[i][1].ToString())
                    {
                        if (dtsql.Tables[0].Rows[j][4].ToString() == "W" && dtsql.Tables[0].Rows[j+1][4].ToString() == "D")
                        {
                            k = k + 1;
                            sumtimeWD = sumtimeWD + Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                        }
                        if (dtsql.Tables[0].Rows[j][4].ToString() == "W" && dtsql.Tables[0].Rows[j + 1][4].ToString() != "D")
                        {
                            k = k + 1;
                            sumtimeWD = sumtimeWD + Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                        }
                        if (j == 0 && dtsql.Tables[0].Rows[j][4].ToString() == "D") 
                        {
                           k = k + 1;
                           sumtimeWD = sumtimeWD + Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                        }
                        if (j > 0 && dtsql.Tables[0].Rows[j][1].ToString() == dtsql.Tables[0].Rows[j-1][1].ToString())
                        { 
                            if ( dtsql.Tables[0].Rows[j - 1][4].ToString() != "W" && dtsql.Tables[0].Rows[j][4].ToString() == "D")
                            {
                                k = k + 1;
                                sumtimeWD = sumtimeWD + Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                            }
                        }
                        if (j > 0 && dtsql.Tables[0].Rows[j][1].ToString() != dtsql.Tables[0].Rows[j - 1][1].ToString())
                        {
                            if (dtsql.Tables[0].Rows[j][4].ToString() == "D")
                            {
                                k = k + 1;
                                sumtimeWD = sumtimeWD + Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                            }
                        }

                    }
                }
*/

                DataRow[] dtmttr = dtsql.Tables[0].Select("(eqid='" + dt.Tables[0].Rows[i][1].ToString() + "')");
                if (dtmttr.Length > 0)
                {
                    for (int t = 0; t < Convert.ToInt32(dtmttr.Length); t++)
                    {
                        if (dtmttr[t].ItemArray[4].ToString() =="W" || dtmttr[t].ItemArray[4].ToString() == "D")
                        {
                            k = k + 1;
                            sumtimeWD = sumtimeWD + Convert.ToDouble(dtmttr[t].ItemArray[2]);
                        }
                        if (t > 0 && dtmttr[t].ItemArray[4].ToString() == "D" && dtmttr[t - 1].ItemArray[4].ToString() == "W")
                        {
                            k = k - 1; //W,D在一起時，扣除多加的次
                        }

                    }
                }
                else
                {
                    drm1["MTTR"] = "0";
                }
                if (k > 0)
                {
                 //  drm1["MTTR"] = Math.Round((Convert.ToDouble(dt.Tables[0].Rows[i][4])+Convert.ToDouble(dt.Tables[0].Rows[i][6])) / Convert.ToInt32(k),2);
                   drm1["MTTR"] = Math.Round(sumtimeWD / Convert.ToInt32(k), 2);
                   MTTRTotleTime = MTTRTotleTime + Math.Round((Convert.ToDouble(dt.Tables[0].Rows[i][4]) + Convert.ToDouble(dt.Tables[0].Rows[i][6])) , 2);
                   MTTRTotleK = MTTRTotleK + k; 
                }
                else
                {
                    drm1["MTTR"] = "0";
                }
    

              //  this.DataGrid1.Items[i].Cells[2].ToolTip = Convert.ToString(k);

                j = 0;
                k = 0;

                if (this.TextBox3.Text=="M2" || this.TextBox3.Text=="M3")
                {
                    dbtime=20;
                }
                else
                {
                    dbtime=30;
                }

            /*    for (j = 0; j < dtsql.Tables[0].Rows.Count; j++)
                {
                  if (dtsql.Tables[0].Rows[j][1].ToString() == dt.Tables[0].Rows[i][1].ToString())
                  { 
                    if (dtsql.Tables[0].Rows[j][4].ToString() == "D"  &&  (Convert.ToDouble(dtsql.Tables[0].Rows[j][2])>dbtime ))
                    {
                        k = k + 1;
                    }
                    if (dtsql.Tables[0].Rows[j][4].ToString() == "D" && (Convert.ToDouble(dtsql.Tables[0].Rows[j][2])< dbtime) && j>0)
                    {
                        dbdifftime = Convert.ToDouble(dtsql.Tables[0].Rows[j][2]);
                        if (dtsql.Tables[0].Rows[j-1][4].ToString() == "W" && (dbdifftime+Convert.ToDouble(dtsql.Tables[0].Rows[j-1][2]))>dbtime)
                        {
                            k = k + 1;
                        }
                    }
                }
                }
             */

         DataRow[] dtmtbf = dtsql.Tables[0].Select("(eqid='" + dt.Tables[0].Rows[i][1].ToString() + "')");
         if (dtmtbf.Length > 0)
         {
             //double SMTBF = 0, HMTTR = 0;
             for (int t = 0; t < Convert.ToInt32(dtmtbf.Length); t++)
             {
                 if ((dtmtbf[t].ItemArray[4].ToString() == "W" || dtmtbf[t].ItemArray[4].ToString() == "D") && Convert.ToDouble(dtmtbf[t].ItemArray[2]) > dbtime)
                 {
                     k = k + 1;
                 }
                 if (t > 0 && dtmtbf[t].ItemArray[4].ToString() == "D" && dtmtbf[t - 1].ItemArray[4].ToString() == "W" && Convert.ToDouble(dtmtbf[t].ItemArray[2]) < dbtime && Convert.ToDouble(dtmtbf[t - 1].ItemArray[2]) < dbtime)
                 {
                     if ((Convert.ToDouble(dtmtbf[t].ItemArray[2]) + Convert.ToDouble(dtmtbf[t - 1].ItemArray[2])) > dbtime)
                     {
                         k = k + 1;
                     }
                 }

             }
         }
         else
         {
             drm1["MTTR"] = "0";
         }

         MTBFTotleTime = MTBFTotleTime + Math.Round((Convert.ToDouble(dt.Tables[0].Rows[i][2]) + Convert.ToDouble(dt.Tables[0].Rows[i][3])) / 60, 2); //R+I時間累加
         MTBFTotleK = MTBFTotleK + k; //MTBF次數累加

                if (k > 0)
                {
                    drm1["MTBF"] = Math.Round((Convert.ToDouble(dt.Tables[0].Rows[i][2]) + Convert.ToDouble(dt.Tables[0].Rows[i][3]))/ (Convert.ToInt32(k) * 60), 2);
                }
                else
                {
                    //drm1["MTBF"] = TTime.ToString();
                    drm1["MTBF"] = Math.Round((Convert.ToDouble(dt.Tables[0].Rows[i][2]) + Convert.ToDouble(dt.Tables[0].Rows[i][3])) /  60, 2);
                }


       /*         DataRow[] dtmtbf = dtsql.Tables[0].Select("(eqid='" + dt.Tables[0].Rows[i][1].ToString() + "' and DOWNTime>0.5  )");
				if(dtmtbf.Length>0)
				{
					double SMTBF=0,HMTBF=0; 
					for(int t=0;t<Convert.ToInt32(dtmtbf.Length);t++)
					{
						HMTBF=0;
						SMTBF=SMTBF+Convert.ToDouble(dtmtbf[t].ItemArray[2]);
						HMTBF=Convert.ToDouble(dtmtbf[t].ItemArray[3]);
					}
					drm1["MTBF"]=Math.Round((HMTBF-SMTBF)/Convert.ToInt32(dtmtbf.Length),2);
				}
				else
				{
					drm1["MTBF"]=TTime.ToString();
				}
        */
				dtm1.Rows.Add(drm1);
				if(i==dt.Tables[0].Rows.Count-1)	/*黨算到最後一筆的時候   同時計算Total的資料*/
				{
					DataRow drm2=dtm1.NewRow();
					drm2.Table.Columns.Add();
					drm2["EQLine"]="Total";
					drm2["RunTime"]=Srun;
                    drm2["IdelTime"] = Sidel;

                    drm2["ENGTime"] = SEng;
                    drm2["OtherTime"] = SOther;
                    drm2["PartialDownTime"] = SPartial;

					drm2["DownTime"]=Sdown;
					drm2["SetupTime"]=Ssetup;
					drm2["WaitTime"]=Swait;
					drm2["PMTime"]=Spm;
                    drm2["NonOperationTime"] = Snprd;
					if((Srun+Sidel+Sdown+Ssetup+Swait+Spm)!=0)
					{
                        drm2["Availability"] = Math.Round((Srun + Sidel + Ssetup) / (Srun + Sidel + Sdown + Ssetup + Swait + Spm + SPartial) * 100, 2) + "%";
					}
					else
					{
						drm2["Availability"]="100%";
					}
					if((Srun+Sidel+Ssetup)!=0)
					{
						drm2["Utilization"]=Math.Round((Srun)/(Srun+Sidel+Ssetup)*100,2)+"%";
						drm2["SetupRate"]=Math.Round((Ssetup)/(Srun+Sidel+Ssetup)*100,2)+"%";
					}
					else
					{
						drm2["Utilization"]="100%";
						drm2["SetupRate"]="100%";
					}
                    drm2["MTTR"] = Math.Round(MTTRTotleTime / MTTRTotleK,2);
					drm2["MTBF"]=Math.Round(MTBFTotleTime / MTBFTotleK,2);
					dtm1.Rows.Add(drm2);
				}
			}
            //this.DataGrid1.DataSource=dtm1;
            //this.DataGrid1.DataBind();

            this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
            localReportRoqcs.ReportPath = @"RDLC\EQSMSummary.rdlc";


            ReportDataSource rsd = new ReportDataSource("EQSMSummary", dtm1);
            localReportRoqcs.DataSources.Clear();
            localReportRoqcs.DataSources.Add(rsd);
         
		}
        //private void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer= false;
        //    Response.Charset="UTF-8";
        //    Response.ContentType="Content-Language;content=UTF-8";
        //    Response.AppendHeader("Content-Disposition","attachment;filename=addNewWorker.xls");
        //    Response.ContentEncoding=System.Text.Encoding.UTF7;  
        //    Response.ContentType = "application/ms-excel";   
        //    this.EnableViewState = false;
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        //    this.ReportViewer.RenderControl(oHtmlTextWriter);
        //    Response.Output.Write(oStringWriter.ToString());
        //    Response.End();
        //}
        //public override void VerifyRenderingInServerForm(Control control)   
        //{
        //    return;   //強迫執行匯出excel
        //}
        //private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        //{
        //    e.Item.Attributes.Add("onmouseover","currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
        //    e.Item.Attributes.Add("onmouseout","this.style.backgroundColor=currencol");
        //}
		public void Button1_Click(object sender, System.EventArgs e)
		{
			BindDataToDataGrid();
		}
	}
}
