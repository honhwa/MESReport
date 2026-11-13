<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DailyInputAndOutput.aspx.cs" Inherits="WebReport.Reports.DailyInputAndOutput" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>


<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DailyInputAndOutput</title>
    <link href="../Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body topmargin=0 leftmargin=0 background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%" cellspacing="0">
                <tr>

                    <td style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        background-repeat: repeat-y; width: 448px;">
                        <div id="filter">
                            <table style="vertical-align: text-bottom; width: 775px; text-align: center; height: 68px;">
                                <tr>
                                    <td style="width: 56px; height: 24px">
                                        年份</td>
                                    <td style="width: 46px; text-align: left; height: 24px;">
                                        <asp:DropDownList ID="dlYear" runat="server" Width="131px">
                                            <asp:ListItem>2008</asp:ListItem>
                                            <asp:ListItem>2009</asp:ListItem>
                                            <asp:ListItem>2010</asp:ListItem>
                                            <asp:ListItem>2011</asp:ListItem>
                                            <asp:ListItem>2012</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td style="width: 75px; height: 24px">
                                        月份</td>
                                    <td style="width: 18px; text-align: left; height: 24px;"><asp:DropDownList ID="dlMonth" runat="server" Width="131px">
                                        <asp:ListItem>01</asp:ListItem>
                                        <asp:ListItem>02</asp:ListItem>
                                        <asp:ListItem>03</asp:ListItem>
                                        <asp:ListItem>04</asp:ListItem>
                                        <asp:ListItem>05</asp:ListItem>
                                        <asp:ListItem>06</asp:ListItem>
                                        <asp:ListItem>07</asp:ListItem>
                                        <asp:ListItem>08</asp:ListItem>
                                        <asp:ListItem>09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList></td>
                                    <td style="width: 54px; height: 24px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 56px; height: 31px;">
                                        群組</td>
                                    <td style="width: 46px; text-align: left; height: 31px;">
                                        <asp:DropDownList ID="DlGroup" runat="server" Width="130px">
                                        </asp:DropDownList></td>
                                    <td style="width: 75px; height: 31px;">
                                        &nbsp;產品號碼</td>
                                    <td style="width: 18px; text-align: left; height: 31px;">
                                        <asp:DropDownList ID="DlProdID" runat="server" Width="129px">
                                        </asp:DropDownList></td>
                                    <td style="width: 54px; height: 31px;">
                                        <asp:Button ID="btnSerch" runat="server" OnClick="btnSerch_Click" Text="查詢" /></td>
                                </tr>
                            </table>
                        </div>
                        <div id="report" style="height: 476px; width: 99%;">
                            &nbsp;<asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label>&nbsp;
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="774px">
                            </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="774px">
                            </rsweb:ReportViewer>
                        </div>
                    </td>
                </tr>
                </table>
        </div>
    </form>
</body>
</html>
