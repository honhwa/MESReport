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
using System.Collections.Generic;
using System.Drawing;
using Report.Library;

namespace ESM
{
    public partial class WebCell2SubLineStatus : System.Web.UI.Page
    {
        //#region Private Property
        private SubLines sls = new SubLines();
        private List<System.Web.UI.UserControl> listCell2 = new List<System.Web.UI.UserControl>();
        //private List<UserControl> listFinal = new List<UserControl>();
        //private List<UserControl> listCassette = new List<UserControl>();
        //private List<UserControl> listLCboat = new List<UserControl>();
        //private List<UserControl> listEnd = new List<UserControl>();
        //private List<UserControl> listLCinjection = new List<UserControl>();
        //private List<UserControl> listAuto = new List<UserControl>();
        //private List<UserControl> listSemi = new List<UserControl>();
        //private List<UserControl> listVacuum = new List<UserControl>();
        //#endregion

        //#region Protected\Public Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            this.form1.Page.Title = "Cell2:All SubLineID State";
            this.addUserControl(this.form1);
            //this.addControl();
            this.sls.RowFilter = " AND shopcd = 'CELL2' ";
            this.sls.Read();
            //this.BindSubLine();
            this.BindUserControl();
            this.BindColor();
        }

        /// <summary>
        /// 添加UserControl進集合

