using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
public partial class WebUserControl_ProductControl : System.Web.UI.UserControl
{
    #region Parameters
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    [Browsable(true)]
    public string GetPlant
    {
        get
        {
            return this.tbPlant.Text;
        }
        set
        {
            this.tbPlant.Text = value;
        }
    }
    [Browsable(true)]
    public string GetTitle
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
    public DropDownList DropDownListSIZE
    {
        get
        {
            return this.ddlSIZE;
        }
        set
        {
            this.ddlSIZE = value;
        }
    }
    [Browsable(true)]
    public DropDownList DropDownListProjectName
    {
        get
        {
            return this.ddlProjectName;
        }
        set
        {
            this.ddlProjectName = value;
        }
    }
    [Browsable(true)]
    public ListBox ListBoxFromValue
    {
        get
        {
            return this.lbFromProduct;
        }
        set
        {
            this.lbFromProduct = value;
        }
    }
    [Browsable(true)]
    public ListBox ListBoxToValue
    {
        get
        {
            return this.lbToProduct;
        }
        set
        {
            this.lbToProduct = value;
        }
    }
    [Browsable(true)]
    public DataTable DataTableSIZEValue
    {
        set
        {
            this.ddlSIZE.DataSource = value;
            ddlSIZE.DataTextField = "IDDesc";
            ddlSIZE.DataValueField = "ID";
            ddlSIZE.DataBind();
            GetDataBound("SIZE");
        }
    }
    [Browsable(true)]
    public DataTable DataTableProjectNameValue
    {
        set
        {
            this.ddlProjectName.DataSource = value;
            ddlProjectName.DataTextField = "IDDesc";
            ddlProjectName.DataValueField = "ID";
            ddlProjectName.DataBind();
        }
    }
    [Browsable(true)]
    public DataTable DataTableProductValue
    {
        set
        {
            this.lbFromProduct.DataSource = value;
            lbFromProduct.DataTextField = "IDDesc";
            lbFromProduct.DataValueField = "ID";
            lbFromProduct.DataBind();
            this.lbToProduct.Items.Clear();
        }
    }
    protected void ibProductUp_Click(object sender, ImageClickEventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        mc.MoveData(lbFromProduct, lbToProduct, lbToProduct.SelectedValue, "Up");
    }
    protected void ibProductDown_Click(object sender, ImageClickEventArgs e)
    {
        ModuleClass mc = new ModuleClass();
        mc.MoveData(lbFromProduct, lbToProduct, lbFromProduct.SelectedValue, "Down");
    }
    protected void GetDataBound(string strMode)
    {
        ModuleClass mc = new ModuleClass();
        string strFabID = mc.GetFABID(tbPlant.Text);
        if (strMode != "ProjectName")
        {
            InitialBoundProjectNameData(strFabID, tbPlant.Text);
        }
        InitialBoundProductData(strFabID, tbPlant.Text);
    }
    protected void InitialBoundProjectNameData(string strFabID, string strPlant)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtProjectName = mc.GetProjectName(strFabID, ddlSIZE.SelectedValue.ToString());
        ddlProjectName.DataSource = dtProjectName;
        ddlProjectName.DataTextField = "IDDesc";
        ddlProjectName.DataValueField = "ID";
        ddlProjectName.DataBind();
    }
    protected void InitialBoundProductData(string strFabID, string strPlant)
    {
        ModuleClass mc = new ModuleClass();
        DataTable dtProduct = mc.GetProductList(strFabID, strPlant, ddlSIZE.SelectedValue.ToString(), ddlProjectName.SelectedValue.ToString());
        lbFromProduct.DataSource = dtProduct;
        lbFromProduct.DataTextField = "IDDesc";
        lbFromProduct.DataValueField = "ID";
        lbFromProduct.DataBind();

        lbToProduct.Items.Clear();
    }
    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetDataBound("ProjectName");
    }
    protected void ddlSIZE_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetDataBound("SIZE");
    }
}