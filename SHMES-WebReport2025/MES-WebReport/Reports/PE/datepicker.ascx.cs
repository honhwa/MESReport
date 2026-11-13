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

namespace Report
{
    public partial class datepicker : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                caldate.Style["position "] = "absolute ";
                TextBox1.Text = DateTime.Today.Date.ToString("yyyy-MM-dd");
            }
            
          
        }
        protected void ibtndate_Click(object sender, ImageClickEventArgs e)
        {
            caldate.Visible = true;
           
        }

        protected void caldate_SelectionChanged(object sender, EventArgs e)
        {
            TextBox1.Text = caldate.SelectedDate.Date.ToString("yyyy-MM-dd");
            caldate.Visible =false;
        }
         public string   datatimer()
        {
            return TextBox1.Text;
        }
    }
}