        /// </summary>
        /// <param name="control"></param>
        private void addUserControl(Control control)
        {
            try
            {
                if (control.Controls.Count == 0)
                    return;


                foreach (Control con in control.Controls)
                {

                    if (con.GetType() == this.SubLineUnit1End10.GetType() && con.Controls.Count > 0)
                    {
                        this.listCell2.Add((System.Web.UI.UserControl)con);
                        continue;
                    }
                    else
                    {
                        this.addUserControl(con);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 绑定UserControl咨询
        /// </summary>
        private void BindUserControl()
        {
            try
            {
                if (this.listCell2.Count == 0)
                {
                    return;
                }

                foreach (EQCUserControl.SubLineUnit usl in listCell2)
                {
                    SubLine sl = this.sls.GetSubLine(usl.SubLineID);
                    if (sl == null)
                        continue;
                    usl.DataSource = sl;
                    usl.Bind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        ///// <summary>
        ///// 缩放比例
        ///// </summary>
        ///// <param name="control"></param>
        ///// <param name="proprotion"></param>
        //public void Pantograph(Control control,int proprotion)
        //{
        //    for (int i = 0; i < control.Controls.Count; i++)
        //    {

        //    }
        //}


        //protected void SubLineUnitRough1_Load(object sender, EventArgs e)
        //{

        //}
        //#endregion

        //#region Private Methods

        //#region UserControl
        //    /// <summary>
        //    /// 将UserControl加入List中

        //    /// </summary>
        //    private void addControl()
        //    {
        //        this.addRoughClean();
        //        this.addFinal();
        //        this.addCassette();
        //        this.addLCboat();
        //        this.addEnd();
        //        this.addLCinjection();
        //        this.addVacuumAnneal();
        //        this.addAuto();
        //        this.addSemi();
        //    }

        //    /// <summary>
        //    /// 加入RoughClean的UserControl
        //    /// </summary>
        //    private void addRoughClean()
        //    {
        //        this.listRough.Add(this.SubLineUnitRough1);
        //        this.listRough.Add(this.SubLineUnitRough2);
        //        this.listRough.Add(this.SubLineUnitRough3);
        //    }

        //    /// <summary>
        //    /// 加入FinalClean的UserControl
        //    /// </summary>
        //    private void addFinal()
        //    {
        //        this.listFinal.Add(this.SubLineUnitFinal1);
        //        this.listFinal.Add(this.SubLineUnitFinal2);
        //        this.listFinal.Add(this.SubLineUnitFinal3);
        //    }

        //    /// <summary>
        //    /// 加入CassetteClean的UserControl
        //    /// </summary>
        //    private void addCassette()
        //    {
        //        this.listCassette.Add(this.SubLineUnitCassette1);
        //        this.listCassette.Add(this.SubLineUnitCassette2);
        //    }

        //    /// <summary>
        //    /// 加入LCboatClean的UserControl
        //    /// </summary>
        //    private void addLCboat()
        //    {
        //        this.listLCboat.Add(this.SubLineUnitLCboat1);
        //        this.listLCboat.Add(this.SubLineUnitLCboat2);

        //    }

        //    /// <summary>
        //    /// 加入End Seal(re-gap)的UserControl
        //    /// </summary>
        //    private void addEnd()
        //    {
        //        this.listEnd.Add(this.SubLineUnit1End1);
        //        this.listEnd.Add(this.SubLineUnit1End2);
        //        this.listEnd.Add(this.SubLineUnit1End3);
        //        this.listEnd.Add(this.SubLineUnit1End4);
        //        this.listEnd.Add(this.SubLineUnit1End5);
        //        this.listEnd.Add(this.SubLineUnit1End6);
        //        this.listEnd.Add(this.SubLineUnit1End7);
        //        this.listEnd.Add(this.SubLineUnit1End8);
        //        this.listEnd.Add(this.SubLineUnit1End9);
        //        this.listEnd.Add(this.SubLineUnit1End10);
        //        this.listEnd.Add(this.SubLineUnit1End11);
        //        this.listEnd.Add(this.SubLineUnit1End12);
        //        this.listEnd.Add(this.SubLineUnit1End13);
        //        this.listEnd.Add(this.SubLineUnit1End14);
        //        this.listEnd.Add(this.SubLineUnit1End15);
        //        this.listEnd.Add(this.SubLineUnit1End16);
        //        this.listEnd.Add(this.SubLineUnit1End17);
        //        this.listEnd.Add(this.SubLineUnit1End18);
        //        this.listEnd.Add(this.SubLineUnit1End19);
        //        this.listEnd.Add(this.SubLineUnit1End20);
        //        this.listEnd.Add(this.SubLineUnit1End21);
        //        this.listEnd.Add(this.SubLineUnit1End22);
        //    }

        //    /// <summary>
        //    /// 加入LC injection的UserControl
        //    /// </summary>
        //    private void addLCinjection()
        //    {
        //        this.listLCinjection.Add(this.SubLineUnitLCi1);
        //        this.listLCinjection.Add(this.SubLineUnitLCi2);
        //        this.listLCinjection.Add(this.SubLineUnitLCi3);
        //        this.listLCinjection.Add(this.SubLineUnitLCi4);
        //        this.listLCinjection.Add(this.SubLineUnitLCi5);
        //        this.listLCinjection.Add(this.SubLineUnitLCi6);
        //        this.listLCinjection.Add(this.SubLineUnitLCi7);
        //        this.listLCinjection.Add(this.SubLineUnitLCi8);
        //        this.listLCinjection.Add(this.SubLineUnitLCi9);
        //        this.listLCinjection.Add(this.SubLineUnitLCi10);
        //        this.listLCinjection.Add(this.SubLineUnitLCi11);
        //        this.listLCinjection.Add(this.SubLineUnitLCi12);
        //        this.listLCinjection.Add(this.SubLineUnitLCi13);
        //        this.listLCinjection.Add(this.SubLineUnitLCi14);
        //        this.listLCinjection.Add(this.SubLineUnitLCi15);
        //        this.listLCinjection.Add(this.SubLineUnitLCi16);
        //        this.listLCinjection.Add(this.SubLineUnitLCi17);
        //        this.listLCinjection.Add(this.SubLineUnitLCi18);
        //        this.listLCinjection.Add(this.SubLineUnitLCi19);
        //        this.listLCinjection.Add(this.SubLineUnitLCi20);
        //        this.listLCinjection.Add(this.SubLineUnitLCi21);
        //        this.listLCinjection.Add(this.SubLineUnitLCi22);
        //        this.listLCinjection.Add(this.SubLineUnitLCi23);
        //        this.listLCinjection.Add(this.SubLineUnitLCi24);
        //        this.listLCinjection.Add(this.SubLineUnitLCi25);
        //        this.listLCinjection.Add(this.SubLineUnitLCi26);
        //        this.listLCinjection.Add(this.SubLineUnitLCi27);
        //        this.listLCinjection.Add(this.SubLineUnitLCi28);
        //        this.listLCinjection.Add(this.SubLineUnitLCi29);
        //        this.listLCinjection.Add(this.SubLineUnitLCi30);
        //        this.listLCinjection.Add(this.SubLineUnitLCi31);
        //        this.listLCinjection.Add(this.SubLineUnitLCi32);
        //        this.listLCinjection.Add(this.SubLineUnitLCi33);
        //        this.listLCinjection.Add(this.SubLineUnitLCi34);
        //        this.listLCinjection.Add(this.SubLineUnitLCi35);
        //        this.listLCinjection.Add(this.SubLineUnitLCi36);
        //        this.listLCinjection.Add(this.SubLineUnitLCi37);
        //        this.listLCinjection.Add(this.SubLineUnitLCi38);
        //        this.listLCinjection.Add(this.SubLineUnitLCi39);
        //        this.listLCinjection.Add(this.SubLineUnitLCi40);
        //    }

        //    /// <summary>
        //    /// 加入Vacuum anneal的UserControl
        //    /// </summary>
        //    private void addVacuumAnneal()
        //    {
        //        this.listVacuum.Add(this.SubLineUnitVa1);
        //        this.listVacuum.Add(this.SubLineUnitVa2);
        //        this.listVacuum.Add(this.SubLineUnitVa3);
        //        this.listVacuum.Add(this.SubLineUnitVa4);
        //        this.listVacuum.Add(this.SubLineUnitVa5);
        //        this.listVacuum.Add(this.SubLineUnitVa6);
        //        this.listVacuum.Add(this.SubLineUnitVa7);
        //        this.listVacuum.Add(this.SubLineUnitVa8);
        //        this.listVacuum.Add(this.SubLineUnitVa9);
        //        this.listVacuum.Add(this.SubLineUnitVa10);
        //        this.listVacuum.Add(this.SubLineUnitVa11);
        //    }

        //    /// <summary>
        //    /// 加入Auto S/B的UserControl
        //    /// </summary>
        //    private void addAuto()
        //    {
        //        this.listAuto.Add(this.SubLineUnitAS1);
        //        this.listAuto.Add(this.SubLineUnitAS2);
        //    }

        //    /// <summary>
        //    /// 加入Semi S/B的UserControl
        //    /// </summary>
        //    private void addSemi()
        //    {
        //        this.listSemi.Add(this.SubLineUnitSS1);
        //        this.listSemi.Add(this.SubLineUnitSS2);
        //        this.listSemi.Add(this.SubLineUnitSS3);
        //        this.listSemi.Add(this.SubLineUnitSS4);
        //        this.listSemi.Add(this.SubLineUnitSS5);
        //        this.listSemi.Add(this.SubLineUnitSS6);
        //        this.listSemi.Add(this.SubLineUnitSS7);
        //        this.listSemi.Add(this.SubLineUnitSS8);
        //        this.listSemi.Add(this.SubLineUnitSS9);
        //        this.listSemi.Add(this.SubLineUnitSS10);
        //        this.listSemi.Add(this.SubLineUnitSS11);
        //        this.listSemi.Add(this.SubLineUnitSS12);
        //        this.listSemi.Add(this.SubLineUnitSS13);
        //        this.listSemi.Add(this.SubLineUnitSS14);
        //        this.listSemi.Add(this.SubLineUnitSS15);
        //        this.listSemi.Add(this.SubLineUnitSS16);
        //        this.listSemi.Add(this.SubLineUnitSS17);
        //        this.listSemi.Add(this.SubLineUnitSS18);
        //        this.listSemi.Add(this.SubLineUnitSS19);
        //        this.listSemi.Add(this.SubLineUnitSS20);
        //        this.listSemi.Add(this.SubLineUnitSS21);
        //        this.listSemi.Add(this.SubLineUnitSS22);
        //        this.listSemi.Add(this.SubLineUnitSS23);
        //        this.listSemi.Add(this.SubLineUnitSS24);
        //        this.listSemi.Add(this.SubLineUnitSS25);
        //        this.listSemi.Add(this.SubLineUnitSS26);
        //        this.listSemi.Add(this.SubLineUnitSS27);
        //        this.listSemi.Add(this.SubLineUnitSS28);
        //        this.listSemi.Add(this.SubLineUnitSS29);
        //        this.listSemi.Add(this.SubLineUnitSS30);
        //        this.listSemi.Add(this.SubLineUnitSS31);
        //        this.listSemi.Add(this.SubLineUnitSS32);
        //        this.listSemi.Add(this.SubLineUnitSS33);
        //        this.listSemi.Add(this.SubLineUnitSS34);
        //        this.listSemi.Add(this.SubLineUnitSS35);
        //        this.listSemi.Add(this.SubLineUnitSS36);
        //        this.listSemi.Add(this.SubLineUnitSS37);
        //        this.listSemi.Add(this.SubLineUnitSS38);
        //        this.listSemi.Add(this.SubLineUnitSS39);
        //        this.listSemi.Add(this.SubLineUnitSS40);
        //        this.listSemi.Add(this.SubLineUnitSS41);
        //        this.listSemi.Add(this.SubLineUnitSS42);
        //    }
        //    #endregion

        //#region BindSubLine
        ///// <summary>
        ///// 綁定所有的SubLine于UserControl上

        ///// </summary>
        //private void BindSubLine()
        //{
        //    try
        //    {
        //        SubLines roughSls = null;
        //        SubLines finalSls = new SubLines();
        //        SubLines cassetteSls = new SubLines();
        //        SubLines lCboatSls = new SubLines();
        //        SubLines endSls = new SubLines();
        //        SubLines lCinjectionSls = new SubLines();
        //        SubLines vacuumSls = new SubLines();
        //        SubLines autoSls = new SubLines();
        //        SubLines semiSls = new SubLines();
        //        if (this.sls.Count == 0)
        //        {
        //            //Response.Write("<script>alert('無SubLine資料!')</script>");
        //            return;
        //        }
        //        //roughSls = this.sls.Create("RoughClean");
        //        //this.BindRough(roughSls);

        //        //finalSls = this.sls.Create("FinalClean");
        //        //this.BindFinal(finalSls);

        //        //cassetteSls = this.sls.Create("CassetteClean");
        //        //this.BindCassette(cassetteSls);

        //        //lCboatSls = this.sls.Create("LCboatClean");
        //        //this.BindLCBoad(lCboatSls);

        //        //endSls = this.sls.Create("EndSeal(re-gap)");
        //        //this.BindEnd(endSls);

        //        //lCinjectionSls = this.sls.Create("LCinjection");
        //        //this.BindLCinjection(lCinjectionSls);

        //        //vacuumSls = this.sls.Create("Vacuumanneal");
        //        //this.BindVacuumanneal(vacuumSls);

        //        //autoSls = this.sls.Create("AutoS/B");
        //        //this.BindAuto(autoSls);

        //        //semiSls = this.sls.Create("SemiS/B");
        //        //this.BindSemi(semiSls);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 對RoughClean區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindRough(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listRough.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面RoughClean區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //Response.Write("111");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listRough.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listRough[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listRough[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對FinalClean區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindFinal(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listFinal.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert("+"頁面FinalClean區域中SubLine個數與實際該區域綁定SubLine個數不相符!"+")</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listFinal.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listFinal[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listFinal[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對CassetteClean區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindCassette(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listCassette.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面CassetteClean區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listCassette.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listCassette[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listCassette[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對LCBoadClean區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindLCBoad(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listLCboat.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面LC boat Clean區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listLCboat.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listLCboat[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listLCboat[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對End Seal(re-gap)區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindEnd(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listEnd.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面End Seal(re-gap)區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listEnd.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listEnd[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listEnd[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對LC injection區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindLCinjection(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listLCinjection.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面LC injection區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listLCinjection.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listLCinjection[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listLCinjection[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對Vacuumanneal區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindVacuumanneal(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listVacuum.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面Vacuum anneal區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listVacuum.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listVacuum[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listVacuum[i]).Bind();
        //        j++;
        //    }
        //}

        ///// <summary>
        ///// 對Auto S/B區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindAuto(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listAuto.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面Auto S/B區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listAuto.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listAuto[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listAuto[i]).Bind();
        //        j++;
        //    }
        //}


        ///// <summary>
        ///// 對Semi S/B區域的UserControl進行綁定
        ///// </summary>
        ///// <param name="tmpsls"></param>
        //private void BindSemi(SubLines tmpsls)
        //{
        //    int j = 0;
        //    if (this.listSemi.Count != tmpsls.Count)
        //    {
        //        //Response.Write("<script>alert('頁面Semi S/B區域中SubLine個數與實際該區域綁定SubLine個數不相符!')</script>");
        //        //return;
        //    }
        //    for (int i = 0; i < this.listSemi.Count; i++)
        //    {
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listSemi[i]).DataSource = tmpsls[j];
        //        ((ASP.eqcusercontrol_sublineunit_ascx)this.listSemi[i]).Bind();
        //        j++;
        //    }
        //}

        //#endregion


        //#endregion
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //this.UpdatePanel1.Update();
        }

        private void BindColor()
        {
            DataTable dt = new DataTable();
            dt=this.sls.dtGetColor();
            for (int i = 0; i < dt.Rows.Count; i++)
            {   
                Color cl = ColorTranslator.FromOle(int.Parse(dt.Rows[i]["EQCCDCOLOR"].ToString().Trim() ));
                switch (dt.Rows[i]["EQCCD"].ToString().Trim())
                {
                    /*
                    D	Down
                    E	ENG
                    I	IDLE
                    N	NonOperation
                    O	Other
                    P	PM
                    R	RUN
                    S	Setup
                    T	PartialDown 
                    W	Wait
                    */
                    case "D":
                        this.lblDown.BackColor = cl;
                        break;
                    case "E":
                        this.lblENG.BackColor = cl;
                        break;
                    case "I":
                        this.lblIDLE.BackColor = cl;
                        break;
                    case "N":
                        this.lblNonOperation.BackColor = cl;
                        break;
                    case "O":
                        this.lblOther.BackColor = cl;
                        break;
                    case "P":
                        this.lblPM.BackColor = cl;
                        break;
                    case "R":
                        this.lblRUN.BackColor = cl;
                        break;
                    case "S":
                        this.lblSetup.BackColor = cl;
                        break;
                    case "T":
                        this.lblPartial.BackColor = cl;
                        break;
                    case "W":
                        this.lblWait.BackColor = cl;
                        break;

                }
               
                
            }

        }
    }
}
