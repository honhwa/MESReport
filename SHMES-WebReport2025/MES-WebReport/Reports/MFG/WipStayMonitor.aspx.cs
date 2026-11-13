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
using Dundas.Charting.WebControl;
using Dundas.Charting;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using Report.Library;
using Report.Sources;

namespace Report.Reports.MFG
{
    public partial class WipStayMonitor : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BoundData();
            }
        }

        protected void BoundData()
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            StringBuilder stb = new StringBuilder();
            stb.Append("SELECT (A.SUBLINEID+'   '+A.PRODID) PRODID,A.PRODID PRODID1,B.PLAN_QTY,SUM(A.ENTITYTQTY)QTY1,CASE WHEN (B.PLAN_QTY-SUM(A.ENTITYTQTY))>0 THEN B.PLAN_QTY-SUM(A.ENTITYTQTY) ELSE  0 END AS QTY2  FROM WLTM A ");
            stb.Append(" INNER JOIN MWPM B ");
            stb.Append(" ON A.FABID=B.FABID ");
            stb.Append(" AND A.PRODID=B.PRODID ");
            stb.Append(" WHERE A.FABID='B' ");
            stb.Append(" AND A.CWORKCTR='8179'");
            stb.Append(" AND ENTITYSTS='R'");
            stb.Append(" AND ISNULL(A.SUBLINEID,'')!=''");
            stb.Append(" GROUP BY A.PRODID,A.SUBLINEID,B.PLAN_QTY ");
            stb.Append(" ORDER BY A.SUBLINEID DESC");

            DataTable dt = this.dbConn.GetDataSetBySql(stb.ToString()).Tables[0];
            Chart1.Series.Clear();

            Chart1.Series.Add("Series1");
            Chart1.Series.Add("Series2");

            Chart1.ChartAreas["Default"].AxisX.Title = "Product ID and Subline";
            Chart1.ChartAreas["Default"].AxisY.Title = "Percentage";
            Chart1.ChartAreas["Default"].AxisX.Interval = 1;
            Chart1.ChartAreas["Default"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["Default"].AxisY.MajorGrid.Enabled = false; 
            Chart1.ChartAreas["Default"].AxisX.Margin = false;
            Chart1.ChartAreas["Default"].AxisY.Minimum = 0;
            Chart1.ChartAreas["Default"].AxisY.StartFromZero = true;
            Chart1.ChartAreas["Default"].AxisY.Maximum = 100;

            Chart1.ChartAreas["Default"].AxisY.LabelStyle.Format = "##,##0.##" + "%";

            Chart1.Series["Series1"].XValueType = ChartValueTypes.String;
            Chart1.Series["Series2"].XValueType = ChartValueTypes.String;

            Chart1.Series["Series1"].Type = SeriesChartType.StackedBar100;
            Chart1.Series["Series2"].Type = SeriesChartType.StackedBar100;

            Chart1.Series["Series1"]["StackedGroupName"] = "Group1";
            Chart1.Series["Series2"]["StackedGroupName"] = "Group1";

            Chart1.Series["Series1"].Color = Color.Red;
            Chart1.Series["Series2"].Color = Color.FromArgb(255, 230, 230);

            Chart1.DataSource = dt;

            Chart1.Series["Series1"].Label = "#VALY";
            Chart1.Series["Series2"].Label = "#VALY";
            Chart1.Series["Series1"].AxisLabel = "#VALX" + "#VALY";

            Chart1.Series["Series1"].ValueMemberX = "PRODID";
            Chart1.Series["Series1"].ValueMembersY = "QTY1";
            Chart1.Series["Series2"].ValueMemberX = "PRODID";
            Chart1.Series["Series2"].ValueMembersY = "QTY2";
            //Chart1.Series["Series3"].ValueMemberX = "PLAN_QTY";
            //Chart1.Series["Series3"].ValueMembersY = "PRODID1";
            //Chart1.Series["Series3"].Color = Color.Transparent;

            Chart1.Series["Series1"].ToolTip = "Current Qty :" + "#VALY";
            Chart1.Series["Series2"].ToolTip = "Still Can Be Used :" + "#VALY";

            Chart1.Series["Series1"].Name = " Current Qty";
            Chart1.Series["Series2"].Name = " Still Can Be Used";

            Chart1.DataBind();
        }
    }
}
