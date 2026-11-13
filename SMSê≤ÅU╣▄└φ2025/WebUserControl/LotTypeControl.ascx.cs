using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

public partial class WebUserControl_LotTypeControl : System.Web.UI.UserControl
{    
    #region Parameters
    //private string strFabID;
    #endregion
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
    public ListBox ListBoxFromValue
    {
        get
        {
            return this.lbFromValue;
        }
        set
        {
            this.lbFromValue = value;
        }
    }
    [Browsable(true)]
    public ListBox ListBoxToValue
    {
        get
        {
            return this.lbToValue;
        }
        set
        {
            this.lbToValue = value;
        }
    }
    [Browsable(true)]
    public DataTable DataTableValue
    {
        set
        {
            this.lbFromValue.DataSource = value;
            lbFromValue.DataTextField = "IDDesc";
            lbFromValue.DataValueField = "ID";
            lbFromValue.DataBind();
            this.lbToValue.Items.Clear();
        }
    }
    [Browsable(true)]
    protected void ibLotTypeUp_Click(object sender, ImageClickEventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        mc.MoveData(lbFromValue, lbToValue, lbToValue.SelectedValue, "Up");
    }
    [Browsable(true)]
    protected void ibLotTypeDown_Click(object sender, ImageClickEventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        mc.MoveData(lbFromValue, lbToValue, lbFromValue.SelectedValue, "Down");
    }
}