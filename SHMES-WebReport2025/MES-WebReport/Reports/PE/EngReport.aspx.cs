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
using System.Text;
using Report.Library;
using Report.Reports;
using Report.Sources;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;

namespace Report.Reports.PE
{
    public partial class EngReport : System.Web.UI.Page
    {
        DBConn dbConn = new DBConn();
        SqlConnection sqlConn = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["UserInfo"] == null) || (Session["IsLogin"] == null) || (!Session["IsLogin"].ToString().Equals("Y")))
                {
                    Response.Redirect("../../Login.aspx");
                }
                this.DataGrid1.Visible = false;
                this.DataGrid2.Visible = false;
                this.BindGroup();
                this.BindProdID();
                this.BindLotType();
            }
        }
        private void BindGroup()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";
                //strSql = " SELECT PRODGROUP FROM MPGTM WHERE FABID ='A' ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.dlGroup.DataSource = null;
                    return;
                }
                this.dlGroup.DataSource = dt;
                this.dlGroup.DataTextField = "PRODGROUP";
                this.dlGroup.DataValueField = "PRODGROUP";
                this.dlGroup.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindProdID()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
/*                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' AND PRODGROUP = '" + this.dlGroup.Items[this.dlGroup.SelectedIndex].Text + "' ";*/
                strSql = " SELECT PRODID FROM MPIM WHERE FABID = '" + ((UserInfo)Session["UserInfo"]).FabId + "' ";

                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.DlProdID.DataSource = null;
                    return;
                }
                this.DlProdID.DataSource = dt;
                this.DlProdID.DataTextField = "PRODID";
                this.DlProdID.DataValueField = "PRODID";
                this.DlProdID.DataBind();
                this.DlProdID.Items.Insert(0, "ALL");
                this.DlProdID.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindLotType()
        {
            string strSql = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT LOTTYPE FROM MLTM WHERE FABID='" + ((UserInfo)Session["UserInfo"]).FabId + "'  ";
                dt = this.dbConn.GetDataSetBySql(strSql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    this.cblLotType.DataSource = null;
                    return;
                }
                this.cblLotType.DataSource = dt;
                this.cblLotType.DataTextField = "LOTTYPE";
                this.cblLotType.DataValueField = "LOTTYPE";
                this.cblLotType.DataBind();
                this.cblLotType.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindModule()
        {
            string strLotType = "''";
            if (this.cblLotType.Items.Count != 0)
            {
                foreach (ListItem li in this.cblLotType.Items)
                {
                    if (li.Selected)
                    {
                        strLotType = strLotType + "," + "'" + li.Text + "'";
                    }
                }
            }

            StringBuilder sql = new StringBuilder();
            StringBuilder Lchar = new StringBuilder();

            sql.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sql.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sql.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sql.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sql.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sql.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sql.Append(" FROM yield_tbl_lighton_product A  ");
            sql.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");

            sql.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sql.Append(" And A.operation_id in('8180','8280','8380','8200','8210','8350') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sql.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sql.Append(" AND A.lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sql.Append(" and A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sql.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");
            //sql.Append(" ORDER BY M.PRODUCT_ID,M.OPERATION_ID ASC ");

            DataSet dt = new DataSet();
            SqlDataAdapter sqlApt = new SqlDataAdapter(sql.ToString(), sqlConn);
            sqlApt.Fill(dt);

            if (dt.Tables[0].Rows.Count == 0)
            {
                this.lblErrMsg.Text = "No Data";
            }
            //------------------------------------------------------------------
            DataTable dtcopy = new DataTable();
            dtcopy.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("target", typeof(string)));

            double Sum = 0, SumA = 0;
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                Sum = 0;
                SumA = 0;
                DataRow drcopy = dtcopy.NewRow();
                drcopy.Table.Columns.Add();

                Sum = Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString());
                SumA = Convert.ToDouble(dt.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][12].ToString());
                drcopy["app_type"] = dt.Tables[0].Rows[i][1].ToString();
                drcopy["product_id"] = dt.Tables[0].Rows[i][2].ToString();
                drcopy["operation_id"] = dt.Tables[0].Rows[i][3].ToString();
                drcopy["t_cnt"] = Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["A"] = Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) / Sum;
                drcopy["R"] = Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString()) / Sum;
                drcopy["A+R"] = (Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString())) / Sum;
                drcopy["S"] = Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["A_Level"] = SumA / Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["target"] = dt.Tables[0].Rows[i][23].ToString();
                dtcopy.Rows.Add(drcopy);
            }


            //--ProdID List----------------------------------------------------------------
            StringBuilder sqlProd = new StringBuilder();

            sqlProd.Append(" SELECT distinct substring(a.product_id,1,15)as product_id ");
            sqlProd.Append(" FROM yield_tbl_lighton_product A ");
            sqlProd.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlProd.Append(" And A.operation_id in('8180','8280','8380','8200','8210','8350') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlProd.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlProd.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlProd.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }

            DataSet dtprod = new DataSet();
            SqlDataAdapter sqlAptProd = new SqlDataAdapter(sqlProd.ToString(), sqlConn);
            sqlAptProd.Fill(dtprod);

            //--Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefe = new StringBuilder();

            sqlDefe.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefe.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefe.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");

            sqlDefe.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefe.Append(" And A.operation_id in('8180','8280','8380','8200','8210','8350','8201','8281','8283') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefe.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefe.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefe.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefe.Append("   and a.lot_grade in ('R','S') ");
            sqlDefe.Append("   and a.rework_cnt = 0 ");
            sqlDefe.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefe.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefe = new DataSet();
            SqlDataAdapter sqlAptdefe = new SqlDataAdapter(sqlDefe.ToString(), sqlConn);
            sqlAptdefe.Fill(dtdefe);

            DataTable dtdef = new DataTable();
            dtdef.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdef.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdef.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdef.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefe.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");

                if (def0.Length > 0)
                {
                    int ReLon1 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                   
                    
                    
                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }

                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }

                        DataRow drdef = dtdef.NewRow();
                        drdef.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdef["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdef["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdef["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdef["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        dtdef.Rows.Add(drdef);
                    }
                }

            }

            //-------M1 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm1 = new StringBuilder();

            sqlm1.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm1.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm1.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm1.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm1.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm1.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm1.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm1.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm1.Append(" And A.operation_id in('8180') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm1.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm1.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm1.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm1 = new DataSet();
            SqlDataAdapter sqlAptm1 = new SqlDataAdapter(sqlm1.ToString(), sqlConn);
            sqlAptm1.Fill(dtm1);

            //------------------------------------------------------------------
            DataTable dtcopym1 = new DataTable();
            dtcopym1.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ1 = 0, SumAm1 = 0;
            for (int i = 0; i < dtm1.Tables[0].Rows.Count; i++)
            {
                Summ1 = 0;
                SumAm1 = 0;
                DataRow drcopym1 = dtcopym1.NewRow();
                drcopym1.Table.Columns.Add();

                Summ1 = Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][8].ToString());
                SumAm1 = Convert.ToDouble(dtm1.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][12].ToString());
                drcopym1["app_type"] = dtm1.Tables[0].Rows[i][1].ToString();
                drcopym1["product_id"] = dtm1.Tables[0].Rows[i][2].ToString();
                drcopym1["operation_id"] = dtm1.Tables[0].Rows[i][3].ToString();
                drcopym1["t_cnt"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["A"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) / Summ1;
                drcopym1["R"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString()) / Summ1;
                drcopym1["A+R"] = (Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString())) / Summ1;
                drcopym1["S"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["A_Level"] = SumAm1 / Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["target"] = dtm1.Tables[0].Rows[i][23].ToString();
                dtcopym1.Rows.Add(drcopym1);
            }
            //--M1  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem1 = new StringBuilder();
            sqlDefem1.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefem1.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem1.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");

            sqlDefem1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem1.Append(" And A.operation_id in('8180') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem1.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem1.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem1.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem1.Append("   and a.rework_cnt = 0 ");
            sqlDefem1.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem1.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem1 = new DataSet();
            SqlDataAdapter sqlAptdefem1 = new SqlDataAdapter(sqlDefem1.ToString(), sqlConn);
            sqlAptdefem1.Fill(dtdefem1);

            DataTable dtdefm1 = new DataTable();
            dtdefm1.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm1.Columns.Add(new DataColumn("ReLon1", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem1.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0,COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }
                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }

                        DataRow drdefm1 = dtdefm1.NewRow();
                        drdefm1.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm1["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm1["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm1["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm1["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm1["ReLon1"] = ReLon1;
                        dtdefm1.Rows.Add(drdefm1);
                    }
                }

            }

            //-------COG Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlcog = new StringBuilder();

            sqlcog.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlcog.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlcog.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlcog.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlcog.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlcog.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlcog.Append(" FROM yield_tbl_lighton_product A  ");
            sqlcog.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlcog.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlcog.Append(" And A.operation_id in('8200') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlcog.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlcog.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlcog.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlcog.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtcog = new DataSet();
            SqlDataAdapter sqlAptcog = new SqlDataAdapter(sqlcog.ToString(), sqlConn);
            sqlAptcog.Fill(dtcog);

            //------------------------------------------------------------------
            DataTable dtcopycog = new DataTable();
            dtcopycog.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopycog.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopycog.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopycog.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopycog.Columns.Add(new DataColumn("target", typeof(string)));

            double Sumcog = 0, SumAcog = 0;
            for (int i = 0; i < dtcog.Tables[0].Rows.Count; i++)
            {
                Sumcog = 0;
                SumAcog = 0;
                DataRow drcopycog = dtcopycog.NewRow();
                drcopycog.Table.Columns.Add();

                Sumcog = Convert.ToDouble(dtcog.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][8].ToString());
                SumAcog = Convert.ToDouble(dtcog.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][12].ToString());
                drcopycog["app_type"] = dtcog.Tables[0].Rows[i][1].ToString();
                drcopycog["product_id"] = dtcog.Tables[0].Rows[i][2].ToString();
                drcopycog["operation_id"] = dtcog.Tables[0].Rows[i][3].ToString();
                drcopycog["t_cnt"] = Convert.ToDouble(dtcog.Tables[0].Rows[i][4].ToString());
                drcopycog["A"] = Convert.ToDouble(dtcog.Tables[0].Rows[i][5].ToString()) / Summ1;
                drcopycog["R"] = Convert.ToDouble(dtcog.Tables[0].Rows[i][7].ToString()) / Summ1;
                drcopycog["A+R"] = (Convert.ToDouble(dtcog.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtcog.Tables[0].Rows[i][7].ToString())) / Summ1;
                drcopycog["S"] = Convert.ToDouble(dtcog.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtcog.Tables[0].Rows[i][4].ToString());
                drcopycog["A_Level"] = SumAm1 / Convert.ToDouble(dtcog.Tables[0].Rows[i][4].ToString());
                drcopycog["target"] = dtcog.Tables[0].Rows[i][23].ToString();
                dtcopycog.Rows.Add(drcopycog);
            }
            //--COG  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefecog = new StringBuilder();
            sqlDefecog.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefecog.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefecog.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefecog.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");

            sqlDefecog.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefecog.Append(" And A.operation_id in('8200') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefecog.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefecog.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefecog.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefecog.Append("   and a.lot_grade in ('R','S') ");
            sqlDefecog.Append("   and a.rework_cnt = 0 ");
            sqlDefecog.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefecog.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefecog = new DataSet();
            SqlDataAdapter sqlAptdefecog = new SqlDataAdapter(sqlDefecog.ToString(), sqlConn);
            sqlAptdefecog.Fill(dtdefecog);

            DataTable dtdefcog = new DataTable();
            dtdefcog.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefcog.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefcog.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefcog.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefcog.Columns.Add(new DataColumn("COG", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefecog.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0,COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopycog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopycog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopycog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty4 = dtcopycog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }


                        DataRow drdefcog = dtdefcog.NewRow();
                        drdefcog.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefcog["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefcog["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefcog["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefcog["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefcog["COG"] = COG;
                        dtdefcog.Rows.Add(drdefcog);
                    }
                }

            }
            //-------M2 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm2 = new StringBuilder();

            sqlm2.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm2.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm2.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm2.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm2.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm2.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm2.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm2.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm2.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm2.Append(" And A.operation_id in('8280') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm2.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm2.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm2.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm2.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm2 = new DataSet();
            SqlDataAdapter sqlAptm2 = new SqlDataAdapter(sqlm2.ToString(), sqlConn);
            sqlAptm2.Fill(dtm2);

            //------------------------------------------------------------------
            DataTable dtcopym2 = new DataTable();
            dtcopym2.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ2 = 0, SumAm2 = 0;
            for (int i = 0; i < dtm2.Tables[0].Rows.Count; i++)
            {
                Summ2 = 0;
                SumAm2 = 0;
                DataRow drcopym2 = dtcopym2.NewRow();
                drcopym2.Table.Columns.Add();

                Summ2 = Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][8].ToString());
                SumAm2 = Convert.ToDouble(dtm2.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][12].ToString());
                drcopym2["app_type"] = dtm2.Tables[0].Rows[i][1].ToString();
                drcopym2["product_id"] = dtm2.Tables[0].Rows[i][2].ToString();
                drcopym2["operation_id"] = dtm2.Tables[0].Rows[i][3].ToString();
                drcopym2["t_cnt"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["A"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) / Summ2;
                drcopym2["R"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString()) / Summ2;
                drcopym2["A+R"] = (Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString())) / Summ2;
                drcopym2["S"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["A_Level"] = SumAm2 / Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["target"] = dtm2.Tables[0].Rows[i][23].ToString();
                dtcopym2.Rows.Add(drcopym2);
            }
            //--m2  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem2 = new StringBuilder();

            sqlDefem2.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefem2.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem2.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem2.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem2.Append(" And A.operation_id in('8280') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem2.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem2.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem2.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem2.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem2.Append("   and a.rework_cnt = 0 ");
            sqlDefem2.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem2.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem2 = new DataSet();
            SqlDataAdapter sqlAptdefem2 = new SqlDataAdapter(sqlDefem2.ToString(), sqlConn);
            sqlAptdefem2.Fill(dtdefem2);

            DataTable dtdefm2 = new DataTable();
            dtdefm2.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm2.Columns.Add(new DataColumn("Lon2", typeof(string)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem2.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0,COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }

                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }


                        DataRow drdefm2 = dtdefm2.NewRow();
                        drdefm2.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm2["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm2["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm2["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm2["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm2["Lon2"] = Lon2;
                        dtdefm2.Rows.Add(drdefm2);
                    }
                }

            }
            //-------M3 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm3 = new StringBuilder();

            sqlm3.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm3.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm3.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm3.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm3.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm3.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm3.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm3.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm3.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm3.Append(" And A.operation_id in('8380') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm3.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm3.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm3.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm3.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm3 = new DataSet();
            SqlDataAdapter sqlAptm3 = new SqlDataAdapter(sqlm3.ToString(), sqlConn);
            sqlAptm3.Fill(dtm3);
            //------------------------------------------------------------------
            DataTable dtcopym3 = new DataTable();
            dtcopym3.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ3 = 0, SumAm3 = 0;
            for (int i = 0; i < dtm3.Tables[0].Rows.Count; i++)
            {
                Summ3 = 0;
                SumAm3 = 0;
                DataRow drcopym3 = dtcopym3.NewRow();
                drcopym3.Table.Columns.Add();

                Summ3 = Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][8].ToString());
                SumAm3 = Convert.ToDouble(dtm3.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][12].ToString());
                drcopym3["app_type"] = dtm3.Tables[0].Rows[i][1].ToString();
                drcopym3["product_id"] = dtm3.Tables[0].Rows[i][2].ToString();
                drcopym3["operation_id"] = dtm3.Tables[0].Rows[i][3].ToString();
                drcopym3["t_cnt"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["A"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) / Summ3;
                drcopym3["R"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString()) / Summ3;
                drcopym3["A+R"] = (Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString())) / Summ3;
                drcopym3["S"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["A_Level"] = SumAm3 / Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["target"] = dtm3.Tables[0].Rows[i][23].ToString();
                dtcopym3.Rows.Add(drcopym3);
            }
            //--m3  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem3 = new StringBuilder();
            sqlDefem3.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefem3.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem3.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem3.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem3.Append(" And A.operation_id in('8380') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem3.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem3.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem3.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem3.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem3.Append("   and a.rework_cnt = 0 ");
            sqlDefem3.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem3.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem3 = new DataSet();
            SqlDataAdapter sqlAptdefem3 = new SqlDataAdapter(sqlDefem3.ToString(), sqlConn);
            sqlAptdefem3.Fill(dtdefem3);

            DataTable dtdefm3 = new DataTable();
            dtdefm3.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm3.Columns.Add(new DataColumn("Lon3", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem3.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0,COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }


                        DataRow drdefm3 = dtdefm3.NewRow();
                        drdefm3.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm3["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm3["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm3["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm3["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm3["Lon3"] = Lon3;
                        dtdefm3.Rows.Add(drdefm3);
                    }
                }

            }


            //-------M2-COG 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm2cog = new StringBuilder();

            sqlm2cog.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm2cog.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm2cog.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm2cog.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm2cog.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm2cog.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm2cog.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm2cog.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm2cog.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm2cog.Append(" And A.operation_id in('8210') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm2cog.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm2cog.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm2cog.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm2cog.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm2cog = new DataSet();
            SqlDataAdapter sqlAptm2cog = new SqlDataAdapter(sqlm2cog.ToString(), sqlConn);
            sqlAptm2cog.Fill(dtm2cog);
            //------------------------------------------------------------------
            DataTable dtcopym2cog = new DataTable();
            dtcopym2cog.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym2cog.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym2cog.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym2cog.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym2cog.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ2cog = 0, SumAm2cog = 0;
            for (int i = 0; i < dtm2cog.Tables[0].Rows.Count; i++)
            {
                Summ2cog = 0;
                SumAm2cog = 0;
                DataRow drcopym2cog = dtcopym2cog.NewRow();
                drcopym2cog.Table.Columns.Add();

                Summ2cog = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][8].ToString());
                SumAm2cog = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][12].ToString());
                drcopym2cog["app_type"] = dtm2cog.Tables[0].Rows[i][1].ToString();
                drcopym2cog["product_id"] = dtm2cog.Tables[0].Rows[i][2].ToString();
                drcopym2cog["operation_id"] = dtm2cog.Tables[0].Rows[i][3].ToString();
                drcopym2cog["t_cnt"] = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][4].ToString());
                drcopym2cog["A"] = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][5].ToString()) / Summ2cog;
                drcopym2cog["R"] = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][7].ToString()) / Summ2cog;
                drcopym2cog["A+R"] = (Convert.ToDouble(dtm2cog.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2cog.Tables[0].Rows[i][7].ToString())) / Summ2cog;
                drcopym2cog["S"] = Convert.ToDouble(dtm2cog.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm2cog.Tables[0].Rows[i][4].ToString());
                drcopym2cog["A_Level"] = SumAm2cog / Convert.ToDouble(dtm2cog.Tables[0].Rows[i][4].ToString());
                drcopym2cog["target"] = dtm2cog.Tables[0].Rows[i][23].ToString();
                dtcopym2cog.Rows.Add(drcopym2cog);
            }
            //--M2-COG  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem2cog = new StringBuilder();
            sqlDefem2cog.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefem2cog.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefem2cog.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem2cog.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem2cog.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem2cog.Append(" And A.operation_id in('8210') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem2cog.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem2cog.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem2cog.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem2cog.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem2cog.Append("   and a.rework_cnt = 0 ");
            sqlDefem2cog.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem2cog.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem2cog = new DataSet();
            SqlDataAdapter sqlAptdefem2cog = new SqlDataAdapter(sqlDefem2cog.ToString(), sqlConn);
            sqlAptdefem2cog.Fill(dtdefem2cog);

            DataTable dtdefm2cog = new DataTable();
            dtdefm2cog.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm2cog.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm2cog.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm2cog.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm2cog.Columns.Add(new DataColumn("M2COG", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem2cog.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopym2cog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym2cog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym2cog.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }



                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }


                        DataRow drdefm2cog = dtdefm2cog.NewRow();
                        drdefm2cog.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm2cog["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm2cog["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm2cog["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm2cog["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm2cog["M2COG"] = M2COG;
                        dtdefm2cog.Rows.Add(drdefm2cog);
                    }
                }

            }


            //-------M3U1 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm3u1 = new StringBuilder();

            sqlm3u1.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm3u1.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm3u1.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm3u1.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm3u1.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm3u1.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm3u1.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm3u1.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm3u1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm3u1.Append(" And A.operation_id in('8350') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm3u1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm3u1.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm3u1.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm3u1.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm3u1 = new DataSet();
            SqlDataAdapter sqlAptm3u1 = new SqlDataAdapter(sqlm3u1.ToString(), sqlConn);
            sqlAptm3u1.Fill(dtm3u1);
            //------------------------------------------------------------------
            DataTable dtcopym3u1 = new DataTable();
            dtcopym3u1.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym3u1.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym3u1.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym3u1.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym3u1.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ3u1 = 0, SumAm3u1 = 0;
            for (int i = 0; i < dtm3u1.Tables[0].Rows.Count; i++)
            {
                Summ3u1 = 0;
                SumAm3u1 = 0;
                DataRow drcopym3u1 = dtcopym3u1.NewRow();
                drcopym3u1.Table.Columns.Add();

                Summ3u1 = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][8].ToString());
                SumAm3u1 = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][12].ToString());
                drcopym3u1["app_type"] = dtm3u1.Tables[0].Rows[i][1].ToString();
                drcopym3u1["product_id"] = dtm3u1.Tables[0].Rows[i][2].ToString();
                drcopym3u1["operation_id"] = dtm3u1.Tables[0].Rows[i][3].ToString();
                drcopym3u1["t_cnt"] = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][4].ToString());
                drcopym3u1["A"] = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][5].ToString()) / Summ3u1;
                drcopym3u1["R"] = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][7].ToString()) / Summ3u1;
                drcopym3u1["A+R"] = (Convert.ToDouble(dtm3u1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3u1.Tables[0].Rows[i][7].ToString())) / Summ3u1;
                drcopym3u1["S"] = Convert.ToDouble(dtm3u1.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm3u1.Tables[0].Rows[i][4].ToString());
                drcopym3u1["A_Level"] = SumAm3u1 / Convert.ToDouble(dtm3u1.Tables[0].Rows[i][4].ToString());
                drcopym3u1["target"] = dtm3u1.Tables[0].Rows[i][23].ToString();
                dtcopym3u1.Rows.Add(drcopym3u1);
            }
            //--M3U1  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem3u1 = new StringBuilder();
            sqlDefem3u1.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefem3u1.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefem3u1.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem3u1.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem3u1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem3u1.Append(" And A.operation_id in('8350') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem3u1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem3u1.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem3u1.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem3u1.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem3u1.Append("   and a.rework_cnt = 0 ");
            sqlDefem3u1.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem3u1.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem3u1 = new DataSet();
            SqlDataAdapter sqlAptdefem3u1 = new SqlDataAdapter(sqlDefem3u1.ToString(), sqlConn);
            sqlAptdefem3u1.Fill(dtdefem3u1);

            DataTable dtdefm3u1 = new DataTable();
            dtdefm3u1.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm3u1.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm3u1.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm3u1.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm3u1.Columns.Add(new DataColumn("M3U1", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem3u1.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopym3u1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym3u1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym3u1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }


                        DataRow drdefm3u1 = dtdefm3u1.NewRow();
                        drdefm3u1.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm3u1["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm3u1["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm3u1["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm3u1["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm3u1["M3U1"] = M3U1;
                        dtdefm3u1.Rows.Add(drdefm3u1);
                    }
                }

            }

            //----------COG-1 Top1~5----------------------------------
            StringBuilder sqlcog1 = new StringBuilder();

            sqlcog1.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlcog1.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlcog1.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlcog1.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlcog1.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlcog1.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlcog1.Append(" FROM yield_tbl_lighton_product A  ");
            sqlcog1.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlcog1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlcog1.Append(" And A.operation_id in('8201') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlcog1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlcog1.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlcog1.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlcog1.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtcog1 = new DataSet();
            SqlDataAdapter sqlAptcog1 = new SqlDataAdapter(sqlcog1.ToString(), sqlConn);
            sqlAptcog1.Fill(dtcog1);
            //------------------------------------------------------------------
            DataTable dtcopycog1 = new DataTable();
            dtcopycog1.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopycog1.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopycog1.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopycog1.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopycog1.Columns.Add(new DataColumn("target", typeof(string)));

            double Sumcog1 = 0, SumAcog1 = 0;
            for (int i = 0; i < dtcog1.Tables[0].Rows.Count; i++)
            {
                Sumcog1 = 0;
                SumAcog1 = 0;
                DataRow drcopycog1 = dtcopycog1.NewRow();
                drcopycog1.Table.Columns.Add();

                Sumcog1 = Convert.ToDouble(dtcog1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][8].ToString());
                SumAcog1 = Convert.ToDouble(dtcog1.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][12].ToString());
                drcopycog1["app_type"] = dtcog1.Tables[0].Rows[i][1].ToString();
                drcopycog1["product_id"] = dtcog1.Tables[0].Rows[i][2].ToString();
                drcopycog1["operation_id"] = dtcog1.Tables[0].Rows[i][3].ToString();
                drcopycog1["t_cnt"] = Convert.ToDouble(dtcog1.Tables[0].Rows[i][4].ToString());
                drcopycog1["A"] = Convert.ToDouble(dtcog1.Tables[0].Rows[i][5].ToString()) / Sumcog1;
                drcopycog1["R"] = Convert.ToDouble(dtcog1.Tables[0].Rows[i][7].ToString()) / Sumcog1;
                drcopycog1["A+R"] = (Convert.ToDouble(dtcog1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtcog1.Tables[0].Rows[i][7].ToString())) / Sumcog1;
                drcopycog1["S"] = Convert.ToDouble(dtcog1.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtcog1.Tables[0].Rows[i][4].ToString());
                drcopycog1["A_Level"] = SumAcog1 / Convert.ToDouble(dtcog1.Tables[0].Rows[i][4].ToString());
                drcopycog1["target"] = dtcog1.Tables[0].Rows[i][23].ToString();
                dtcopycog1.Rows.Add(drcopycog1);
            }
            //--COG-1  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefecog1 = new StringBuilder();
            sqlDefecog1.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefecog1.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefecog1.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefecog1.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefecog1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefecog1.Append(" And A.operation_id in('8201') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefecog1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefecog1.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefecog1.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefecog1.Append("   and a.lot_grade in ('R','S') ");
            sqlDefecog1.Append("   and a.rework_cnt = 0 ");
            sqlDefecog1.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefecog1.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefecog1 = new DataSet();
            SqlDataAdapter sqlAptdefecog1 = new SqlDataAdapter(sqlDefecog1.ToString(), sqlConn);
            sqlAptdefecog1.Fill(dtdefecog1);

            DataTable dtdefcog1 = new DataTable();
            dtdefcog1.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefcog1.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefcog1.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefcog1.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefcog1.Columns.Add(new DataColumn("COG-1", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem3u1.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopycog1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopycog1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopycog1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }

                        DataRow drdefcog1 = dtdefcog1.NewRow();
                        drdefcog1.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefcog1["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefcog1["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefcog1["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefcog1["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefcog1["COG-1"] = COG - 1;
                        dtdefcog1.Rows.Add(drdefcog1);
                    }
                }

            }


            //----------PCB Top1~5----------------------------------
            StringBuilder sqlpcb = new StringBuilder();

            sqlpcb.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlpcb.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlpcb.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlpcb.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlpcb.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlpcb.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlpcb.Append(" FROM yield_tbl_lighton_product A  ");
            sqlpcb.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlpcb.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlpcb.Append(" And A.operation_id in('8281') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlpcb.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlpcb.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlpcb.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlpcb.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtpcb = new DataSet();
            SqlDataAdapter sqlAptpcb = new SqlDataAdapter(sqlpcb.ToString(), sqlConn);
            sqlAptpcb.Fill(dtpcb);
            //------------------------------------------------------------------
            DataTable dtcopypcb = new DataTable();
            dtcopypcb.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopypcb.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopypcb.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopypcb.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopypcb.Columns.Add(new DataColumn("target", typeof(string)));

            double Sumpcb = 0, SumApcb = 0;
            for (int i = 0; i < dtpcb.Tables[0].Rows.Count; i++)
            {
                Sumpcb = 0;
                SumApcb = 0;
                DataRow drcopypcb = dtcopypcb.NewRow();
                drcopypcb.Table.Columns.Add();

                Sumpcb = Convert.ToDouble(dtpcb.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][8].ToString());
                SumApcb = Convert.ToDouble(dtpcb.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][12].ToString());
                drcopypcb["app_type"] = dtpcb.Tables[0].Rows[i][1].ToString();
                drcopypcb["product_id"] = dtpcb.Tables[0].Rows[i][2].ToString();
                drcopypcb["operation_id"] = dtpcb.Tables[0].Rows[i][3].ToString();
                drcopypcb["t_cnt"] = Convert.ToDouble(dtpcb.Tables[0].Rows[i][4].ToString());
                drcopypcb["A"] = Convert.ToDouble(dtpcb.Tables[0].Rows[i][5].ToString()) / Sumpcb;
                drcopypcb["R"] = Convert.ToDouble(dtpcb.Tables[0].Rows[i][7].ToString()) / Sumpcb;
                drcopypcb["A+R"] = (Convert.ToDouble(dtpcb.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtpcb.Tables[0].Rows[i][7].ToString())) / Sumpcb;
                drcopypcb["S"] = Convert.ToDouble(dtpcb.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtpcb.Tables[0].Rows[i][4].ToString());
                drcopypcb["A_Level"] = SumApcb / Convert.ToDouble(dtpcb.Tables[0].Rows[i][4].ToString());
                drcopypcb["target"] = dtpcb.Tables[0].Rows[i][23].ToString();
                dtcopypcb.Rows.Add(drcopypcb);
            }
            //--PCB  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefepcb = new StringBuilder();
            sqlDefepcb.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefepcb.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefepcb.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefepcb.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefepcb.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefepcb.Append(" And A.operation_id in('8281') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefepcb.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefepcb.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefepcb.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefepcb.Append("   and a.lot_grade in ('R','S') ");
            sqlDefepcb.Append("   and a.rework_cnt = 0 ");
            sqlDefepcb.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefepcb.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefepcb = new DataSet();
            SqlDataAdapter sqlAptdefepcb = new SqlDataAdapter(sqlDefepcb.ToString(), sqlConn);
            sqlAptdefepcb.Fill(dtdefepcb);

            DataTable dtdefpcb = new DataTable();
            dtdefpcb.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefpcb.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefpcb.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefpcb.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefpcb.Columns.Add(new DataColumn("PCB", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefepcb.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopypcb.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopypcb.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopypcb.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }


                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }

                        DataRow drdefpcb = dtdefpcb.NewRow();
                        drdefpcb.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefpcb["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefpcb["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefpcb["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefpcb["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefpcb["PCB"] = PCB;
                        dtdefpcb.Rows.Add(drdefpcb);
                    }
                }

            }


            //----------TAILFPC Top1~5----------------------------------
            StringBuilder sqltailfpc = new StringBuilder();

            sqltailfpc.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqltailfpc.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqltailfpc.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqltailfpc.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqltailfpc.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqltailfpc.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqltailfpc.Append(" FROM yield_tbl_lighton_product A  ");
            sqltailfpc.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqltailfpc.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqltailfpc.Append(" And A.operation_id in('8283') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqltailfpc.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqltailfpc.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqltailfpc.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqltailfpc.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dttailfpc = new DataSet();
            SqlDataAdapter sqlApttailfpc = new SqlDataAdapter(sqltailfpc.ToString(), sqlConn);
            sqlApttailfpc.Fill(dttailfpc);
            //------------------------------------------------------------------
            DataTable dtcopytailfpc = new DataTable();
            dtcopytailfpc.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopytailfpc.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopytailfpc.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopytailfpc.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopytailfpc.Columns.Add(new DataColumn("target", typeof(string)));

            double Sumtailfpc = 0, SumAtailfpc = 0;
            for (int i = 0; i < dttailfpc.Tables[0].Rows.Count; i++)
            {
                Sumtailfpc = 0;
                SumAtailfpc = 0;
                DataRow drcopytailfpc = dtcopytailfpc.NewRow();
                drcopytailfpc.Table.Columns.Add();

                Sumtailfpc = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][8].ToString());
                SumAtailfpc = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][12].ToString());
                drcopytailfpc["app_type"] = dttailfpc.Tables[0].Rows[i][1].ToString();
                drcopytailfpc["product_id"] = dttailfpc.Tables[0].Rows[i][2].ToString();
                drcopytailfpc["operation_id"] = dttailfpc.Tables[0].Rows[i][3].ToString();
                drcopytailfpc["t_cnt"] = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][4].ToString());
                drcopytailfpc["A"] = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][5].ToString()) / Sumtailfpc;
                drcopytailfpc["R"] = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][7].ToString()) / Sumtailfpc;
                drcopytailfpc["A+R"] = (Convert.ToDouble(dttailfpc.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dttailfpc.Tables[0].Rows[i][7].ToString())) / Sumtailfpc;
                drcopytailfpc["S"] = Convert.ToDouble(dttailfpc.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dttailfpc.Tables[0].Rows[i][4].ToString());
                drcopytailfpc["A_Level"] = SumAtailfpc / Convert.ToDouble(dttailfpc.Tables[0].Rows[i][4].ToString());
                drcopytailfpc["target"] = dttailfpc.Tables[0].Rows[i][23].ToString();
                dtcopytailfpc.Rows.Add(drcopytailfpc);
            }
            //--Tail FPC  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefetailfpc = new StringBuilder();
            sqlDefetailfpc.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8180') then a.def_cnt else 0 end) RELON1,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8280') then a.def_cnt else 0 end) LON2,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8380') then a.def_cnt else 0 end) LON3,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8200') then a.def_cnt else 0 end) COG,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8210') then a.def_cnt else 0 end) M2COG,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8350') then a.def_cnt else 0 end) M3U1,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8201') then a.def_cnt else 0 end) 'COG-1',    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8281') then a.def_cnt else 0 end) PCB,    ");
            sqlDefetailfpc.Append("        sum(case when a.operation_id in ('8283') then a.def_cnt else 0 end) TAILFPC,    ");
            sqlDefetailfpc.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefetailfpc.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefetailfpc.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefetailfpc.Append(" And A.operation_id in('8283') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefetailfpc.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefetailfpc.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefetailfpc.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefetailfpc.Append("   and a.lot_grade in ('R','S') ");
            sqlDefetailfpc.Append("   and a.rework_cnt = 0 ");
            sqlDefetailfpc.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefetailfpc.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefetailfpc = new DataSet();
            SqlDataAdapter sqlAptdefetailfpc = new SqlDataAdapter(sqlDefetailfpc.ToString(), sqlConn);
            sqlAptdefetailfpc.Fill(dtdefetailfpc);

            DataTable dtdeftailfpc = new DataTable();
            dtdeftailfpc.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdeftailfpc.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdeftailfpc.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdeftailfpc.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdeftailfpc.Columns.Add(new DataColumn("TailFPC", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefetailfpc.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int ReLon1 = 0, ReLon2 = 0, Lon2 = 0, Lon3 = 0, COG = 0, M2COG = 0, M3U1 = 0, COG1 = 0, PCB = 0, TAILFPC = 0;

                    DataRow[] dtqty1 = dtcopytailfpc.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')");
                    if (dtqty1.Length > 0)
                    { ReLon1 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopytailfpc.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280')");
                    if (dtqty2.Length > 0)
                    { Lon2 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopytailfpc.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')");
                    if (dtqty3.Length > 0)
                    { Lon3 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200')");
                    if (dtqty4.Length > 0)
                    { COG = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')");
                    if (dtqty5.Length > 0)
                    { M2COG = Convert.ToInt32(dtqty5[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')");
                    if (dtqty6.Length > 0)
                    { M3U1 = Convert.ToInt32(dtqty6[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')");
                    if (dtqty7.Length > 0)
                    { COG1 = Convert.ToInt32(dtqty7[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')");
                    if (dtqty8.Length > 0)
                    { PCB = Convert.ToInt32(dtqty8[0].ItemArray[3].ToString()); }

                    DataRow[] dtqty9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')");
                    if (dtqty9.Length > 0)
                    { TAILFPC = Convert.ToInt32(dtqty9[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (ReLon1 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(ReLon1));
                        }
                        if (COG != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(COG));
                        }
                        if (Lon2 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(Lon2));
                        }
                        if (Lon3 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(Lon3));
                        }
                        if (M2COG != 0 && Convert.ToDouble(def0[k].ItemArray[8]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[8]) / Convert.ToDouble(M2COG));
                        }
                        if (M3U1 != 0 && Convert.ToDouble(def0[k].ItemArray[9]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[9]) / Convert.ToDouble(M3U1));
                        }

                        if (COG1 != 0 && Convert.ToDouble(def0[k].ItemArray[10]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[10]) / Convert.ToDouble(COG1));
                        }
                        if (PCB != 0 && Convert.ToDouble(def0[k].ItemArray[11]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[11]) / Convert.ToDouble(PCB));
                        }
                        if (TAILFPC != 0 && Convert.ToDouble(def0[k].ItemArray[12]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[12]) / Convert.ToDouble(TAILFPC));
                        }

                        DataRow drdeftailfpc = dtdeftailfpc.NewRow();
                        drdeftailfpc.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdeftailfpc["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdeftailfpc["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdeftailfpc["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdeftailfpc["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdeftailfpc["TAILFPC"] = TAILFPC;
                        dtdeftailfpc.Rows.Add(drdeftailfpc);
                    }
                }

            }




            /*開始填值*/
            DataTable dtme = new DataTable();
            dtme.Columns.Add(new DataColumn("Group", typeof(string)));
            dtme.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtme.Columns.Add(new DataColumn("Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("PlanYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top10y", typeof(string)));
            /*M1 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M1Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M1Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top10y", typeof(string)));
            /*COG Top1~Top5*/
            dtme.Columns.Add(new DataColumn("COGInput", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop1", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop2", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop3", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop4", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop5", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("COGTop6", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop7", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop8", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop9", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop10", typeof(string)));
            dtme.Columns.Add(new DataColumn("COGTop10y", typeof(string)));
            /*M2 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M2Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M2Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top10y", typeof(string)));
            /*M3 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M3Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M3Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top10y", typeof(string)));
            /*M2COG Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M2COGInput", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M2COGTop6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2COGTop10y", typeof(string)));
            /*M3U1 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M3U1Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M3U1Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3U1Top10y", typeof(string)));

            /*COG-1 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("COG1Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("COG1Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("COG1Top10y", typeof(string)));

            /*PCB Top1~Top5*/
            dtme.Columns.Add(new DataColumn("PCBInput", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop1", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop2", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop3", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop4", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop5", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("PCBTop6", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop7", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop8", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop9", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop10", typeof(string)));
            dtme.Columns.Add(new DataColumn("PCBTop10y", typeof(string)));

            /*TAIL FPC Top1~Top5*/
            dtme.Columns.Add(new DataColumn("TAILFPCInput", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop1", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop2", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop3", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop4", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop5", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("TAILFPCTop6", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop7", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop8", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop9", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop10", typeof(string)));
            dtme.Columns.Add(new DataColumn("TAILFPCTop10y", typeof(string)));



            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                int t_cnt = 0;
                string[] Defe = new string[10];
                double[] Def_QTY = new double[10];

                string[] Defe_M1 = new string[10];
                double[] Def_QTY_M1 = new double[10];

                string[] Defe_COG = new string[10];
                double[] Def_QTY_COG = new double[10];

                string[] Defe_M2 = new string[10];
                double[] Def_QTY_M2 = new double[10];

                string[] Defe_M3 = new string[10];
                double[] Def_QTY_M3 = new double[10];

                string[] Defe_M2COG = new string[10];
                double[] Def_QTY_M2COG = new double[10];

                string[] Defe_M3U1 = new string[10];
                double[] Def_QTY_M3U1 = new double[10];

                string[] Defe_COG1 = new string[10];
                double[] Def_QTY_COG1 = new double[10];

                string[] Defe_PCB = new string[10];
                double[] Def_QTY_PCB = new double[10];

                string[] Defe_TAILFPC = new string[10];
                double[] Def_QTY_TAILFPC = new double[10];


                double A_QTY = 1, A_R_QTY = 1, R_QTY = 1, S_QTY = 1;
                double M1_A_Y = 0, COG_A_Y = 0, M2_A_Y = 0, M3_A_Y = 0, M2COG_A_Y = 0, M3U1_A_Y = 0, COG1_A_Y = 0, PCB_A_Y = 0, TAILFPC_A_Y = 0;

                long M1_Input = 0, COG_Input = 0, M2_Input = 0, M3_Input = 0, M2COG_Input = 0, M3U1_Input = 0, COG1_Input = 0, PCB_Input = 0, TAILFPC_Input = 0;
                //--Defecect
                DataRow[] dr0 = dtdef.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr0.Length > 0)
                {

                    int A = 0;
                    if (dr0.Length < 10)
                    {
                        A = Convert.ToInt32(dr0.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe[j] = null;
                        Def_QTY[j] = 0;
                        if (dr0[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr0[j].ItemArray[2].ToString();
                        }
                        Defe[j] = dr0[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY[j] = Convert.ToDouble(dr0[j].ItemArray[3].ToString());
                    }
                }
                /*M1 Top1~Top5*/
                DataRow[] dr01 = dtdefm1.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr01.Length > 0)
                {

                    int A = 0;
                    if (dr01.Length < 10)
                    {
                        A = Convert.ToInt32(dr01.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M1[j] = null;
                        Def_QTY_M1[j] = 0;
                        if (dr01[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr01[j].ItemArray[2].ToString();
                        }
                        Defe_M1[j] = dr01[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M1[j] = Convert.ToDouble(dr01[j].ItemArray[3].ToString());

                        M1_Input = Convert.ToInt32(dr01[j].ItemArray[4].ToString());
                    }
                }
                /*COG Top1~Top5*/
                DataRow[] dr04 = dtdefcog.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr04.Length > 0)
                {

                    int A = 0;
                    if (dr04.Length < 10)
                    {
                        A = Convert.ToInt32(dr04.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        Defe_COG[j] = null;
                        Def_QTY_COG[j] = 0;
                        DefeDesc = "";
                        if (dr04[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr04[j].ItemArray[2].ToString();
                        }
                        Defe_COG[j] = dr04[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_COG[j] = Convert.ToDouble(dr04[j].ItemArray[3].ToString());

                        COG_Input = Convert.ToInt32(dr04[j].ItemArray[4].ToString());
                    }
                }
                /*M2 Top1~Top5*/
                DataRow[] dr02 = dtdefm2.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr02.Length > 0)
                {

                    int A = 0;
                    if (dr02.Length < 10)
                    {
                        A = Convert.ToInt32(dr02.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        Defe_M2[j] = null;
                        Def_QTY_M2[j] = 0;
                        DefeDesc = "";
                        if (dr02[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr02[j].ItemArray[2].ToString();
                        }
                        Defe_M2[j] = dr02[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M2[j] = Convert.ToDouble(dr02[j].ItemArray[3].ToString());

                        M2_Input = Convert.ToInt32(dr02[j].ItemArray[4].ToString());
                    }
                }
                /*M3 Top1~Top5*/
                DataRow[] dr03 = dtdefm3.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr03.Length > 0)
                {

                    int A = 0;
                    if (dr03.Length < 10)
                    {
                        A = Convert.ToInt32(dr03.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M3[j] = null;
                        Def_QTY_M3[j] = 0;
                        if (dr03[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr03[j].ItemArray[2].ToString();
                        }
                        Defe_M3[j] = dr03[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M3[j] = Convert.ToDouble(dr03[j].ItemArray[3].ToString());

                        M3_Input = Convert.ToInt32(dr03[j].ItemArray[4].ToString());
                    }
                }
                /*M2COG Top1~Top5*/
                DataRow[] dr05 = dtdefm2cog.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr05.Length > 0)
                {

                    int A = 0;
                    if (dr05.Length < 10)
                    {
                        A = Convert.ToInt32(dr05.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M2COG[j] = null;
                        Def_QTY_M2COG[j] = 0;
                        if (dr05[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr05[j].ItemArray[2].ToString();
                        }
                        Defe_M2COG[j] = dr05[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M2COG[j] = Convert.ToDouble(dr05[j].ItemArray[3].ToString());

                        M2COG_Input = Convert.ToInt32(dr05[j].ItemArray[4].ToString());
                    }
                }

                /*COG1 Top1~Top5*/
                DataRow[] dr07 = dtdefcog1.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr07.Length > 0)
                {

                    int A = 0;
                    if (dr07.Length < 10)
                    {
                        A = Convert.ToInt32(dr07.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_COG1[j] = null;
                        Def_QTY_COG1[j] = 0;
                        if (dr07[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr07[j].ItemArray[2].ToString();
                        }
                        Defe_COG1[j] = dr07[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_COG1[j] = Convert.ToDouble(dr07[j].ItemArray[3].ToString());

                        COG1_Input = Convert.ToInt32(dr07[j].ItemArray[4].ToString());
                    }
                }

                /*PCB Top1~Top5*/
                DataRow[] dr08 = dtdefpcb.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr08.Length > 0)
                {

                    int A = 0;
                    if (dr08.Length < 10)
                    {
                        A = Convert.ToInt32(dr08.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_PCB[j] = null;
                        Def_QTY_PCB[j] = 0;
                        if (dr08[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr08[j].ItemArray[2].ToString();
                        }
                        Defe_PCB[j] = dr08[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_PCB[j] = Convert.ToDouble(dr08[j].ItemArray[3].ToString());

                        PCB_Input = Convert.ToInt32(dr08[j].ItemArray[4].ToString());
                    }
                }

                /*TAILFPC Top1~Top5*/
                DataRow[] dr09 = dtdeftailfpc.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr09.Length > 0)
                {
                    int A = 0;
                    if (dr09.Length < 10)
                    {
                        A = Convert.ToInt32(dr09.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_TAILFPC[j] = null;
                        Def_QTY_TAILFPC[j] = 0;
                        if (dr09[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr09[j].ItemArray[2].ToString();
                        }
                        Defe_TAILFPC[j] = dr09[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_TAILFPC[j] = Convert.ToDouble(dr09[j].ItemArray[3].ToString());

                        TAILFPC_Input = Convert.ToInt32(dr09[j].ItemArray[4].ToString());
                    }
                }


                /*M3U1 Top1~Top5*/
                DataRow[] dr06 = dtdefm3u1.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr06.Length > 0)
                {

                    int A = 0;
                    if (dr06.Length < 10)
                    {
                        A = Convert.ToInt32(dr06.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M3U1[j] = null;
                        Def_QTY_M3U1[j] = 0;
                        if (dr06[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr06[j].ItemArray[2].ToString();
                        }
                        Defe_M3U1[j] = dr06[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M3U1[j] = Convert.ToDouble(dr06[j].ItemArray[3].ToString());

                        M3U1_Input = Convert.ToInt32(dr06[j].ItemArray[4].ToString());
                    }
                }

                DataRow[] dr1 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8180')  ");
                if (dr1.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr1[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr1[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr1[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr1[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr1[0].ItemArray[8].ToString());
                    M1_A_Y = 100 * Convert.ToDouble(dr1[0].ItemArray[4].ToString());
                }
                /*
                DataRow[] dr2 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280') ");
                if (dr2.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr2[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr2[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr2[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr2[0].ItemArray[8].ToString());
                    M1_A_Y = 100 * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                }

                DataRow[] dr3 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380') ");
                if (dr3.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr3[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr3[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr3[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr3[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr3[0].ItemArray[8].ToString());
                }
                */
                DataRow[] dr2 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8200') ");
                if (dr2.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr2[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr2[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr2[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr2[0].ItemArray[8].ToString());
                    COG_A_Y = 100 * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                }

                DataRow[] dr7 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8210')  ");
                if (dr7.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr7[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr7[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr7[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr7[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr7[0].ItemArray[8].ToString());
                    M2COG_A_Y = 100 * Convert.ToDouble(dr7[0].ItemArray[4].ToString());

                }

                DataRow[] dr4 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8280') ");
                if (dr4.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr4[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr4[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr4[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr4[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr4[0].ItemArray[8].ToString());
                    M2_A_Y = 100 * Convert.ToDouble(dr4[0].ItemArray[4].ToString());
                }

                DataRow[] dr8 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8350')  ");
                if (dr8.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr8[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr8[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr8[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr8[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr8[0].ItemArray[8].ToString());
                    M3U1_A_Y = 100 * Convert.ToDouble(dr8[0].ItemArray[4].ToString());

                }

                DataRow[] dr5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8380')  ");
                if (dr5.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr5[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr5[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr5[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr5[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr5[0].ItemArray[8].ToString());
                    M3_A_Y = 100 * Convert.ToDouble(dr5[0].ItemArray[4].ToString());

                }

                DataRow[] dr9 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8201')  ");
                if (dr9.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr9[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr9[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr9[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr9[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr9[0].ItemArray[8].ToString());
                    COG1_A_Y = 100 * Convert.ToDouble(dr9[0].ItemArray[4].ToString());

                }

                DataRow[] dr10 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8281')  ");
                if (dr10.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr10[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr10[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr10[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr10[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr10[0].ItemArray[8].ToString());
                    PCB_A_Y = 100 * Convert.ToDouble(dr10[0].ItemArray[4].ToString());

                }

                DataRow[] dr11 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8283')  ");
                if (dr11.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr11[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr11[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr11[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr11[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr11[0].ItemArray[8].ToString());
                    TAILFPC_A_Y = 100 * Convert.ToDouble(dr11[0].ItemArray[4].ToString());

                }



                DataRow[] dr6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')");
                if (dr6.Length > 0)
                {
                    DataRow drme = dtme.NewRow();
                    drme.Table.Columns.Add();

                    drme["Group"] = dr6[0].ItemArray[0].ToString();
                    drme["ProdID"] = dr6[0].ItemArray[1].ToString();
                    drme["Input"] = t_cnt;
                    drme["PlanYield"] = dr6[0].ItemArray[9].ToString();
                    drme["ActualYield"] = Math.Round(A_QTY * 100, 2);

                    drme["Top1"] = Defe[0];
                    drme["Top1y"] = Def_QTY[0];
                    drme["Top2"] = Defe[1];
                    drme["Top2y"] = Def_QTY[1];
                    drme["Top3"] = Defe[2];
                    drme["Top3y"] = Def_QTY[2];
                    drme["Top4"] = Defe[3];
                    drme["Top4y"] = Def_QTY[3];
                    drme["Top5"] = Defe[4];
                    drme["Top5y"] = Def_QTY[4];

                    drme["Top6"] = Defe[5];
                    drme["Top6y"] = Def_QTY[5];
                    drme["Top7"] = Defe[6];
                    drme["Top7y"] = Def_QTY[6];
                    drme["Top8"] = Defe[7];
                    drme["Top8y"] = Def_QTY[7];
                    drme["Top9"] = Defe[8];
                    drme["Top9y"] = Def_QTY[8];
                    drme["Top10"] = Defe[9];
                    drme["Top10y"] = Def_QTY[9];
                    /*M1 Top1~Top5*/
                    drme["M1Input"] = M1_Input;
                    drme["M1ActualYield"] = Math.Round(M1_A_Y, 2);
                    drme["M1Top1"] = Defe_M1[0];
                    drme["M1Top1y"] = Def_QTY_M1[0];
                    drme["M1Top2"] = Defe_M1[1];
                    drme["M1Top2y"] = Def_QTY_M1[1];
                    drme["M1Top3"] = Defe_M1[2];
                    drme["M1Top3y"] = Def_QTY_M1[2];
                    drme["M1Top4"] = Defe_M1[3];
                    drme["M1Top4y"] = Def_QTY_M1[3];
                    drme["M1Top5"] = Defe_M1[4];
                    drme["M1Top5y"] = Def_QTY_M1[4];

                    drme["M1Top6"] = Defe_M1[5];
                    drme["M1Top6y"] = Def_QTY_M1[5];
                    drme["M1Top7"] = Defe_M1[6];
                    drme["M1Top7y"] = Def_QTY_M1[6];
                    drme["M1Top8"] = Defe_M1[7];
                    drme["M1Top8y"] = Def_QTY_M1[7];
                    drme["M1Top9"] = Defe_M1[8];
                    drme["M1Top9y"] = Def_QTY_M1[8];
                    drme["M1Top10"] = Defe_M1[9];
                    drme["M1Top10y"] = Def_QTY_M1[9];
                    /*COG Top1~Top5*/
                    drme["COGInput"] = COG_Input;
                    drme["COGActualYield"] = Math.Round(COG_A_Y, 2);
                    drme["COGTop1"] = Defe_COG[0];
                    drme["COGTop1y"] = Def_QTY_COG[0];
                    drme["COGTop2"] = Defe_COG[1];
                    drme["COGTop2y"] = Def_QTY_COG[1];
                    drme["COGTop3"] = Defe_COG[2];
                    drme["COGTop3y"] = Def_QTY_COG[2];
                    drme["COGTop4"] = Defe_COG[3];
                    drme["COGTop4y"] = Def_QTY_COG[3];
                    drme["COGTop5"] = Defe_COG[4];
                    drme["COGTop5y"] = Def_QTY_COG[4];

                    drme["COGTop6"] = Defe_COG[5];
                    drme["COGTop6y"] = Def_QTY_COG[5];
                    drme["COGTop7"] = Defe_COG[6];
                    drme["COGTop7y"] = Def_QTY_COG[6];
                    drme["COGTop8"] = Defe_COG[7];
                    drme["COGTop8y"] = Def_QTY_COG[7];
                    drme["COGTop9"] = Defe_COG[8];
                    drme["COGTop9y"] = Def_QTY_COG[8];
                    drme["COGTop10"] = Defe_COG[9];
                    drme["COGTop10y"] = Def_QTY_COG[9];
                    /*M2 Top1~Top5*/
                    drme["M2Input"] = M2_Input;
                    drme["M2ActualYield"] = Math.Round(M2_A_Y, 2);
                    drme["M2Top1"] = Defe_M2[0];
                    drme["M2Top1y"] = Def_QTY_M2[0];
                    drme["M2Top2"] = Defe_M2[1];
                    drme["M2Top2y"] = Def_QTY_M2[1];
                    drme["M2Top3"] = Defe_M2[2];
                    drme["M2Top3y"] = Def_QTY_M2[2];
                    drme["M2Top4"] = Defe_M2[3];
                    drme["M2Top4y"] = Def_QTY_M2[3];
                    drme["M2Top5"] = Defe_M2[4];
                    drme["M2Top5y"] = Def_QTY_M2[4];

                    drme["M2Top6"] = Defe_M2[5];
                    drme["M2Top6y"] = Def_QTY_M2[5];
                    drme["M2Top7"] = Defe_M2[6];
                    drme["M2Top7y"] = Def_QTY_M2[6];
                    drme["M2Top8"] = Defe_M2[7];
                    drme["M2Top8y"] = Def_QTY_M2[7];
                    drme["M2Top9"] = Defe_M2[8];
                    drme["M2Top9y"] = Def_QTY_M2[8];
                    drme["M2Top10"] = Defe_M2[9];
                    drme["M2Top10y"] = Def_QTY_M2[9];
                    /*M3 Top1~Top5*/
                    drme["M3Input"] = M3_Input;
                    drme["M3ActualYield"] = Math.Round(M3_A_Y, 2);
                    drme["M3Top1"] = Defe_M3[0];
                    drme["M3Top1y"] = Def_QTY_M3[0];
                    drme["M3Top2"] = Defe_M3[1];
                    drme["M3Top2y"] = Def_QTY_M3[1];
                    drme["M3Top3"] = Defe_M3[2];
                    drme["M3Top3y"] = Def_QTY_M3[2];
                    drme["M3Top4"] = Defe_M3[3];
                    drme["M3Top4y"] = Def_QTY_M3[3];
                    drme["M3Top5"] = Defe_M3[4];
                    drme["M3Top5y"] = Def_QTY_M3[4];

                    drme["M3Top6"] = Defe_M3[5];
                    drme["M3Top6y"] = Def_QTY_M3[5];
                    drme["M3Top7"] = Defe_M3[6];
                    drme["M3Top7y"] = Def_QTY_M3[6];
                    drme["M3Top8"] = Defe_M3[7];
                    drme["M3Top8y"] = Def_QTY_M3[7];
                    drme["M3Top9"] = Defe_M3[8];
                    drme["M3Top9y"] = Def_QTY_M3[8];
                    drme["M3Top10"] = Defe_M3[9];
                    drme["M3Top10y"] = Def_QTY_M3[9];
                    /*M2COG Top1~Top5*/
                    drme["M2COGInput"] = M2COG_Input;
                    drme["M2COGActualYield"] = Math.Round(M2COG_A_Y, 2);
                    drme["M2COGTop1"] = Defe_M2COG[0];
                    drme["M2COGTop1y"] = Def_QTY_M2COG[0];
                    drme["M2COGTop2"] = Defe_M2COG[1];
                    drme["M2COGTop2y"] = Def_QTY_M2COG[1];
                    drme["M2COGTop3"] = Defe_M2COG[2];
                    drme["M2COGTop3y"] = Def_QTY_M2COG[2];
                    drme["M2COGTop4"] = Defe_M2COG[3];
                    drme["M2COGTop4y"] = Def_QTY_M2COG[3];
                    drme["M2COGTop5"] = Defe_M2COG[4];
                    drme["M2COGTop5y"] = Def_QTY_M2COG[4];

                    drme["M2COGTop6"] = Defe_M2COG[5];
                    drme["M2COGTop6y"] = Def_QTY_M2COG[5];
                    drme["M2COGTop7"] = Defe_M2COG[6];
                    drme["M2COGTop7y"] = Def_QTY_M2COG[6];
                    drme["M2COGTop8"] = Defe_M2COG[7];
                    drme["M2COGTop8y"] = Def_QTY_M2COG[7];
                    drme["M2COGTop9"] = Defe_M2COG[8];
                    drme["M2COGTop9y"] = Def_QTY_M2COG[8];
                    drme["M2COGTop10"] = Defe_M2COG[9];
                    drme["M2COGTop10y"] = Def_QTY_M2COG[9];
                    /*M3U1 Top1~Top5*/
                    drme["M3U1Input"] = M3U1_Input;
                    drme["M3U1ActualYield"] = Math.Round(M3U1_A_Y, 2);
                    drme["M3U1Top1"] = Defe_M3U1[0];
                    drme["M3U1Top1y"] = Def_QTY_M3U1[0];
                    drme["M3U1Top2"] = Defe_M3U1[1];
                    drme["M3U1Top2y"] = Def_QTY_M3U1[1];
                    drme["M3U1Top3"] = Defe_M3U1[2];
                    drme["M3U1Top3y"] = Def_QTY_M3U1[2];
                    drme["M3U1Top4"] = Defe_M3U1[3];
                    drme["M3U1Top4y"] = Def_QTY_M3U1[3];
                    drme["M3U1Top5"] = Defe_M3U1[4];
                    drme["M3U1Top5y"] = Def_QTY_M3U1[4];

                    drme["M3U1Top6"] = Defe_M3U1[5];
                    drme["M3U1Top6y"] = Def_QTY_M3U1[5];
                    drme["M3U1Top7"] = Defe_M3U1[6];
                    drme["M3U1Top7y"] = Def_QTY_M3U1[6];
                    drme["M3U1Top8"] = Defe_M3U1[7];
                    drme["M3U1Top8y"] = Def_QTY_M3U1[7];
                    drme["M3U1Top9"] = Defe_M3U1[8];
                    drme["M3U1Top9y"] = Def_QTY_M3U1[8];
                    drme["M3U1Top10"] = Defe_M3U1[9];
                    drme["M3U1Top10y"] = Def_QTY_M3U1[9];

                    //*COG-1 Top1~Top5*/
                    drme["COG1Input"] = COG1_Input;
                    drme["COG1ActualYield"] = Math.Round(COG1_A_Y, 2);
                    drme["COG1Top1"] = Defe_COG1[0];
                    drme["COG1Top1y"] = Def_QTY_COG1[0];
                    drme["COG1Top2"] = Defe_COG1[1];
                    drme["COG1Top2y"] = Def_QTY_COG1[1];
                    drme["COG1Top3"] = Defe_COG1[2];
                    drme["COG1Top3y"] = Def_QTY_COG1[2];
                    drme["COG1Top4"] = Defe_COG1[3];
                    drme["COG1Top4y"] = Def_QTY_COG1[3];
                    drme["COG1Top5"] = Defe_COG1[4];
                    drme["COG1Top5y"] = Def_QTY_COG1[4];

                    drme["COG1Top6"] = Defe_COG1[5];
                    drme["COG1Top6y"] = Def_QTY_COG1[5];
                    drme["COG1Top7"] = Defe_COG1[6];
                    drme["COG1Top7y"] = Def_QTY_COG1[6];
                    drme["COG1Top8"] = Defe_COG1[7];
                    drme["COG1Top8y"] = Def_QTY_COG1[7];
                    drme["COG1Top9"] = Defe_COG1[8];
                    drme["COG1Top9y"] = Def_QTY_COG1[8];
                    drme["COG1Top10"] = Defe_COG1[9];
                    drme["COG1Top10y"] = Def_QTY_COG1[9];

                    /*PCB Top1~Top5*/
                    drme["PCBInput"] = PCB_Input;
                    drme["PCBActualYield"] = Math.Round(PCB_A_Y, 2);
                    drme["PCBTop1"] = Defe_PCB[0];
                    drme["PCBTop1y"] = Def_QTY_PCB[0];
                    drme["PCBTop2"] = Defe_PCB[1];
                    drme["PCBTop2y"] = Def_QTY_PCB[1];
                    drme["PCBTop3"] = Defe_PCB[2];
                    drme["PCBTop3y"] = Def_QTY_PCB[2];
                    drme["PCBTop4"] = Defe_PCB[3];
                    drme["PCBTop4y"] = Def_QTY_PCB[3];
                    drme["PCBTop5"] = Defe_PCB[4];
                    drme["PCBTop5y"] = Def_QTY_PCB[4];

                    drme["PCBTop6"] = Defe_PCB[5];
                    drme["PCBTop6y"] = Def_QTY_PCB[5];
                    drme["PCBTop7"] = Defe_PCB[6];
                    drme["PCBTop7y"] = Def_QTY_PCB[6];
                    drme["PCBTop8"] = Defe_PCB[7];
                    drme["PCBTop8y"] = Def_QTY_PCB[7];
                    drme["PCBTop9"] = Defe_PCB[8];
                    drme["PCBTop9y"] = Def_QTY_PCB[8];
                    drme["PCBTop10"] = Defe_PCB[9];
                    drme["PCBTop10y"] = Def_QTY_PCB[9];

                    /*TAILFPC Top1~Top5*/
                    drme["TAILFPCInput"] = TAILFPC_Input;
                    drme["TAILFPCActualYield"] = Math.Round(TAILFPC_A_Y, 2);
                    drme["TAILFPCTop1"] = Defe_TAILFPC[0];
                    drme["TAILFPCTop1y"] = Def_QTY_TAILFPC[0];
                    drme["TAILFPCTop2"] = Defe_TAILFPC[1];
                    drme["TAILFPCTop2y"] = Def_QTY_TAILFPC[1];
                    drme["TAILFPCTop3"] = Defe_TAILFPC[2];
                    drme["TAILFPCTop3y"] = Def_QTY_TAILFPC[2];
                    drme["TAILFPCTop4"] = Defe_TAILFPC[3];
                    drme["TAILFPCTop4y"] = Def_QTY_TAILFPC[3];
                    drme["TAILFPCTop5"] = Defe_TAILFPC[4];
                    drme["TAILFPCTop5y"] = Def_QTY_TAILFPC[4];

                    drme["TAILFPCTop6"] = Defe_TAILFPC[5];
                    drme["TAILFPCTop6y"] = Def_QTY_TAILFPC[5];
                    drme["TAILFPCTop7"] = Defe_TAILFPC[6];
                    drme["TAILFPCTop7y"] = Def_QTY_TAILFPC[6];
                    drme["TAILFPCTop8"] = Defe_TAILFPC[7];
                    drme["TAILFPCTop8y"] = Def_QTY_TAILFPC[7];
                    drme["TAILFPCTop9"] = Defe_TAILFPC[8];
                    drme["TAILFPCTop9y"] = Def_QTY_TAILFPC[8];
                    drme["TAILFPCTop10"] = Defe_TAILFPC[9];
                    drme["TAILFPCTop10y"] = Def_QTY_TAILFPC[9];


                    dtme.Rows.Add(drme);
                }
                dtme.DefaultView.Sort = "Group,ProdID asc";
            }
            this.DataGrid1.DataSource = dtme;
            this.DataGrid1.DataBind();
        }
        private void BindCell2()
        {
            string strLotType = "''";
            if (this.cblLotType.Items.Count != 0)
            {
                foreach (ListItem li in this.cblLotType.Items)
                {
                    if (li.Selected)
                    {
                        strLotType = strLotType + "," + "'" + li.Text + "'";
                    }
                }
            }

            StringBuilder sql = new StringBuilder();
            StringBuilder Lchar = new StringBuilder();

            sql.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sql.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sql.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sql.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sql.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sql.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sql.Append(" FROM yield_tbl_lighton_product A  ");
            sql.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");

            sql.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sql.Append(" And A.operation_id in('8040','8050','8080','8090') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sql.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sql.Append(" AND A.lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sql.Append(" and A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sql.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dt = new DataSet();
            SqlDataAdapter sqlApt = new SqlDataAdapter(sql.ToString(), sqlConn);
            sqlApt.Fill(dt);

            if (dt.Tables[0].Rows.Count == 0)
            {
                this.lblErrMsg.Text = "No Data";
            }
            //------------------------------------------------------------------
            DataTable dtcopy = new DataTable();
            dtcopy.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopy.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopy.Columns.Add(new DataColumn("target", typeof(string)));

            double Sum = 0, SumA = 0;
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                Sum = 0;
                SumA = 0;
                DataRow drcopy = dtcopy.NewRow();
                drcopy.Table.Columns.Add();

                Sum = Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString());
                SumA = Convert.ToDouble(dt.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][12].ToString());
                drcopy["app_type"] = dt.Tables[0].Rows[i][1].ToString();
                drcopy["product_id"] = dt.Tables[0].Rows[i][2].ToString();
                drcopy["operation_id"] = dt.Tables[0].Rows[i][3].ToString();
                drcopy["t_cnt"] = Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["A"] = Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) / Sum;
                drcopy["R"] = Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString()) / Sum;
                drcopy["A+R"] = (Convert.ToDouble(dt.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dt.Tables[0].Rows[i][7].ToString())) / Sum;
                drcopy["S"] = Convert.ToDouble(dt.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["A_Level"] = SumA / Convert.ToDouble(dt.Tables[0].Rows[i][4].ToString());
                drcopy["target"] = dt.Tables[0].Rows[i][23].ToString();
                dtcopy.Rows.Add(drcopy);
            }


            //--ProdID List----------------------------------------------------------------
            StringBuilder sqlProd = new StringBuilder();

            sqlProd.Append(" SELECT distinct substring(a.product_id,1,15)as product_id ");
            sqlProd.Append(" FROM yield_tbl_lighton_product A ");
            sqlProd.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlProd.Append(" And A.operation_id in('8040','8050','8080','8090') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlProd.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlProd.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlProd.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }

            DataSet dtprod = new DataSet();
            SqlDataAdapter sqlAptProd = new SqlDataAdapter(sqlProd.ToString(), sqlConn);
            sqlAptProd.Fill(dtprod);

            //--Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefe = new StringBuilder();

            sqlDefe.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8040') then a.def_cnt else 0 end) R8180,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8050') then a.def_cnt else 0 end) R8050,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8080') then a.def_cnt else 0 end) R8080,    ");
            sqlDefe.Append("        sum(case when a.operation_id in ('8090') then a.def_cnt else 0 end) R8090,    ");
            sqlDefe.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefe.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");

            sqlDefe.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefe.Append(" And A.operation_id in('8040','8050','8080','8090') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefe.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefe.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefe.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefe.Append("   and a.lot_grade in ('R','S') ");
            sqlDefe.Append("   and a.rework_cnt = 0 ");
            sqlDefe.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefe.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefe = new DataSet();
            SqlDataAdapter sqlAptdefe = new SqlDataAdapter(sqlDefe.ToString(), sqlConn);
            sqlAptdefe.Fill(dtdefe);

            DataTable dtdef = new DataTable();
            dtdef.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdef.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdef.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdef.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefe.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");

                if (def0.Length > 0)
                {
                    int R8040 = 0, R8050 = 0, R8080 = 0, R8090 = 0;

                    DataRow[] dtqty1 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')");
                    if (dtqty1.Length > 0)
                    { R8040 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050')");
                    if (dtqty2.Length > 0)
                    { R8050 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080')");
                    if (dtqty3.Length > 0)
                    { R8080 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')");
                    if (dtqty4.Length > 0)
                    { R8090 = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (R8040 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(R8040));
                        }
                        if (R8050 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(R8050));
                        }
                        if (R8080 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8080));
                        }
                        if (R8090 != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(R8090));
                        }

                        DataRow drdef = dtdef.NewRow();
                        drdef.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdef["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdef["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdef["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdef["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        dtdef.Rows.Add(drdef);
                    }
                }

            }

            //-------8040 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm1 = new StringBuilder();

            sqlm1.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm1.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm1.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm1.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm1.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm1.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm1.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm1.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm1.Append(" And A.operation_id in('8040') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm1.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm1.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm1.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm1 = new DataSet();
            SqlDataAdapter sqlAptm1 = new SqlDataAdapter(sqlm1.ToString(), sqlConn);
            sqlAptm1.Fill(dtm1);

            //------------------------------------------------------------------
            DataTable dtcopym1 = new DataTable();
            dtcopym1.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym1.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym1.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ1 = 0, SumAm1 = 0;
            for (int i = 0; i < dtm1.Tables[0].Rows.Count; i++)
            {
                Summ1 = 0;
                SumAm1 = 0;
                DataRow drcopym1 = dtcopym1.NewRow();
                drcopym1.Table.Columns.Add();

                Summ1 = Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][8].ToString());
                SumAm1 = Convert.ToDouble(dtm1.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][12].ToString());
                drcopym1["app_type"] = dtm1.Tables[0].Rows[i][1].ToString();
                drcopym1["product_id"] = dtm1.Tables[0].Rows[i][2].ToString();
                drcopym1["operation_id"] = dtm1.Tables[0].Rows[i][3].ToString();
                drcopym1["t_cnt"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["A"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) / Summ1;
                drcopym1["R"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString()) / Summ1;
                drcopym1["A+R"] = (Convert.ToDouble(dtm1.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm1.Tables[0].Rows[i][7].ToString())) / Summ1;
                drcopym1["S"] = Convert.ToDouble(dtm1.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["A_Level"] = SumAm1 / Convert.ToDouble(dtm1.Tables[0].Rows[i][4].ToString());
                drcopym1["target"] = dtm1.Tables[0].Rows[i][23].ToString();
                dtcopym1.Rows.Add(drcopym1);
            }
            //--M1  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem1 = new StringBuilder();
            sqlDefem1.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8040') then a.def_cnt else 0 end) R8040,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8050') then a.def_cnt else 0 end) R8050,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8080') then a.def_cnt else 0 end) R8080,    ");
            sqlDefem1.Append("        sum(case when a.operation_id in ('8090') then a.def_cnt else 0 end) R8090,    ");
            sqlDefem1.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem1.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");

            sqlDefem1.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem1.Append(" And A.operation_id in('8040') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem1.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem1.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem1.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem1.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem1.Append("   and a.rework_cnt = 0 ");
            sqlDefem1.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem1.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem1 = new DataSet();
            SqlDataAdapter sqlAptdefem1 = new SqlDataAdapter(sqlDefem1.ToString(), sqlConn);
            sqlAptdefem1.Fill(dtdefem1);

            DataTable dtdefm1 = new DataTable();
            dtdefm1.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm1.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm1.Columns.Add(new DataColumn("ReLon1", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem1.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int R8040 = 0, R8050 = 0, R8080 = 0, R8090 = 0;

                    DataRow[] dtqty1 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')");
                    if (dtqty1.Length > 0)
                    { R8040 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050')");
                    if (dtqty2.Length > 0)
                    { R8050 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080')");
                    if (dtqty3.Length > 0)
                    { R8080 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym1.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')");
                    if (dtqty4.Length > 0)
                    { R8090 = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (R8040 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(R8040));
                        }
                        if (R8050 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(R8050));
                        }
                        if (R8080 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8080));
                        }
                        if (R8090 != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(R8090));
                        }

                        DataRow drdefm1 = dtdefm1.NewRow();
                        drdefm1.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm1["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm1["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm1["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm1["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm1["ReLon1"] = R8040;
                        dtdefm1.Rows.Add(drdefm1);
                    }
                }

            }
            //-------8050 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm2 = new StringBuilder();

            sqlm2.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm2.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm2.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm2.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm2.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm2.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm2.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm2.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm2.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm2.Append(" And A.operation_id in('8050') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm2.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm2.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm2.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm2.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm2 = new DataSet();
            SqlDataAdapter sqlAptm2 = new SqlDataAdapter(sqlm2.ToString(), sqlConn);
            sqlAptm2.Fill(dtm2);

            //------------------------------------------------------------------
            DataTable dtcopym2 = new DataTable();
            dtcopym2.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym2.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym2.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ2 = 0, SumAm2 = 0;
            for (int i = 0; i < dtm2.Tables[0].Rows.Count; i++)
            {
                Summ2 = 0;
                SumAm2 = 0;
                DataRow drcopym2 = dtcopym2.NewRow();
                drcopym2.Table.Columns.Add();

                Summ2 = Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][8].ToString());
                SumAm2 = Convert.ToDouble(dtm2.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][12].ToString());
                drcopym2["app_type"] = dtm2.Tables[0].Rows[i][1].ToString();
                drcopym2["product_id"] = dtm2.Tables[0].Rows[i][2].ToString();
                drcopym2["operation_id"] = dtm2.Tables[0].Rows[i][3].ToString();
                drcopym2["t_cnt"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["A"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) / Summ2;
                drcopym2["R"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString()) / Summ2;
                drcopym2["A+R"] = (Convert.ToDouble(dtm2.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm2.Tables[0].Rows[i][7].ToString())) / Summ2;
                drcopym2["S"] = Convert.ToDouble(dtm2.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["A_Level"] = SumAm2 / Convert.ToDouble(dtm2.Tables[0].Rows[i][4].ToString());
                drcopym2["target"] = dtm2.Tables[0].Rows[i][23].ToString();
                dtcopym2.Rows.Add(drcopym2);
            }
            //--8050  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem2 = new StringBuilder();

            sqlDefem2.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8040') then a.def_cnt else 0 end) R8040,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8050') then a.def_cnt else 0 end) R8050,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8080') then a.def_cnt else 0 end) R8080,    ");
            sqlDefem2.Append("        sum(case when a.operation_id in ('8090') then a.def_cnt else 0 end) R8090,    ");
            sqlDefem2.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem2.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem2.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "'");
            sqlDefem2.Append(" And A.operation_id in('8050') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem2.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem2.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem2.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem2.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem2.Append("   and a.rework_cnt = 0 ");
            sqlDefem2.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem2.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem2 = new DataSet();
            SqlDataAdapter sqlAptdefem2 = new SqlDataAdapter(sqlDefem2.ToString(), sqlConn);
            sqlAptdefem2.Fill(dtdefem2);

            DataTable dtdefm2 = new DataTable();
            dtdefm2.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm2.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm2.Columns.Add(new DataColumn("Lon2", typeof(string)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem2.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int R8040 = 0, R8050 = 0, R8080 = 0, R8090 = 0;

                    DataRow[] dtqty1 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')");
                    if (dtqty1.Length > 0)
                    { R8040 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050')");
                    if (dtqty2.Length > 0)
                    { R8050 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080')");
                    if (dtqty3.Length > 0)
                    { R8080 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym2.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')");
                    if (dtqty4.Length > 0)
                    { R8090 = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (R8040 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(R8040));
                        }
                        if (R8050 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(R8050));
                        }
                        if (R8080 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8080));
                        }
                        if (R8090 != 0 && Convert.ToDouble(def0[k].ItemArray[7]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[7]) / Convert.ToDouble(R8090));
                        }

                        DataRow drdefm2 = dtdefm2.NewRow();
                        drdefm2.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm2["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm2["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm2["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm2["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm2["Lon2"] = R8050;
                        dtdefm2.Rows.Add(drdefm2);
                    }
                }

            }
            //-------8080 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm3 = new StringBuilder();

            sqlm3.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm3.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm3.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm3.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm3.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm3.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm3.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm3.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm3.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm3.Append(" And A.operation_id in('8080') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm3.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm3.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm3.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm3.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm3 = new DataSet();
            SqlDataAdapter sqlAptm3 = new SqlDataAdapter(sqlm3.ToString(), sqlConn);
            sqlAptm3.Fill(dtm3);
            //------------------------------------------------------------------
            DataTable dtcopym3 = new DataTable();
            dtcopym3.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym3.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym3.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ3 = 0, SumAm3 = 0;
            for (int i = 0; i < dtm3.Tables[0].Rows.Count; i++)
            {
                Summ3 = 0;
                SumAm3 = 0;
                DataRow drcopym3 = dtcopym3.NewRow();
                drcopym3.Table.Columns.Add();

                Summ3 = Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][8].ToString());
                SumAm3 = Convert.ToDouble(dtm3.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][12].ToString());
                drcopym3["app_type"] = dtm3.Tables[0].Rows[i][1].ToString();
                drcopym3["product_id"] = dtm3.Tables[0].Rows[i][2].ToString();
                drcopym3["operation_id"] = dtm3.Tables[0].Rows[i][3].ToString();
                drcopym3["t_cnt"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["A"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) / Summ3;
                drcopym3["R"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString()) / Summ3;
                drcopym3["A+R"] = (Convert.ToDouble(dtm3.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm3.Tables[0].Rows[i][7].ToString())) / Summ3;
                drcopym3["S"] = Convert.ToDouble(dtm3.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["A_Level"] = SumAm3 / Convert.ToDouble(dtm3.Tables[0].Rows[i][4].ToString());
                drcopym3["target"] = dtm3.Tables[0].Rows[i][23].ToString();
                dtcopym3.Rows.Add(drcopym3);
            }
            //--8080  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem3 = new StringBuilder();
            sqlDefem3.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8040') then a.def_cnt else 0 end) R8040,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8050') then a.def_cnt else 0 end) R8050,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8080') then a.def_cnt else 0 end) R8080,    ");
            sqlDefem3.Append("        sum(case when a.operation_id in ('8090') then a.def_cnt else 0 end) R8090,    ");
            sqlDefem3.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem3.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem3.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem3.Append(" And A.operation_id in('8080') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem3.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem3.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem3.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem3.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem3.Append("   and a.rework_cnt = 0 ");
            sqlDefem3.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem3.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem3 = new DataSet();
            SqlDataAdapter sqlAptdefem3 = new SqlDataAdapter(sqlDefem3.ToString(), sqlConn);
            sqlAptdefem3.Fill(dtdefem3);

            DataTable dtdefm3 = new DataTable();
            dtdefm3.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm3.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm3.Columns.Add(new DataColumn("Lon3", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem3.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int R8040 = 0, R8050 = 0, R8080 = 0, R8090 = 0;

                    DataRow[] dtqty1 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')");
                    if (dtqty1.Length > 0)
                    { R8040 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050')");
                    if (dtqty2.Length > 0)
                    { R8050 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080')");
                    if (dtqty3.Length > 0)
                    { R8080 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym3.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')");
                    if (dtqty4.Length > 0)
                    { R8090 = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (R8040 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(R8040));
                        }
                        if (R8050 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(R8050));
                        }
                        if (R8080 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8080));
                        }
                        if (R8090 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8090));
                        }

                        DataRow drdefm3 = dtdefm3.NewRow();
                        drdefm3.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm3["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm3["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm3["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm3["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm3["Lon3"] = R8080;
                        dtdefm3.Rows.Add(drdefm3);
                    }
                }

            }
            //-------8090 的Top1~Top5-----------------------------------------------------------------------------------
            StringBuilder sqlm4 = new StringBuilder();

            sqlm4.Append(" SELECT A.fab_id,A.product_group,substring(A.product_id,1,15) as product_id,A.operation_id, ");
            sqlm4.Append(" sum(A.t_cnt) as t_cnt,sum(A.a_cnt) as a_cnt,sum(A.b_cnt) as b_cnt,sum(A.r_cnt) as r_cnt,sum(A.s_cnt) as s_cnt, ");
            sqlm4.Append(" sum(A.aa_cnt) as aa_cnt,sum(A.a0_cnt) as a0_cnt,sum(A.a1_cnt) as a1_cnt,sum(A.a2_cnt) as a2_cnt,sum(A.a3_cnt) as a3_cnt, ");
            sqlm4.Append(" sum(A.ap_cnt) as ap_cnt,sum(A.ak_cnt) as ak_cnt,sum(A.ab_cnt) as ab_cnt,sum(A.af_cnt) as af_cnt,sum(A.ah_cnt) as ah_cnt, ");
            sqlm4.Append(" sum(A.al_cnt) as al_cnt,sum(A.ax_cnt) as ax_cnt,sum(A.am_cnt) as am_cnt,sum(A.undefine_cnt) as undefine_cnt, ");
            sqlm4.Append(" case when B.Target is null then 0 else B.Target end as Target ");
            sqlm4.Append(" FROM yield_tbl_lighton_product A  ");
            sqlm4.Append(" LEFT JOIN yield_tbl_nj_target B ON(A.shopcd=B.shop and A.product_group=B.app_type and A.reportmonth=B.month)  ");
            sqlm4.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text.ToString() + "' and '" + this.txtToTime.Text + "' and A.fab_id in ('" + ((UserInfo)Session["UserInfo"]).FabId + "')  ");
            sqlm4.Append(" And A.operation_id in('8090') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlm4.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlm4.Append(" AND lot_type in(" + strLotType.ToString() + ")  ");
            }
            else
            {
                sqlm4.Append(" and lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlm4.Append(" Group by A.fab_id,A.operation_id,substring(A.product_id,1,15),A.product_group,B.Target ");

            DataSet dtm4 = new DataSet();
            SqlDataAdapter sqlAptm4 = new SqlDataAdapter(sqlm4.ToString(), sqlConn);
            sqlAptm4.Fill(dtm4);
            //------------------------------------------------------------------
            DataTable dtcopym4 = new DataTable();
            dtcopym4.Columns.Add(new DataColumn("app_type", typeof(string)));
            dtcopym4.Columns.Add(new DataColumn("product_id", typeof(string)));
            dtcopym4.Columns.Add(new DataColumn("operation_id", typeof(string)));
            dtcopym4.Columns.Add(new DataColumn("t_cnt", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("A", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("R", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("A+R", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("S", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("A_Level", typeof(double)));
            dtcopym4.Columns.Add(new DataColumn("target", typeof(string)));

            double Summ4 = 0, SumAm4 = 0;
            for (int i = 0; i < dtm4.Tables[0].Rows.Count; i++)
            {
                Summ4 = 0;
                SumAm4 = 0;
                DataRow drcopym4 = dtcopym4.NewRow();
                drcopym4.Table.Columns.Add();

                Summ4 = Convert.ToDouble(dtm4.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][6].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][7].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][8].ToString());
                SumAm4 = Convert.ToDouble(dtm4.Tables[0].Rows[i][9].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][10].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][11].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][12].ToString());
                drcopym4["app_type"] = dtm4.Tables[0].Rows[i][1].ToString();
                drcopym4["product_id"] = dtm4.Tables[0].Rows[i][2].ToString();
                drcopym4["operation_id"] = dtm4.Tables[0].Rows[i][3].ToString();
                drcopym4["t_cnt"] = Convert.ToDouble(dtm4.Tables[0].Rows[i][4].ToString());
                drcopym4["A"] = Convert.ToDouble(dtm4.Tables[0].Rows[i][5].ToString()) / Summ3;
                drcopym4["R"] = Convert.ToDouble(dtm4.Tables[0].Rows[i][7].ToString()) / Summ3;
                drcopym4["A+R"] = (Convert.ToDouble(dtm4.Tables[0].Rows[i][5].ToString()) + Convert.ToDouble(dtm4.Tables[0].Rows[i][7].ToString())) / Summ3;
                drcopym4["S"] = Convert.ToDouble(dtm4.Tables[0].Rows[i][8].ToString()) / Convert.ToDouble(dtm4.Tables[0].Rows[i][4].ToString());
                drcopym4["A_Level"] = SumAm3 / Convert.ToDouble(dtm4.Tables[0].Rows[i][4].ToString());
                drcopym4["target"] = dtm4.Tables[0].Rows[i][23].ToString();
                dtcopym4.Rows.Add(drcopym4);
            }
            //--8080  Defecect_cd----------------------------------------------------------------

            StringBuilder sqlDefem4 = new StringBuilder();
            sqlDefem4.Append(" Select distinct substring(a.product_id,1,15) as product_id,a.def_code,b.defectdesc, A.shopcd,  ");
            sqlDefem4.Append("        sum(case when a.operation_id in ('8040') then a.def_cnt else 0 end) R8040,    ");
            sqlDefem4.Append("        sum(case when a.operation_id in ('8050') then a.def_cnt else 0 end) R8050,    ");
            sqlDefem4.Append("        sum(case when a.operation_id in ('8080') then a.def_cnt else 0 end) R8080,    ");
            sqlDefem4.Append("        sum(case when a.operation_id in ('8090') then a.def_cnt else 0 end) R8090,    ");
            sqlDefem4.Append("        sum(a.def_cnt) TTLDef From nj_tbl_lighton_defect a ");
            sqlDefem4.Append(" left join MDCM b on(a.fab_id = b.fabid and a.def_code= b.defectcd) ");
            sqlDefem4.Append(" where 1=1 and A.cdate between '" + this.txtFromTime.Text + "' and '" + this.txtToTime.Text + "' and A.fab_id ='" + ((UserInfo)Session["UserInfo"]).FabId + "' ");
            sqlDefem4.Append(" And A.operation_id in('8090') and a.product_group='" + this.dlGroup.Text.ToString() + "' ");
            if (this.DlProdID.Text.ToString() != "ALL")
            {
                sqlDefem4.Append(" And A.product_id='" + this.DlProdID.Text.ToString() + "' ");
            }
            if (strLotType.ToString() != "''")
            {
                sqlDefem4.Append(" And A.lot_type in(" + strLotType.ToString() + ") ");
            }
            else
            {
                sqlDefem4.Append(" And A.lot_type  in ('EK','ES','PR','RW')  ");
            }
            sqlDefem4.Append("   and a.lot_grade in ('R','S') ");
            sqlDefem4.Append("   and a.rework_cnt = 0 ");
            sqlDefem4.Append(" Group by a.product_id,a.def_code,b.defectdesc,A.shopcd ");
            sqlDefem4.Append(" Order by product_id,sum(a.def_cnt) Desc ");

            DataSet dtdefem4 = new DataSet();
            SqlDataAdapter sqlAptdefem4 = new SqlDataAdapter(sqlDefem4.ToString(), sqlConn);
            sqlAptdefem4.Fill(dtdefem4);

            DataTable dtdefm4 = new DataTable();
            dtdefm4.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtdefm4.Columns.Add(new DataColumn("Def_Code", typeof(string)));
            dtdefm4.Columns.Add(new DataColumn("CodeDesc", typeof(string)));
            dtdefm4.Columns.Add(new DataColumn("Yield_Def", typeof(double)));
            dtdefm4.Columns.Add(new DataColumn("Lon4", typeof(double)));
            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                DataRow[] def0 = dtdefem4.Tables[0].Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "product_id desc,TTLDef desc");


                if (def0.Length > 0)
                {
                    int R8040 = 0, R8050 = 0, R8080 = 0, R8090 = 0;

                    DataRow[] dtqty1 = dtcopym4.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')");
                    if (dtqty1.Length > 0)
                    { R8040 = Convert.ToInt32(dtqty1[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty2 = dtcopym4.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050')");
                    if (dtqty2.Length > 0)
                    { R8050 = Convert.ToInt32(dtqty2[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty3 = dtcopym4.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080')");
                    if (dtqty3.Length > 0)
                    { R8080 = Convert.ToInt32(dtqty3[0].ItemArray[3].ToString()); }
                    DataRow[] dtqty4 = dtcopym4.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')");
                    if (dtqty4.Length > 0)
                    { R8090 = Convert.ToInt32(dtqty4[0].ItemArray[3].ToString()); }

                    for (int k = 0; k < Convert.ToInt32(def0.Length); k++)
                    {
                        double DefRate = 1;
                        if (R8040 != 0 && Convert.ToDouble(def0[k].ItemArray[4]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[4]) / Convert.ToDouble(R8040));
                        }
                        if (R8050 != 0 && Convert.ToDouble(def0[k].ItemArray[5]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[5]) / Convert.ToDouble(R8050));
                        }
                        if (R8080 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8080));
                        }
                        if (R8090 != 0 && Convert.ToDouble(def0[k].ItemArray[6]) != 0)
                        {
                            if (DefRate == 0) { DefRate = 1; }
                            DefRate = DefRate * (1 - Convert.ToDouble(def0[k].ItemArray[6]) / Convert.ToDouble(R8090));
                        }

                        DataRow drdefm4 = dtdefm4.NewRow();
                        drdefm4.Table.Columns.Add();
                        //Math.Round(A_QTY*100,2)
                        drdefm4["ProdID"] = def0[k].ItemArray[0].ToString();
                        drdefm4["Def_Code"] = def0[k].ItemArray[1].ToString();
                        drdefm4["CodeDesc"] = def0[k].ItemArray[2].ToString();
                        drdefm4["Yield_Def"] = Convert.ToDouble(Math.Round(((1 - DefRate)) * 100, 2));
                        drdefm4["Lon4"] = R8090;
                        dtdefm4.Rows.Add(drdefm4);
                    }
                }

            }

            /*開始填值*/
            DataTable dtme = new DataTable();
            dtme.Columns.Add(new DataColumn("Group", typeof(string)));
            dtme.Columns.Add(new DataColumn("ProdID", typeof(string)));
            dtme.Columns.Add(new DataColumn("Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("PlanYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("Top10y", typeof(string)));
            /*8040 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M1Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M1Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M1Top10y", typeof(string)));
            /*8050 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M2Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M2Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M2Top10y", typeof(string)));
            /*8080 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M3Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M3Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M3Top10y", typeof(string)));
            /*8090 Top1~Top5*/
            dtme.Columns.Add(new DataColumn("M4Input", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4ActualYield", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top1", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top1y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top2", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top2y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top3", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top3y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top4", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top4y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top5", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top5y", typeof(string)));

            dtme.Columns.Add(new DataColumn("M4Top6", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top6y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top7", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top7y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top8", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top8y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top9", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top9y", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top10", typeof(string)));
            dtme.Columns.Add(new DataColumn("M4Top10y", typeof(string)));

            for (int i = 0; i < dtprod.Tables[0].Rows.Count; i++)
            {
                int t_cnt = 0;
                string[] Defe = new string[10];
                double[] Def_QTY = new double[10];

                string[] Defe_M1 = new string[10];
                double[] Def_QTY_M1 = new double[10];

                string[] Defe_M2 = new string[10];
                double[] Def_QTY_M2 = new double[10];

                string[] Defe_M3 = new string[10];
                double[] Def_QTY_M3 = new double[10];

                string[] Defe_M4 = new string[10];
                double[] Def_QTY_M4 = new double[10];

                double A_QTY = 1, A_R_QTY = 1, R_QTY = 1, S_QTY = 1;
                double M1_A_Y = 0, M2_A_Y = 0, M3_A_Y = 0, M4_A_Y = 0;

                long M1_Input = 0, M2_Input = 0, M3_Input = 0, M4_Input = 0;
                //--Defecect
                DataRow[] dr0 = dtdef.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr0.Length > 0)
                {

                    int A = 0;
                    if (dr0.Length < 10)
                    {
                        A = Convert.ToInt32(dr0.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe[j] = null;
                        Def_QTY[j] = 0;
                        if (dr0[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr0[j].ItemArray[2].ToString();
                        }
                        Defe[j] = dr0[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY[j] = Convert.ToDouble(dr0[j].ItemArray[3].ToString());
                    }
                }
                /*8040 Top1~Top5*/
                DataRow[] dr01 = dtdefm1.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr01.Length > 0)
                {

                    int A = 0;
                    if (dr01.Length < 10)
                    {
                        A = Convert.ToInt32(dr01.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M1[j] = null;
                        Def_QTY_M1[j] = 0;
                        if (dr01[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr01[j].ItemArray[2].ToString();
                        }
                        Defe_M1[j] = dr01[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M1[j] = Convert.ToDouble(dr01[j].ItemArray[3].ToString());

                        M1_Input = Convert.ToInt32(dr01[j].ItemArray[4].ToString());
                    }
                }
                /*8050 Top1~Top5*/
                DataRow[] dr02 = dtdefm2.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr02.Length > 0)
                {

                    int A = 0;
                    if (dr02.Length < 10)
                    {
                        A = Convert.ToInt32(dr02.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        Defe_M2[j] = null;
                        Def_QTY_M2[j] = 0;
                        DefeDesc = "";
                        if (dr02[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr02[j].ItemArray[2].ToString();
                        }
                        Defe_M2[j] = dr02[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M2[j] = Convert.ToDouble(dr02[j].ItemArray[3].ToString());

                        M2_Input = Convert.ToInt32(dr02[j].ItemArray[4].ToString());
                    }
                }
                /*8080 Top1~Top5*/
                DataRow[] dr03 = dtdefm3.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr03.Length > 0)
                {

                    int A = 0;
                    if (dr03.Length < 10)
                    {
                        A = Convert.ToInt32(dr03.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M3[j] = null;
                        Def_QTY_M3[j] = 0;
                        if (dr03[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr03[j].ItemArray[2].ToString();
                        }
                        Defe_M3[j] = dr03[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M3[j] = Convert.ToDouble(dr03[j].ItemArray[3].ToString());

                        M3_Input = Convert.ToInt32(dr03[j].ItemArray[4].ToString());
                    }
                }
                /*8090 Top1~Top5*/
                DataRow[] dr04 = dtdefm4.Select("(ProdID='" + dtprod.Tables[0].Rows[i][0].ToString() + "')", "Yield_Def desc");
                if (dr04.Length > 0)
                {

                    int A = 0;
                    if (dr04.Length < 10)
                    {
                        A = Convert.ToInt32(dr04.Length);
                    }
                    else
                    {
                        A = 10;
                    }
                    for (int j = 0; j < A; j++)
                    {
                        string DefeDesc;
                        DefeDesc = "";
                        Defe_M4[j] = null;
                        Def_QTY_M4[j] = 0;
                        if (dr04[j].ItemArray[2].ToString() == null)
                        {
                            DefeDesc = " ";
                        }
                        else
                        {
                            DefeDesc = dr04[j].ItemArray[2].ToString();
                        }
                        Defe_M4[j] = dr04[j].ItemArray[1].ToString() + "	" + DefeDesc;
                        Def_QTY_M4[j] = Convert.ToDouble(dr04[j].ItemArray[3].ToString());

                        M4_Input = Convert.ToInt32(dr04[j].ItemArray[4].ToString());
                    }
                }

                DataRow[] dr1 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8040')  ");
                if (dr1.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr1[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr1[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr1[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr1[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr1[0].ItemArray[8].ToString());
                    M1_A_Y = 100 * Convert.ToDouble(dr1[0].ItemArray[4].ToString());
                }
                
                DataRow[] dr2 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8050') ");
                if (dr2.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr2[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr2[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr2[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr2[0].ItemArray[8].ToString());
                    M2_A_Y = 100 * Convert.ToDouble(dr2[0].ItemArray[4].ToString());
                }

                DataRow[] dr4 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8080') ");
                if (dr4.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr4[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr4[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr4[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr4[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr4[0].ItemArray[8].ToString());
                    M3_A_Y = 100 * Convert.ToDouble(dr4[0].ItemArray[4].ToString());
                }

                DataRow[] dr5 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "' and operation_id='8090')  ");
                if (dr5.Length > 0)
                {
                    t_cnt = Convert.ToInt32(dr5[0].ItemArray[3].ToString());
                    A_QTY = A_QTY * Convert.ToDouble(dr5[0].ItemArray[4].ToString());
                    A_R_QTY = A_R_QTY * Convert.ToDouble(dr5[0].ItemArray[6].ToString());
                    R_QTY = R_QTY * Convert.ToDouble(dr5[0].ItemArray[7].ToString());
                    S_QTY = S_QTY * Convert.ToDouble(dr5[0].ItemArray[8].ToString());
                    M4_A_Y = 100 * Convert.ToDouble(dr5[0].ItemArray[4].ToString());
                }

                DataRow[] dr6 = dtcopy.Select("(product_id='" + dtprod.Tables[0].Rows[i][0].ToString() + "')");
                if (dr6.Length > 0)
                {
                    DataRow drme = dtme.NewRow();
                    drme.Table.Columns.Add();

                    drme["Group"] = dr6[0].ItemArray[0].ToString();
                    drme["ProdID"] = dr6[0].ItemArray[1].ToString();
                    drme["Input"] = t_cnt;
                    drme["PlanYield"] = dr6[0].ItemArray[9].ToString();
                    drme["ActualYield"] = Math.Round(A_QTY * 100, 2);

                    drme["Top1"] = Defe[0];
                    drme["Top1y"] = Def_QTY[0];
                    drme["Top2"] = Defe[1];
                    drme["Top2y"] = Def_QTY[1];
                    drme["Top3"] = Defe[2];
                    drme["Top3y"] = Def_QTY[2];
                    drme["Top4"] = Defe[3];
                    drme["Top4y"] = Def_QTY[3];
                    drme["Top5"] = Defe[4];
                    drme["Top5y"] = Def_QTY[4];

                    drme["Top6"] = Defe[5];
                    drme["Top6y"] = Def_QTY[5];
                    drme["Top7"] = Defe[6];
                    drme["Top7y"] = Def_QTY[6];
                    drme["Top8"] = Defe[7];
                    drme["Top8y"] = Def_QTY[7];
                    drme["Top9"] = Defe[8];
                    drme["Top9y"] = Def_QTY[8];
                    drme["Top10"] = Defe[9];
                    drme["Top10y"] = Def_QTY[9];
                    /*8040 Top1~Top5*/
                    drme["M1Input"] = M1_Input;
                    drme["M1ActualYield"] = Math.Round(M1_A_Y, 2);
                    drme["M1Top1"] = Defe_M1[0];
                    drme["M1Top1y"] = Def_QTY_M1[0];
                    drme["M1Top2"] = Defe_M1[1];
                    drme["M1Top2y"] = Def_QTY_M1[1];
                    drme["M1Top3"] = Defe_M1[2];
                    drme["M1Top3y"] = Def_QTY_M1[2];
                    drme["M1Top4"] = Defe_M1[3];
                    drme["M1Top4y"] = Def_QTY_M1[3];
                    drme["M1Top5"] = Defe_M1[4];
                    drme["M1Top5y"] = Def_QTY_M1[4];

                    drme["M1Top6"] = Defe_M1[5];
                    drme["M1Top6y"] = Def_QTY_M1[5];
                    drme["M1Top7"] = Defe_M1[6];
                    drme["M1Top7y"] = Def_QTY_M1[6];
                    drme["M1Top8"] = Defe_M1[7];
                    drme["M1Top8y"] = Def_QTY_M1[7];
                    drme["M1Top9"] = Defe_M1[8];
                    drme["M1Top9y"] = Def_QTY_M1[8];
                    drme["M1Top10"] = Defe_M1[9];
                    drme["M1Top10y"] = Def_QTY_M1[9];
                    /*8050 Top1~Top5*/
                    drme["M2Input"] = M2_Input;
                    drme["M2ActualYield"] = Math.Round(M2_A_Y, 2);
                    drme["M2Top1"] = Defe_M2[0];
                    drme["M2Top1y"] = Def_QTY_M2[0];
                    drme["M2Top2"] = Defe_M2[1];
                    drme["M2Top2y"] = Def_QTY_M2[1];
                    drme["M2Top3"] = Defe_M2[2];
                    drme["M2Top3y"] = Def_QTY_M2[2];
                    drme["M2Top4"] = Defe_M2[3];
                    drme["M2Top4y"] = Def_QTY_M2[3];
                    drme["M2Top5"] = Defe_M2[4];
                    drme["M2Top5y"] = Def_QTY_M2[4];

                    drme["M2Top6"] = Defe_M2[5];
                    drme["M2Top6y"] = Def_QTY_M2[5];
                    drme["M2Top7"] = Defe_M2[6];
                    drme["M2Top7y"] = Def_QTY_M2[6];
                    drme["M2Top8"] = Defe_M2[7];
                    drme["M2Top8y"] = Def_QTY_M2[7];
                    drme["M2Top9"] = Defe_M2[8];
                    drme["M2Top9y"] = Def_QTY_M2[8];
                    drme["M2Top10"] = Defe_M2[9];
                    drme["M2Top10y"] = Def_QTY_M2[9];
                    /*8080 Top1~Top5*/
                    drme["M3Input"] = M3_Input;
                    drme["M3ActualYield"] = Math.Round(M3_A_Y, 2);
                    drme["M3Top1"] = Defe_M3[0];
                    drme["M3Top1y"] = Def_QTY_M3[0];
                    drme["M3Top2"] = Defe_M3[1];
                    drme["M3Top2y"] = Def_QTY_M3[1];
                    drme["M3Top3"] = Defe_M3[2];
                    drme["M3Top3y"] = Def_QTY_M3[2];
                    drme["M3Top4"] = Defe_M3[3];
                    drme["M3Top4y"] = Def_QTY_M3[3];
                    drme["M3Top5"] = Defe_M3[4];
                    drme["M3Top5y"] = Def_QTY_M3[4];

                    drme["M3Top6"] = Defe_M3[5];
                    drme["M3Top6y"] = Def_QTY_M3[5];
                    drme["M3Top7"] = Defe_M3[6];
                    drme["M3Top7y"] = Def_QTY_M3[6];
                    drme["M3Top8"] = Defe_M3[7];
                    drme["M3Top8y"] = Def_QTY_M3[7];
                    drme["M3Top9"] = Defe_M3[8];
                    drme["M3Top9y"] = Def_QTY_M3[8];
                    drme["M3Top10"] = Defe_M3[9];
                    drme["M3Top10y"] = Def_QTY_M3[9];
                    /*8090 Top1~Top5*/
                    drme["M4Input"] = M4_Input;
                    drme["M4ActualYield"] = Math.Round(M4_A_Y, 2);
                    drme["M4Top1"] = Defe_M4[0];
                    drme["M4Top1y"] = Def_QTY_M4[0];
                    drme["M4Top2"] = Defe_M4[1];
                    drme["M4Top2y"] = Def_QTY_M4[1];
                    drme["M4Top3"] = Defe_M4[2];
                    drme["M4Top3y"] = Def_QTY_M4[2];
                    drme["M4Top4"] = Defe_M4[3];
                    drme["M4Top4y"] = Def_QTY_M4[3];
                    drme["M4Top5"] = Defe_M4[4];
                    drme["M4Top5y"] = Def_QTY_M4[4];

                    drme["M4Top6"] = Defe_M4[5];
                    drme["M4Top6y"] = Def_QTY_M4[5];
                    drme["M4Top7"] = Defe_M4[6];
                    drme["M4Top7y"] = Def_QTY_M4[6];
                    drme["M4Top8"] = Defe_M4[7];
                    drme["M4Top8y"] = Def_QTY_M4[7];
                    drme["M4Top9"] = Defe_M4[8];
                    drme["M4Top9y"] = Def_QTY_M4[8];
                    drme["M4Top10"] = Defe_M4[9];
                    drme["M4Top10y"] = Def_QTY_M4[9];

                    dtme.Rows.Add(drme);
                }
                dtme.DefaultView.Sort = "Group,ProdID asc";
            }
            this.DataGrid2.DataSource = dtme;
            this.DataGrid2.DataBind();
        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBox1.Checked)
            {
                foreach (ListItem li in this.cblLotType.Items)
                {
                    li.Selected = true;
                }
            }
            else
            {
                foreach (ListItem li in this.cblLotType.Items)
                {
                    li.Selected = false;
                }
            }
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.lblErrMsg.Text = string.Empty;
            string strErrorMsg = string.Empty;

            if (this.txtFromTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "開始時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
            }

            if (this.txtToTime.Text.Trim().Replace("'", "") == "")
            {
                strErrorMsg += "結束時間為空！\n";
                this.lblErrMsg.Text = strErrorMsg;
                return;
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
            RecordLog Log = new RecordLog();
            if (Log.InsertLog(((UserInfo)Session["UserInfo"]).FabId, "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), ((UserInfo)Session["UserInfo"]).UserID, Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            Configuration cfg = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            string strConnectionName = cfg.AppSettings.Settings["DefaultConnectionStringName"].Value;
            string strConnectionString = cfg.ConnectionStrings.ConnectionStrings[strConnectionName].ConnectionString;
            sqlConn = new SqlConnection(strConnectionString);
            if (this.RadioButtonList1.Items[0].Selected == true)
            {
                this.BindModule();
                this.DataGrid1.Visible = true;
                this.DataGrid2.Visible = false;
            }
            else
            {
                this.BindCell2();
                this.DataGrid1.Visible = false;
                this.DataGrid2.Visible = true;
            }
        }
        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=work.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            if (this.RadioButtonList1.Items[0].Selected == true)
            {
                this.DataGrid1.RenderControl(oHtmlTextWriter);
            }
            else
            {
                this.DataGrid2.RenderControl(oHtmlTextWriter);
            }
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
        protected void DlProdID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void dlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
