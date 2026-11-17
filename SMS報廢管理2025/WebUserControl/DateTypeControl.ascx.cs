using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

public partial class WebUserControl_MonthControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblDateType.Text = rblDateType.SelectedValue.ToString();
            BoundData(rblDateType.SelectedValue);
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
    /// <summary>
    /// FromMohth
    /// </summary>
    public string DateTypeValue
    {
        get
        {
            return this.rblDateType.SelectedValue;
        }
        set
        {
            this.rblDateType.SelectedValue = value;
        }
    }
    [Browsable(true)]
    /// <summary>
    /// FromMohth
    /// </summary>
    public string FromMohth
    {
        get
        {
            return this.ddlFrom.SelectedValue.ToString();
        }
        set
        {
            this.ddlFrom.SelectedValue = value;
        }
    }
    [Browsable(true)]
    /// <summary>
    /// FromMohth
    /// </summary>
    public string ToMohth
    {
        get
        {
            return this.ddlTo.SelectedValue.ToString();
        }
        set
        {
            this.ddlTo.SelectedValue = value;
        }
    }

    public void BoundData(string strDateType)
    {
        int intCnt = 2;
        DateTime dtNow=DateTime.Now.AddHours(-8);
        ModuleClass mc = new ModuleClass();
        ddlFrom.Items.Clear();
        ddlTo.Items.Clear();
        if (strDateType == "Month")
        {
            for (int i = 0; i < intCnt * 12; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = dtNow.AddMonths(-1 * i).ToString("yyyy-MM");
                lt.Value = dtNow.AddMonths(-1 * i).ToString("yyyy-MM");
                ddlFrom.Items.Add(lt);
                ddlTo.Items.Add(lt);
            }
        }
        if (strDateType == "Week")
        {
            for (int i = 0; i < intCnt * 52; i++)
            {
                int intDay = mc.GetNumWeek(Convert.ToInt32(dtNow.DayOfWeek));
                string strFromDate = "", strToDate = "";
                strFromDate = dtNow.AddDays(-1 * 7 * i - intDay - 6 + 7).ToString("MM/dd");
                strToDate = dtNow.AddDays(-1 * 7 * i - intDay+7).ToString("MM/dd");

                ListItem lt = new ListItem();
                lt.Text = mc.GetYearWeekFormatString(tbFormat.Text, mc.GetYearWeek(dtNow.AddDays(-1 * 7 * i))) + " (" + strFromDate + "~" + strToDate + ")";
                lt.Value = mc.GetYearWeek(dtNow.AddDays(-1 * 7 * i));
                ddlFrom.Items.Add(lt);
                ddlTo.Items.Add(lt);
            }
        }
        if (strDateType == "Date")
        {
            for (int i = 0; i < intCnt * 365; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = dtNow.AddDays(-1 * i).ToString("yyyy-MM-dd") + " (" + mc.GetWeek(dtNow.AddDays(-1 * i)) + ")";
                lt.Value = dtNow.AddDays(-1 * i).ToString("yyyy-MM-dd");
                ddlFrom.Items.Add(lt);
                ddlTo.Items.Add(lt);
            }
        }
    }
    protected void rblDateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblDateType.Text = rblDateType.SelectedValue.ToString();
        BoundData(rblDateType.SelectedValue);
    }
}