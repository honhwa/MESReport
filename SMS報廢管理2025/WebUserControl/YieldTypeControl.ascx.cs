using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

public partial class WebUserControl_YieldTypeControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
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
    public DataTable DataTableValue
    {
        set
        {
            this.ddlValue.Items.Clear();
            this.ddlValue.DataSource = value;
            ddlValue.DataTextField = "IDDesc";
            ddlValue.DataValueField = "ID";
            ddlValue.DataBind();
            
        }
    }
    [Browsable(true)]
    public string strValue
    {
        get
        {
            return this.ddlValue.SelectedValue;
        }
        set
        {
            this.ddlValue.SelectedValue = value;
        }
    }
}