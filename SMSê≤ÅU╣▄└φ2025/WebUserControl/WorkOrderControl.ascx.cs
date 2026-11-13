using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
public partial class WebUserControl_WorkOrderControl : System.Web.UI.UserControl
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
            return this.tbValue.Text;
        }
        set
        {
            this.tbValue.Text = value;
        }
    }
    [Browsable(true)]
    public int MaxLen
    {
        get
        {
            return this.tbValue.MaxLength;
        }
        set
        {
            this.tbValue.MaxLength = value;
        }
    }
}