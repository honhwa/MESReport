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

namespace ESM
{
    public partial class WebEQSM : System.Web.UI.Page
    {

        #region private Property
        private Eqsms eq = new Eqsms();
        private List<Panel> listPanel = new List<Panel>();
        private List<Panel> listPanelAdd = new List<Panel>();
        #endregion

        #region Public\Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                this.Form2.Attributes.Add("onload", "window.moveTo(0,0);");
                this.LinkButton1.Attributes.Add("onclick", "window.close();");
                this.eq.SubLineID = Request.QueryString["userid"];
                this.eq.Read();
                this.Form2.Page.Title = "SubLineID:" + this.eq.SubLineID + "連接EQ狀況 " + " EQ數量:" + this.eq.Count;              
                if (this.eq.Count == 0)
                {
                    //Response.Write("<script>alert('該SubLineID沒有連接EQ!')</script>");
                    return;
                }
                else
                {
                    this.Packing();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region private Methods

        /// <summary>
        /// 加入EQ
        /// </summary>
        private void Packing()
        {
            try
            {
                this.AddPanel();
                int j = 0;
                for (int i = 0; i < this.eq.Count; i++)
                {
                    //if(i = (this.eq.Count-4) && this.eq.Count Mod 11)

                    EQCUserControl.EQUnit eqUnit = (EQCUserControl.EQUnit)LoadControl("~/WebControl/EQUnit.ascx");
                    this.listPanel[j].Controls.Add(eqUnit);
                    //(ASP.eqcusercontrol_equnit_ascx)LoadControl(
                    eqUnit.DataSource = this.eq[i];
                    eqUnit.Bind();

                    j++;
                    if (j == 5)
                        j = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Private Methods

        private void AddPanel()
        {
            this.listPanel.Add(this.Panel1);
            this.listPanel.Add(this.Panel2);
            this.listPanel.Add(this.Panel3);
            this.listPanel.Add(this.Panel4);
            this.listPanel.Add(this.Panel5);
            this.listPanel.Add(this.Panel6);

            this.listPanelAdd.Add(this.Panel7);
            this.listPanelAdd.Add(this.Panel8);
            this.listPanelAdd.Add(this.Panel9);
            this.listPanelAdd.Add(this.Panel10);
            this.listPanelAdd.Add(this.Panel11);
        }

        #endregion
        protected void Timer1_Tick(object sender, EventArgs e)
        {

            this.UpdatePanel1.Update();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            this.LinkButton1.Attributes.Add("OnClick", " window.close ");
        }
    }
}
