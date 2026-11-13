using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

public partial class WebUserControl_DefectControl1 : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    [Browsable(true)]
    public DropDownList DropDownListDefect
    {
        get
        {
            return this.ddlValue;
        }
        set
        {
            this.ddlValue = value;
        }
    }
    [Browsable(true)]
    public DataTable DataTableDefectValue
    {
        set
        {
            this.ddlValue.DataSource = value;
            ddlValue.DataTextField = "IDDesc";
            ddlValue.DataValueField = "ID";
            ddlValue.DataBind();
            //GetDataBound();
        }
    }
}