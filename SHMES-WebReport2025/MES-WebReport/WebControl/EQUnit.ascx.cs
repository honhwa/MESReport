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
using System.Drawing;

namespace ESM.EQCUserControl
{
    public partial class EQUnit : System.Web.UI.UserControl
    {
        #region Private Property
        private object obj = null;
        #endregion

        #region Public\Protected Methods
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 设置UserControl的DataSource
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

        public void Bind()
        {
            TimeSpan ts = new TimeSpan();
            try
            {
                if (this.obj == null)
                {
                    //Response.Write("<script>alert('请先绑定EQUnit的数据源')</script>");
                    return;
                }
                Eqsm eq = (Eqsm)this.obj;
                int x = Int32.Parse(eq.SetColor);
                this.Panel1.BackColor = ColorTranslator.FromOle(x);
                this.lbEQID.Text = eq.EqID;
                ts = DateTime.Now - Convert.ToDateTime(eq.TransDate);
                this.lbKt.Text = (ts.Days * 24 + ts.Hours).ToString() + "H:" + ts.Minutes.ToString()+"M";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}