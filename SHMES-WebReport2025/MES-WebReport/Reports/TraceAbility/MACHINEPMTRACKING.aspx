<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmOQC.aspx.cs" Inherits="WebReport.Reports.MACHINE_PM_TRACKING" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
    <style type="text/css">
        .auto-style1 {
            height: 28px;
            width: 149px;
        }
        .auto-style2 {
            height: 46px;
            width: 149px;
        }
        .auto-style3 {
            height: 38px;
            width: 149px;
        }
    </style>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 880px; height: 124px" border="1" bordercolor="#cccc99">
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 58px">
                </td>
                <td style="width: 30px; font-family: Times New Roman; height: 58px">
                </td>
                <td colspan="4" style="font-family: Times New Roman; height: 58px; text-align: center">
                    <span><span style="font-size: 14pt"><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <span style="font-size: 20pt"> <span><span>MACHINE PM TRACKING</span></span></span></strong></span></span></td>
            </tr>
            <tr style="font-size: 12pt">
                 <td style="vertical-align: middle; width: 23px; height: 28px;">
                    </td>
                <td style="vertical-align: middle; width: 30px; height: 28px;">
                    <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center; width: 94px; height: 22px;">
                        <span style="font-family: Arial"><strong>
                            開始時間</strong>&nbsp;</span></div>
                </td>
                <td style="width: 142px; font-family: Times New Roman; height: 28px;">
                    <div id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                </td>
                <td style="font-family: Times New Roman; width: 73px; height: 28px;">
                    <div id="timePickerLabelRight" class="LabelText" style="width: 69px; height: 19px">
                        <strong>結束時間</strong></div>
                </td>
                <td style="font-size: 12pt; font-family: Times New Roman; " class="auto-style1">
                    <div id="timePickerRightPicker" class="timePicker">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 46px;">
                    </td>
                <td style="width: 30px; text-align: center; height: 46px;">
                <span style="font-size: 11pt"><asp:CheckBox ID="ChkWctr" runat="server"
                    AutoPostBack="True" Font-Bold="True" Font-Size="Larger" Height="21px" OnCheckedChanged="ChkWctr_CheckedChanged"
                    Text="WORKCTR:" Width="76px" /></span></td>
                <td style="width: 142px; height: 46px;">
                    <div id="partIdList">
                        <asp:DropDownList ID="DDLCTR" runat="server" Enabled="False" Width="173px" OnSelectedIndexChanged="ChkWctr_CheckedChanged" AppendDataBoundItems="True" AutoPostBack="True" DataTextField="workctr">
                        </asp:DropDownList>&nbsp;&nbsp;</div>
                </td>
                <td style="text-align: center; width: 73px; height: 46px;">
                    <asp:CheckBox ID="ChkSub" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="Larger"
                        Height="3px" OnCheckedChanged="ChkSub_CheckedChanged" Text="SUBLINE:" Width="67px" /></td>
                <td class="auto-style2">
                    <asp:DropDownList ID="DDLSUB" runat="server" Enabled="False" Width="170px">
                    </asp:DropDownList></td>
                <td style="text-align: center; width: 54px;" rowspan="2">
                    </td>
                <td style="width: 173px;" rowspan="2">
                    &nbsp; &nbsp;</td>
            </tr>
             <tr>
                <td style="vertical-align: middle; width: 23px; height: 38px;">
                </td>
                <td style="width: 30px; text-align: center; height: 38px;">
                    <asp:CheckBox ID="ChkSta" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="Larger"
                        OnCheckedChanged="ChkSta_CheckedChanged" Text="STATUS:" Width="67px" /></td>
                <td style="width: 142px; height: 38px;">
                    <div id="Div1">
                        <asp:DropDownList ID="DDLSTA" runat="server" Enabled="False" Width="141px">
                        </asp:DropDownList>&nbsp;</div>
                </td>
                <td style="text-align: center; width: 73px; height: 38px;">
                    </td>
                <td class="auto-style3">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button
                        ID="ButQry" runat="server" Font-Bold="True" Font-Size="Larger" OnClick="ButQry_Click"
                        Text="查詢" /></td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 21px">
                </td>
                <td colspan="4" style="height: 21px">
                    &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblErr" runat="server" Font-Underline="True" ForeColor="Red" Width="251px" Font-Names="Arial" Font-Size="Small"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
        </table>    
    </div>
        <table style="width: 998px; height: 401px">
            <tr>
                <td style="width: 100px">
<%--                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="461px" Width="961px">
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server"  Height="461px" Width="961px">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
