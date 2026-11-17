using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_DayControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BoundData();
        }
    }
    [Browsable(true)]
    /// <summary>
    /// FromDate
    /// </summary>
    public string Date
    {
        get
        {
            return this.tbDate.Text;
        }
        set
        {
            this.tbDate.Text = value;
        }
    }


    public void BoundData()
    {
        tbDate.Text = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd");

        tbDate.Attributes.Add("readonly", "readonly");
    }
}