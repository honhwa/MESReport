using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_OQCTypeControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    [Browsable(true)]
    public string strTitle
    {
        get
        {
            return this.lblTitle.Text;
        }
        set
        {
            this.lblTitle.Text = value;
        }
    }
    [Browsable(true)]
    public string strValue
    {
        get
        {
            string strTransMode = "";
            for (int i = 0; i < 2; i++)
            {
                if (rblTransMode.Items[i].Selected == true)
                {
                    strTransMode = rblTransMode.Items[i].Value.ToString();
                }
            }
            return strTransMode;
        }
    }
    public RadioButtonList GetRadioButtonList
    {
        get
        {
            return this.rblTransMode;
        }
    }
}