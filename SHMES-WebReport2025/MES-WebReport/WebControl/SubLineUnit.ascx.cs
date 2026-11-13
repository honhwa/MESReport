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
using System.ComponentModel;
using System.Drawing;


namespace ESM.EQCUserControl
{
    public partial class SubLineUnit : System.Web.UI.UserControl
    {
        private object obj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lbID.BackColor = this.Button1.BackColor;
            this.lbKT.BackColor = this.Button1.BackColor;
            BindToopTip();
        }


        [Browsable(true)]
        /// <summary>
        /// UserControl的寬
        /// </summary>
        public Unit Width
        {
            get
            {
                return this.Button1.Width;
            }
            set
            {
                this.Button1.Width = value;
            }
        }

        [Browsable(true)]
        /// <summary>
        /// UserControl的高
        /// </summary>
        public Unit Height
        {
            get
            {
                return this.Button1.Height;
            }
            set
            {
                this.Button1.Height = value;
            }
        }

        /// <summary>
        /// 設定SubLineID
        /// </summary>
        [Browsable(true)]
        public string SubLineID
        {
            get
            {
                return this.lbID.Text;
            }
            set
            {
                this.lbID.Text = value;
            }
        }

        [Browsable(false)]
        /// <summary>
        /// 取得或設定UserControl的數據源
        /// </summary>
        public object DataSource
        {
            get
            {
                return this.obj;
            }
            set
            {
                this.obj = value;
            }
        }

        /// <summary>
        /// 綁定UserControl
        /// </summary>
        public void Bind()
        {
            TimeSpan ts = new TimeSpan();

            try
            {
                if (this.obj == null)
                {
                    //Response.Write("<Script>alert('请先绑定SubLineUnit的数据源!')</Script>");
                    return;
                }
                else
                {
                    SubLine sl = (SubLine)this.obj;
                    //string hex = sl.SetColor;
                    //int x = int.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    Color cl = ColorTranslator.FromOle(int.Parse(sl.SetColor));

                    this.Button1.BackColor = cl;



                    this.lbID.Text = sl.SubLineID;
                    this.lbID.BackColor = this.Button1.BackColor;
                    //this.lbTD.Text = sl.TransDate;
                    ts = DateTime.Now - Convert.ToDateTime(sl.TransDate);
                    
                    this.lbKT.Text = (ts.Days*24 + ts.Hours ).ToString()+ "H:" + ts.Minutes.ToString()+"M";
                    this.lbKT.BackColor = this.Button1.BackColor;
                    Session[this.ID] = this.ID;
                    BindToopTip();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string src = "";
            if (this.obj != null)
            {
                Session["SubLineID"] = ((SubLine)this.obj).SubLineID;
            }
            else
            {
                Session["SubLineID"] = "";
            }
            src = @"window.open('WebEQSM.aspx?userid=" + Session["SubLineID"] + "',null,'toolbar=no,scrollbars=yes,width=1024,height=768,resizable=no')";
            //ClientScriptManager cs = Page.ClientScript;
            //cs.RegisterClientScriptBlock(this.get

            ScriptManager.RegisterClientScriptBlock(this.Button1, this.GetType(), "SubLineID", src, true);
            
        }
        private void BindToopTip()
        {
            try
            {
                if (this.obj == null)
                {
                    return;
                }
                else
                {
                    SubLine sl = (SubLine)this.obj;

                    Button1.Attributes.Add("onmouseover", "currencol=this.style.backgroundColor;this.style.backgroundColor='#ccFFff'");
                    Button1.Attributes.Add("onmouseout", "this.style.backgroundColor=currencol");
                    Button1.ToolTip = sl.StringToolTip;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}