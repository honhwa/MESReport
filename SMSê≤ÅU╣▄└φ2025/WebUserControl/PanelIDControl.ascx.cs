using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_PanelIDControl : System.Web.UI.UserControl
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
    public string PanelID
    {
        get
        {
            return this.tbPanelID.Text;
        }
        set
        {
            this.tbPanelID.Text = value;
        }
    }

    public void BoundData()
    {
        //tbSNCode.Text = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd");

        //tbSNCode.Attributes.Add("readonly", "readonly");

    }
}