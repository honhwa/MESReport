using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

public partial class WebUserControl_OneWeekControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BoundData();
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
    public string YearWeekFormatString
    {
        get
        {
            return this.tbFormat.Text;
        }
        set
        {
            this.tbFormat.Text = value;
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

    public void BoundData()
    {
        int intCnt = 2;
        DateTime dtNow = DateTime.Now.AddHours(-8);
        ModuleClass mc = new ModuleClass();
        ddlValue.Items.Clear();
        for (int i = 0; i < intCnt * 52; i++)
        {
            int intDay = mc.GetNumWeek(Convert.ToInt32(dtNow.DayOfWeek));
            string strFromDate = "", strToDate = "";
            strFromDate = dtNow.AddDays(-1 * 7 * i - intDay - 6 + 7).ToString("MM/dd");
            strToDate = dtNow.AddDays(-1 * 7 * i - intDay + 7).ToString("MM/dd");

            ListItem lt = new ListItem();
            lt.Text = mc.GetYearWeekFormatString(tbFormat.Text, mc.GetYearWeek(dtNow.AddDays(-1 * 7 * i))) + " (" + strFromDate + "~" + strToDate + ")";
            lt.Value = mc.GetYearWeek(dtNow.AddDays(-1 * 7 * i));
            ddlValue.Items.Add(lt);
        }
    }
}