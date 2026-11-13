<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EQ24HRSTATUSQuery.aspx.cs" Inherits="WebReport.Reports.EMS.EQ24HRSTATUSQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EQ前24小時狀態查詢</title>
</head>
<body topmargin=0 leftmargin=0 style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            &nbsp;</div>
    
    </div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 103%; padding-top: 0px; height: 506px">
                <tr>
                    <td rowspan="2" style="background-image: none; text-align: left; background-color: transparent; width: 1024px;">
                        <div id="filter">
                            <table>
                                <tr>
                                    <td style="height: 40px">
                                        <div id="fromStationLabel" class="LabelText">
                                            &nbsp;</div>
                                    </td>
                                    <td style="width: 4px; height: 40px;">
                                        <div id="prodIdDropDownList">
                                            &nbsp;</div>
                                    </td>
                                    <td style="height: 40px">
                                        <div id="subLineLabel" class="LabelText">
                                            SubLine<br />
                                            (For EQ Query)
                                        </div>
                                    </td>
                                    <td style="width: 102px; height: 40px;">
                                        <asp:DropDownList ID="cmbSubLine" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSubLine_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="width: 4px">
                                        <div id="partIdList">
                                            &nbsp;</div>
                                    </td>
                                    <td colspan="2" style="text-align: right">
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div id="ErrMsg">
                                            <asp:Label ID="lblErrMsg" runat="server"></asp:Label></div>
                                    </td>
                                </tr>
                            </table>
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                                                    Text="ALL" Width="100%" Visible="False" />
                                                </div>
                        <div id="report">
                        <%--<rsweb:ReportViewer ID="reportViewer" runat="server" Width="856px">
                        </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="reportViewer" runat="server" Width="856px">
                            </rsweb:ReportViewer>
                    </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 415px; background-color: #3c2679; width: 43px;">
                    </td>
                </tr>
                </table>
    </form>
</body>
</html>
