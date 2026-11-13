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
using System.Text;
using System.Data.SqlClient;

namespace Report.Reports.PC
{
    public partial class ActualOutputMark : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    this.Response.Write("<script language='javascript'>window.alert('請登錄!');</script>");
                    Response.Redirect("../../Login.aspx");
                    return;
                }
                txtColID.Text = "";
                txtFabID.Text = "";
                txtMFGDate.Text = "";
                txtProdLine.Text = "";
                txtProductID.Text = "";

                txtFabID.Text = this.Request.QueryString["FABID"].ToString().Trim();
                txtMFGDate.Text = this.Request.QueryString["MFGDate"].ToString().Trim();
                txtProdLine.Text = this.Request.QueryString["PRODLINE"].ToString().Trim();
                txtProductID.Text = this.Request.QueryString["PRODUCTID"].ToString().Trim();
                txtColID.Text = this.Request.QueryString["ColID"].ToString().Trim();
            }
            if (Session["UserInfo"] == null || ((UserInfo)Session["UserInfo"]).FabId == "")
            {
                Response.Write("No FabId!");
                Response.Redirect("../../Login.aspx");
                return;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string strSql = string.Empty;

                if (TextBox1.Text.Trim()=="")
                {
                    this.Response.Write("<script language='javascript'>window.alert('請輸入備註說明內容!');</script>");
                    return;
                }
                RecordLog Log = new RecordLog();
                if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
                {
                    this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
                }
                StringBuilder sqlMark = new StringBuilder();
                sqlMark.Append(" SELECT  FABID FROM ROPRD ");
                sqlMark.Append(" WHERE FABID = '" + txtFabID.Text + "' and MFGDATE = '" + txtMFGDate.Text + "' and PRODLINE = '" + txtProdLine.Text + "' and PRODUCTID = '" + txtProductID.Text + "' ");
                DataTable dtMark = this.dbConn.GetDataSetBySql(sqlMark.ToString()).Tables[0];
                if (dtMark.Rows.Count == 0)
                {
                    strSql = " INSERT INTO ROPRD(DataEnd_Time,FabID,Shop,Year,Month,WeekNum,MFGDate,ProdLine,PRODUCTID,MTD_Rmk,WTD_Rmk,  ";
                    strSql = strSql + " First_2D_Rmk,Prev_1D_Rmk,DTH_Rmk,H8_Rmk,H9_Rmk,H10_Rmk,H11_Rmk,H12_Rmk,H13_Rmk ";
                    strSql = strSql+" ,H14_Rmk,H15_Rmk,H16_Rmk,H17_Rmk,H18_Rmk,H19_Rmk,H20_Rmk,H21_Rmk,H22_Rmk,H23_Rmk ";
                    strSql = strSql+" ,H24_Rmk,H1_Rmk,H2_Rmk,H3_Rmk,H4_Rmk,H5_Rmk,H6_Rmk,H7_Rmk,Create_Time,Create_By) ";
                    strSql = strSql + " VALUES(GETDATE(), '" + txtFabID.Text + "','MODULE',DATEPART(YEAR,'" + txtMFGDate.Text + "'), ";
                    strSql = strSql + " DATEPART(MONTH,'" + txtMFGDate.Text + "'),DATEPART(WEEK,'" + txtMFGDate.Text + "'),'" + txtMFGDate.Text + "',";
                    strSql = strSql + " '" + txtProdLine.Text + "','" + txtProductID.Text + "',";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '4' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '5' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '6' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '7' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '8' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '9' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '10' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '11' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '12' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '13' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '14' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '15' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '16' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '17' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '18' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '19' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '20' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '21' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '22' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '23' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '24' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '25' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '26' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '27' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '28' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '29' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '30' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '31' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " CASE WHEN '" + txtColID.Text + "'= '32' THEN '" + TextBox1.Text + "' ELSE '' END,";
                    strSql = strSql + " GETDATE(),'" + ((UserInfo)Session["UserInfo"]).UserID.ToString() + "') ";
                }
                else
                {
                    strSql = " UPDATE ROPRD SET ";
                    switch (txtColID.Text)
                    {
                        case "4":
                            strSql = strSql + "MTD_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "5":
                            strSql = strSql + "WTD_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "6":
                            strSql = strSql + "First_2D_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "7":
                            strSql = strSql + "Prev_1D_Rmk ='" + TextBox1.Text + "'";
                            break;
                        case "8":
                            strSql = strSql + "DTH_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "9":
                            strSql = strSql + "H8_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "10":
                            strSql = strSql + "H9_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "11":
                            strSql = strSql + "H10_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "12":
                            strSql = strSql + "H11_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "13":
                            strSql = strSql + "H12_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "14":
                            strSql = strSql + "H13_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "15":
                            strSql = strSql + "H14_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "16":
                            strSql = strSql + "H15_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "17":
                            strSql = strSql + "H16_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "18":
                            strSql = strSql + "H17_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "19":
                            strSql = strSql + "H18_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "20":
                            strSql = strSql + "H19_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "21":
                            strSql = strSql + "H20_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "22":
                            strSql = strSql + "H21_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "23":
                            strSql = strSql + "H22_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "24":
                            strSql = strSql + "H23_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "25":
                            strSql = strSql + "H24_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "26":
                            strSql = strSql + "H1_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "27":
                            strSql = strSql + "H2_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "28":
                            strSql = strSql + "H3_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "29":
                            strSql = strSql + "H4_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "30":
                            strSql = strSql + "H5_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "31":
                            strSql = strSql + "H6_Rmk='" + TextBox1.Text + "'";
                            break;
                        case "32":
                            strSql = strSql + "H7_Rmk='" + TextBox1.Text + "'";
                            break;
                    }
                    strSql = strSql + ",Modify_Time=GETDATE(),Modify_by = '" + ((UserInfo)Session["UserInfo"]).UserID.ToString() + "' ";
                    strSql = strSql + " WHERE FABID = '" + txtFabID.Text + "' AND MFGDATE = '" + txtMFGDate.Text + "' AND PRODLINE = '" + txtProdLine.Text + "'  and PRODUCTID = '" + txtProductID.Text + "' ";
                }
                if (this.dbConn.ExecuteSql(strSql) == true)
                {
                    this.Response.Write("<script language='javascript'>window.close('作業成功!');</script>");
                    return;
                }
                else
                {
                    this.Response.Write("<script language='javascript'>window.alert('作業失敗!');</script>");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
