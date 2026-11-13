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
using System.Data.SqlClient;
using System.Web.Hosting;

//-------------------------------------------------------------------------------------------------------------------------------
//Last Modify Date    Author          Version     Description
//--------------------    -------------   -----------  ----------------------------------------------------------------------------
//  2012/08/01         TPO/Li.Miao    1.00         由之前的GridView變更為ReportViewer呈現數據
//-------------------------------------------------------------------------------------------------------------------------------


//namespace Report.Reports.ACC        1.00 
namespace WebReport.Reports.ACC
{
    public partial class WIPCheck : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../Login.aspx");
                }
                this.txtStartDT.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void Seac_but_Click(object sender, EventArgs e)
        {
            if (this.txtStartDT.Text == string.Empty)
            {
                this.Response.Write("<script language=javascript>window.alert('請選擇開始日期！');</script>");
                return;
            }
            Label2.Text = "";
            string strOutput1="",strOutput2="",strOutput3="";
            try
            {
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                SqlConnection myConn = new SqlConnection(dbConn.ConnectionString);
                SqlParameter ParaIn1=new SqlParameter("@Z_FABID",SqlDbType.Char,1);
                ParaIn1.Direction=ParameterDirection.Input;
                ParaIn1.Value = ((UserInfo)Session["UserInfo"]).FabId;

                SqlParameter ParaIn2 = new SqlParameter("@Z_DateTime", SqlDbType.Char, 10);
                ParaIn2.Direction=ParameterDirection.Input;
                ParaIn2.Value = this.txtStartDT.Text;

                SqlParameter ParaIn3 = new SqlParameter("@Z_QueryFLG", SqlDbType.Char, 1);
                ParaIn3.Direction=ParameterDirection.Input;
                ParaIn3.Value = this.DDLCSHOPCD.SelectedValue;

                SqlParameter ParaOut1=new SqlParameter("@Z_SQLCODE",SqlDbType.Int);
                ParaOut1.Direction=ParameterDirection.Output;

                SqlParameter ParaOut2=new SqlParameter("@Z_SQLERRM",SqlDbType.Char,300);
                ParaOut2.Direction=ParameterDirection.Output;

                SqlParameter ParaOut3=new SqlParameter("@Z_DBERROR",SqlDbType.Int);
                ParaOut3.Direction=ParameterDirection.Output;

                SqlParameter ParaReturn = new SqlParameter("@ReturnValue","");
                ParaReturn.Direction = ParameterDirection.ReturnValue;

                SqlCommand myda = new SqlCommand("USP_RPT_Query_EIFWIP_CHECK", myConn);
                myda.CommandType = CommandType.StoredProcedure;
                myda.Parameters.Add(ParaIn1);
                myda.Parameters.Add(ParaIn2);
                myda.Parameters.Add(ParaIn3);
                myda.Parameters.Add(ParaOut1);
                myda.Parameters.Add(ParaOut2);
                myda.Parameters.Add(ParaOut3);
                myConn.Open();
                myda.ExecuteNonQuery();
                strOutput1 = myda.Parameters["@Z_SQLCODE"].Value.ToString();
                strOutput2 = myda.Parameters["@Z_SQLERRM"].Value.ToString();
                strOutput3= myda.Parameters["@Z_DBERROR"].Value.ToString();

                //1.00 Begin By Li.Miao 
                DataSet myds = new DataSet();
                SqlDataAdapter myDap = new SqlDataAdapter(myda);
                myDap.Fill(myds, "TEMP01");
                myConn.Close();

                //this.GridView1.DataSource = myds;
                //this.GridView1.DataBind();

                this.ReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                LocalReport localReportRoqcs = this.ReportViewer.LocalReport;
                localReportRoqcs.ReportPath = @"RDLC\WIPCheck.rdlc";

                ReportDataSource rsd = new ReportDataSource("WIPCheck",myds.Tables[0]);
                localReportRoqcs.DataSources.Clear();
                localReportRoqcs.DataSources.Add(rsd);
                //1.00 End
                Label2.Text = strOutput2;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;  
        }
        //1.00 Begin By Li.Miao Mark
        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer = false;
        //    Response.Charset = "UTF-8";
        //    Response.ContentType = "Content-Language;content=UTF-8";
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=addNewWorker.xls");
        //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        //    Response.ContentType = "application/ms-excel";
        //    this.EnableViewState = false;
        //    System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        //    this.GridView1.RenderControl(oHtmlTextWriter);
        //    Response.Output.Write(oStringWriter.ToString());
        //    Response.Flush();
        //    Response.End();
        //}
        //1.00 End

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            //this.Response.Redirect("~/Help/會計WIP盤點報表.html");
            this.Response.Write("<script language=javascript>window.open('../../Description/會計WIP盤點報表.html');</script>");
        }
    }
}
