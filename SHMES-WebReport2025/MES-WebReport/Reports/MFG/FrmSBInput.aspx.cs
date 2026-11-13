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
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using Report.Library;

namespace WebReport.Reports
{
	/// <summary>
	/// FrmSBInput 的摘要描述。
	/// </summary>
	public partial class FrmSBInput : System.Web.UI.Page
	{
        DBConn dbConn = new DBConn();
        private string verb = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在這裡放置使用者程式碼以初始化網頁
                if (!this.IsPostBack)
                {
                    if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                    {
                      Response.Redirect("../../Login.aspx");
                    }
                    FrmSBInputQuery wf6;
                    wf6 = (FrmSBInputQuery)Context.Handler;
                    this.TextBox1.Text = wf6.FrTime;
                    this.TextBox2.Text = wf6.ToTime;
                    this.TextBox3.Text = wf6.Prodid;
                    this.TextBox4.Text = wf6.Wonbr;
                    this.TextBox5.Text = wf6.Customer;
                    this.TextBox6.Text = wf6.Type;

                    if (TextBox6.Text.Trim() == "CRLT")
                    {
                        verb = "CRLT";
                        Label3.Text = "Start Bank Input Query Result";
                    }
                    if (TextBox6.Text.Trim() == "FILT")
                    {
                        verb = "FILT";
                        Label3.Text = "BankIn Query Result";
                    }
                     BindDataToDataGrid();

                }
                    if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
                    {
                       Response.Write("No FabId!");
                        Response.Redirect("../../Login.aspx");
                    }
		}

