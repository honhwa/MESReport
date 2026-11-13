<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SPCCPKData.aspx.cs" Inherits="WebReport.Reports.SPC.SPCCPKData" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        &nbsp;</div>
        <table style="width: 921px; height: 14px">
            <tr>
                <td align="center" colspan="2" style="height: 41px">
                    <strong><span style="font-size: 16pt">SPC CPK Data Info</span></strong></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkFabID" runat="server" Text="FabID" Width="102px" OnCheckedChanged="ChkFabID_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlFabID" runat="server" Width="170px" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkTime" runat="server" Text="Time" Width="100px" OnCheckedChanged="ChkTime_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlTime" runat="server" Width="170px" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 14px; height: 16px;" align="left">
                    <asp:CheckBox ID="ChkDateType" runat="server" Text="DateType" Width="100px" OnCheckedChanged="ChkDateType_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 16px;">
                    <asp:DropDownList ID="ddlType" runat="server" Width="170px" Enabled="False">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="ButQuery" runat="server" Text="查詢" Width="101px" Height="26px" OnClick="ButQuery_Click" />&nbsp;
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="20px" ImageUrl="~/Images/xls.jpg"
                        OnClick="ImageButton1_Click" Visible="False" Width="25px" AlternateText="匯Excel" /></td>
            </tr>
        </table>
        <table style="width: 799px">
            <tr>
                <td style="width: 98px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="912px" Visible="False"></asp:Label></td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
<%--        <rsweb:reportviewer id="reportViewer" runat="server" height="454px" width="884px">
                            </rsweb:reportviewer>--%>
        <rsweb:ReportViewer ID="reportViewer" runat="server" height="454px" width="884px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>