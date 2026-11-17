using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_DateControl : System.Web.UI.UserControl
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
    public string FromDate
    {
        get
        {
            return this.tbFromDate.Text;
        }
        set
        {
            this.tbFromDate.Text = value;
        }
    }
    [Browsable(true)]
    /// <summary>
    /// FromDate
    /// </summary>
    public string ToDate
    {
        get
        {
            return this.tbToDate.Text;
        }
        set
        {
            this.tbToDate.Text = value;
        }
    }

    public void BoundData()
    {
        tbFromDate.Text = DateTime.Now.AddDays(-1).AddHours(-8).ToString("yyyy-MM-dd");
        tbToDate.Text = DateTime.Now.AddDays(-1).AddHours(-8).ToString("yyyy-MM-dd");

        tbFromDate.Attributes.Add("readonly", "readonly");
        tbToDate.Attributes.Add("readonly", "readonly");
    }
}