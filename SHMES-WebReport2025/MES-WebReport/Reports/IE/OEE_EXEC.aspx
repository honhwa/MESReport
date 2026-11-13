<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OEE_EXEC.aspx.cs" Inherits="WebReport.Reports.OEE_EXEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
    
<%@ Register TagPrefix="uc" TagName="datepicker"Src="WebUserControl1.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OEE 手動執行</title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">dateFormat = 'yyyy/mm/dd'</script>
    <script src="popcalendar.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>       
        <table style="width: 600px; height: 185px">
            <tr>
            <td colspan="3" align ="center" ><asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Manual       OEE " Font-Names="標楷體" Width="206px"></asp:Label></td>
            </tr>
             <tr style="height :30px">
            <td colspan="3"   ></td>
            </tr>
            <tr>
                <td style="width: 200px"  align ="right"  >
                    <asp:Label ID="Label1" runat="server" Font-Size="Medium" ForeColor="Black" Text="請選擇需要執行的日期："
                         Font-Bold="True" Font-Names="標楷體" Height="40px"></asp:Label></td>
                <td><asp:TextBox ID="txtDT" runat="server" Width="100px"></asp:TextBox>
                        <input id="Button11" class="CalendarBotton" onclick="popUpCalendar(this, txtDT, dateFormat,-1,-1,false)"
                            style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                            background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                            width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" type="button" /></td>
            </tr>
            <tr>
            <td colspan ="3" align ="RIGHT" >
            &nbsp;
                                      &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="BtnOEEExec" runat="server" OnClick="BtnOEEExec_Click" Text="執行" Font-Bold="True" />
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnQueryLog" runat="server" Text="查詢" OnClick="btnQueryLog_Click" Font-Bold="True" />
            </td>
            </tr>
            <tr>
                <td style="width: 706px; height: 63px;" colspan="3">
                    &nbsp;<asp:Label ID="lblErrorCD" runat="server" ForeColor="Red" Text="lblErrorCD"
                        Width="423px" Visible="False"></asp:Label><br />
                    &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Text="lblERROR" Width="755px" Visible="False"></asp:Label><br />
                    &nbsp;<asp:Label ID="lbl_LOG_ERROR" runat="server" Text="lbl_LOG_ERROR" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3" >
                    <asp:Label ID="lblQueryError" runat="server" BackColor="White" ForeColor="Red"
                        Text="lblQueryError" Visible="False"></asp:Label></td>
            </tr>
            </table>
            <table style="width: 80%">
            <tr>
                <td colspan="3" style="width: 706px; height: 111px">
                    &nbsp;<br />
                    <%--<rsweb:reportviewer id="ReportViewer1" runat="server" visible="true" width="101%"> <LOCALREPORT DisplayName="OEE Report" /></rsweb:reportviewer>--%>


                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" visible="true" width="101%">
                        <LOCALREPORT DisplayName="OEE Report" />
                    </rsweb:ReportViewer>


                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
