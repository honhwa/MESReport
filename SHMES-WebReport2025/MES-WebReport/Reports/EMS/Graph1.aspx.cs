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
    public partial class Graph1 : System.Web.UI.Page
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

                if (Request["rpt"] == "2")
                {
                    if (Session["G1"] != null)
                    {
                        this.reportViewer4.ProcessingMode = ProcessingMode.Local;
                        LocalReport localReport4 = reportViewer4.LocalReport;
                        localReport4.ReportPath = @"RDLC\Graph1.rdlc";

                        ReportDataSource rds4 = new ReportDataSource();
                        rds4.Name = "Graph1";
                        DataTable d = (DataTable)Session["G1"];
                        DataRow[] drs = d.Select("TRANSDATE='" + date + "' and SUBLINEID = '" + sl + "' and EQID = '" + eq + "'");
                        DataTable dt4 = d.Clone();
                        foreach (DataRow dr in drs)
                        {
                            //dt4.Rows.Add(dr);
                            DataRow row = dt4.NewRow();
                            row["TRANSDATE"] = dr["TRANSDATE"].ToString();
                            row["WORKCTR"] = dr["WORKCTR"].ToString();
                            row["LINEID"] = dr["LINEID"].ToString();
                            row["SUBLINEID"] = dr["SUBLINEID"].ToString();
                            row["EQID"] = dr["EQID"].ToString();
                            row["OPTIME"] = dr["OPTIME"].ToString();
                            row["NONOPTIME"] = dr["NONOPTIME"].ToString();
                            row["NOPDUPTIME"] = dr["NOPDUPTIME"].ToString();
                            row["PDUPTIME"] = dr["PDUPTIME"].ToString();
                            row["PDOPTIME"] = dr["PDOPTIME"].ToString();
                            row["NOPDNONAVAILTIME"] = dr["NOPDNONAVAILTIME"].ToString();
                            row["PMTIME"] = dr["PMTIME"].ToString();
                            row["DOWNTIME"] = dr["DOWNTIME"].ToString();
                            row["RUNTIME"] = dr["RUNTIME"].ToString();
                            row["IDLETIME"] = dr["IDLETIME"].ToString();
                            row["WAITTIME"] = dr["WAITTIME"].ToString();
                            row["SETUPTIME"] = dr["SETUPTIME"].ToString();
                            row["ENGTIME"] = dr["ENGTIME"].ToString();
                            row["OTHERTIME"] = dr["OTHERTIME"].ToString();
                            row["ALLTIME"] = dr["ALLTIME"].ToString();

                            dt4.Rows.Add(row);
                        }
                        rds4.Value = dt4;

                        localReport4.DataSources.Add(rds4);
                    }
                }
                else if (Request["rpt"] == "3")
                {
                    //if (Session["G2"] != null)
                    //{
                    //    this.reportViewer4.ProcessingMode = ProcessingMode.Local;
                    //    LocalReport localReport5 = this.reportViewer4.LocalReport;
                    //    localReport5.ReportPath = @"RDLC\Graph2.rdlc";

                    //    ReportDataSource rds5 = new ReportDataSource();
                    //    rds5.Name = "Graph2";
                    //    DataTable d = (DataTable)Session["G2"];
                    //    DataRow[] drs = d.Select("TRANSDATE='" + date + "' and SUBLINEID = '" + sl + "' and EQID = '" + eq + "'");
                    //    DataTable dt5 = d.Clone();
                    //    foreach (DataRow dr in drs)
                    //    {
                    //        //dt4.Rows.Add(dr);
                    //        DataRow row = dt5.NewRow();
                    //        row["TRANSDATE"] = dr["TRANSDATE"].ToString();
                    //        row["WORKCTR"] = dr["WORKCTR"].ToString();
                    //        row["LINEID"] = dr["LINEID"].ToString();
                    //        row["SUBLINEID"] = dr["SUBLINEID"].ToString();
                    //        row["EQID"] = dr["EQID"].ToString();
                    //        row["ACC"] = dr["ACC"].ToString();
                    //        row["ENG"] = dr["ENG"].ToString();
                    //        row["FAC"] = dr["FAC"].ToString();
                    //        row["IQC"] = dr["IQC"].ToString();
                    //        row["MFG"] = dr["MFG"].ToString();
                    //        row["OTHER"] = dr["OTHER"].ToString();
                    //        row["PC"] = dr["PC"].ToString();
                    //        row["PE"] = dr["PE"].ToString();
                    //        row["PUR"] = dr["PUR"].ToString();
                    //        row["QRA"] = dr["QRA"].ToString();
                    //        row["ALLTIME"] = dr["ALLTIME"].ToString();

                    //        dt5.Rows.Add(row);
                    //    }
                    //    rds5.Value = dt5;

                    //    localReport5.DataSources.Add(rds5);
                    //}
                }
            }
            #endregion
        }
    }
}
