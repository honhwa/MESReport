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

namespace WebReport.Reports
{
    public partial class Graph2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 20080328
            if (Request["rpt"] != null && Request["rpt"] != "")
            {
                string date = string.Empty;
                string sl = string.Empty;
                string eq = string.Empty;

                if (Request["date"] != null && Request["date"] != "")
                {
                    date = Request["date"];
                }
                else
                {
                    date = "";
                }

                if (Request["sl"] != null && Request["sl"] != "")
                {
                    sl = Request["sl"];
                }
                else
                {
                    sl = "";
                }

                if (Request["eq"] != null && Request["eq"] != "")
                {
                    eq = Request["eq"];
                }
                else
                {
                    eq = "";
                }

                if (Request["rpt"] == "3")
                {
                    if (Session["G2"] != null)
                    {
                        this.reportViewer5.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport5 = reportViewer5.LocalReport;
                        localReport5.ReportPath = @"RDLC\Graph2.rdlc";

                        ReportDataSource rds5 = new ReportDataSource();
                        rds5.Name = "Graph2";
                        DataTable d = (DataTable)Session["G2"];
                        DataRow[] drs = d.Select("TRANSDATE='" + date + "' and SUBLINEID = '" + sl + "' and EQID = '" + eq + "'");
                        DataTable dt5 = d.Clone();
                        foreach (DataRow dr in drs)
                        {
                            //dt4.Rows.Add(dr);
                            DataRow row = dt5.NewRow();
                            row["TRANSDATE"] = dr["TRANSDATE"].ToString();
                            row["WORKCTR"] = dr["WORKCTR"].ToString();
                            row["LINEID"] = dr["LINEID"].ToString();
                            row["SUBLINEID"] = dr["SUBLINEID"].ToString();
                            row["EQID"] = dr["EQID"].ToString();
                            row["ACC"] = dr["ACC"].ToString();
                            row["ENG"] = dr["ENG"].ToString();
                            row["FAC"] = dr["FAC"].ToString();
                            row["IQC"] = dr["IQC"].ToString();
                            row["MFG"] = dr["MFG"].ToString();
                            row["OTHER"] = dr["OTHER"].ToString();
                            row["PC"] = dr["PC"].ToString();
                            row["PE"] = dr["PE"].ToString();
                            row["PUR"] = dr["PUR"].ToString();
                            row["QRA"] = dr["QRA"].ToString();
                            row["ALLTIME"] = dr["ALLTIME"].ToString();

                            dt5.Rows.Add(row);
                        }
                        rds5.Value = dt5;

                        localReport5.DataSources.Add(rds5);
                    }
                }
                else if (Request["rpt"] == "2")
                {
                    //
                }
            }
            #endregion
        }
    }
}
