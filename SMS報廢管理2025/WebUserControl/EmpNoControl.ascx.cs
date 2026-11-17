using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class WebUserControl_EmpNoControl : System.Web.UI.UserControl
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
    public string GetEmpNo
    {
        get
        {
            return this.tbEmpNo.Text;
        }
        set
        {
            this.tbEmpNo.Text = value;
        }
    }    

    public void BoundData()
    {
        //tbEmpNo.Text = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd");

        //tbEmpNo.Attributes.Add("readonly", "readonly");
       
    }
}