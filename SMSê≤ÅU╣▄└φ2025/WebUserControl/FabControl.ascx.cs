using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_FabControl : System.Web.UI.UserControl
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
            string strRadioButtonList = "";
            for (int i = 0; i < 3; i++)
            {
                if (rblFactoryName.Items[i].Selected == true)
                {
                    strRadioButtonList = rblFactoryName.Items[i].Value.ToString();
                }
            }
            return strRadioButtonList;
        }
        set
        {
            this.rblFactoryName.SelectedValue = value;
        }
    }
}