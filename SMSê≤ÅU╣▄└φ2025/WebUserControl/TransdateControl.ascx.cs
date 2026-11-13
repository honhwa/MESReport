using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_TransdateControl : System.Web.UI.UserControl
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
    public string FromTime
    {
        get
        {
            return this.tbFromTime.Text;
        }
        set
        {
            this.tbFromTime.Text = value;
        }
    }
    [Browsable(true)]
    /// <summary>
    /// FromDate
    /// </summary>
    public string ToTime
    {
        get
        {
            return this.tbToTime.Text;
        }
        set
        {
            this.tbToTime.Text = value;
        }
    }

    public void BoundData()
    {
        tbFromTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        tbToTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        tbFromTime.Attributes.Add("readonly", "readonly");
        tbToTime.Attributes.Add("readonly", "readonly");
    }
}