		#region Web Form 設計工具產生的程式碼
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 此為 ASP.NET Web Form 設計工具所需的呼叫。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 此為設計工具支援所必須的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.ImageButton2.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButton2_Click);
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion

		private void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Clear();
			Response.Buffer= false;
			Response.Charset="UTF-8";
			Response.ContentType="Content-Language;content=UTF-8";
			Response.AppendHeader("Content-Disposition","attachment;filename=addNewWorker.xls");
			Response.ContentEncoding=System.Text.Encoding.GetEncoding("UTF-8");   
			Response.ContentType = "application/ms-excel";   
			this.EnableViewState = false;
			System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
			this.DataGrid1.RenderControl(oHtmlTextWriter);
			Response.Output.Write(oStringWriter.ToString());
			Response.End();
		}
		public override void VerifyRenderingInServerForm(Control control)   
		{
			return;   //強迫執行匯出excel
		}  

		private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			e.Item.Attributes.Add("onmouseover","currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
			e.Item.Attributes.Add("onmouseout","this.style.backgroundColor=currencol");
			if(e.Item.ItemType==ListItemType.AlternatingItem)
			{
				e.Item.BackColor=Color.Gainsboro;
			}
			//e.Item.Attributes.Add("ondblclick","window.open('CrystalReport/Cry_SBInput.aspx?id="+e.Item.Cells[1].Text+"&frdate="+this.TextBox1.Text+"&todate="+this.TextBox2.Text+"')");
		}
	    private void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex =e.NewPageIndex ;
			this.BindDataToDataGrid();
		}
        private void BindDataToDataGrid()
                {
                    StringBuilder sql=new StringBuilder();
                    StringBuilder Lchar=new StringBuilder();
        //			sql.Append(" select frwonbr,toProdid,workctr,tolotGrade,tolotLevel,FRATTVALUE,sum(toentitytqty) as qty from VwltxA where verb='RLLT' ");
        //			sql.Append(" and transdate>=to_date('"+TextBox1.Text+"','yyyy/mm/dd hh24:mi:ss')");
        //			sql.Append(" and transdate<=to_date('"+TextBox2.Text+"','yyyy/mm/dd hh24:mi:ss')");
        //			if(TextBox3.Text.Trim()!="")
        //			{
        //				sql.Append(" AND TOPRODID='"+TextBox3.Text+"' ");
        //			}
        //			if(TextBox4.Text.Trim()!="")
        //			{
        //				sql.Append(" AND TOWONBR='"+TextBox4.Text+"' ");
        //			}
        //			if(TextBox5.Text.Trim()!="")
        //			{
        //				sql.Append(" AND FRATTVALUE='"+TextBox5.Text+"' ");
        //			}
                    RecordLog Log = new RecordLog();
                    if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                    {
                        this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                    }
                    sql.Append("SELECT A.wonbr,A.prodid,A.workctr,A.lotgrade,A.lotlevel,A.opt1,sum(qty) as qty ");
                    sql.Append("FROM ( ");
                    sql.Append(" SELECT FABID,towonbr as wonbr,toProdid as prodid,cworkctr as workctr,tolotGrade as lotgrade,tolotLevel as lotlevel,customerid as opt1,centitytqty as qty  FROM New_WLTX");
                    sql.Append(" where VERB='"+this.verb+"' ");
                    sql.Append(" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) ");
                    sql.Append(" and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ");
                    sql.Append(" union all ");
                    sql.Append(" SELECT FABID,towonbr as wonbr,toProdid as prodid,cworkctr as workctr,tolotGrade as lotgrade,tolotLevel as lotlevel,customerid as opt1,centitytqty as qty  FROM WLTXH");
                    sql.Append(" where VERB='" + verb + "' ");
                    sql.Append(" and TRANSDATE>=convert(datetime,'" + TextBox1.Text + "',20) ");
                    sql.Append(" and TRANSDATE<=convert(datetime,'" + TextBox2.Text + "',20) ");
                    sql.Append(" ) A ");
                    sql.Append(" WHERE A.FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
                 //   sql.Append(" WHERE A.FABID='A'");
                    if(TextBox3.Text.Trim()!="")
                    {
                        sql.Append(" AND A.PRODID='"+TextBox3.Text+"' ");
                    }
                    if(TextBox4.Text.Trim()!="")
                    {
                        sql.Append(" AND A.WONBR='"+TextBox4.Text+"' ");
                    }
                    if(TextBox5.Text.Trim()!="")
                    {
                        sql.Append(" AND A.opt1='"+TextBox5.Text+"' ");
                    }
                    sql.Append(" and A.qty<>0 group by A.wonbr,A.Prodid,A.workctr,A.lotGrade,A.lotLevel,A.opt1 ");


                    SqlConnection myConn = new SqlConnection(dbConn.ConnectionString);
                    SqlDataAdapter df = new SqlDataAdapter(sql.ToString(), myConn);
                    DataSet dt = new DataSet();
                    df.SelectCommand.CommandTimeout = 0;
                    myConn.Open();
                    df.Fill(dt, "A");
                    myConn.Close();


                    //if(dt.Tables[0].Rows.Count>1000)
                    //{
                    //    this.DataGrid1.AllowPaging=true;
                    //}
                    this.DataGrid1.DataSource=dt;
                    this.DataGrid1.DataBind();
                    this.DataGrid1.Height=20;
                    if(dt.Tables[0].Rows.Count==0)
                    {
                        this.Response.Write("<script language=javascript>window.alert('Sorry, no data under this category！');</script>");
                    }
                     /* switch(this.Session["pbfab"].ToString())
                    {
                        case "5":
                            Lchar.Append(" NJModule(來料加工);");
                            break;
                        case "4":
                            Lchar.Append(" PRE-MODULE(來料加工);");
                            break;
                    }
                    */
                    if(TextBox3.Text.Trim()!=""){Lchar.Append("ProdID:"+TextBox3.Text+";");}

                    if(TextBox4.Text.Trim()!=""){Lchar.Append("Wonbr:"+TextBox4.Text+";");}

                    if(TextBox5.Text.Trim()!=""){Lchar.Append("CustomerID:"+TextBox5.Text+";");}
                    

                    this.Label2.Text=Lchar.ToString()+"Rows:"+dt.Tables[0].Rows.Count.ToString();
    
                }
	}
}
