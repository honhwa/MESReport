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
    public partial class MoudleSubLineStatus : System.Web.UI.Page
    {
        private SubLines sls = new SubLines();
        private List<System.Web.UI.UserControl> listModule = new List<System.Web.UI.UserControl>();
        protected void Page_Load(object sender, EventArgs e)
        {
            RecordLog Log = new RecordLog();
            if (Log.InsertLog("-", "Query", HttpContext.Current.Request.Url.AbsolutePath.ToString(), "NO", Request.UserHostAddress.ToString()) == false)
            {
                this.Response.Write("<script language=javascript>window.alert('RecordLog Error！');</script>");
            }
            //int i = 46;
            //this.TextBox1.Text = i.ToString("x");
            this.form1.Page.Title = "Module:All SubLineID State";
            this.addUserControl(this.form1);
            //this.addControl();
            this.sls.RowFilter = " AND shopcd = 'MODULE' ";
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

                    if (con.GetType() == this.ModuleSubLineUnit2.GetType() && con.Controls.Count > 0)
                    {
                        this.listModule.Add((System.Web.UI.UserControl)con);
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
                if (this.listModule.Count == 0)
                {
                    return;
                }

                foreach (EQCUserControl.ModuleSubLineUnit usl in listModule)
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



        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.UpdatePanel1.Update();
        }

        private void BindColor()
        {
            DataTable dt = new DataTable();
            dt = this.sls.dtGetColor();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Color cl = ColorTranslator.FromOle(int.Parse(dt.Rows[i]["EQCCDCOLOR"].ToString().Trim()));
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
