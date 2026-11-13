<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMDDQuery.aspx.cs" Inherits="WebReport.Reports.MFG.RMDDQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>生產會報表查詢功能</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>    
</head>
<body style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div >
        <table style="width: 828px; height: 30px; text-align: center">
            <tr>
                <td>
                    生產日報查詢(Module)</td>
            </tr>
        </table>
        <table style="width: 831px; text-align: right">
            <tr>
                <td style="height: 16px; width: 120px;">
                    開始日期</td>
                <td style="height: 16px; text-align: left; width: 242px;">
                    <div id="timePickerLeftPicker" class="timePicker">
                        &nbsp;<asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromTime, dateFormat,-1,-1,false);"
                            type="button" />
                    </div>
                </td>
                <td style="width: 221px; height: 16px">
                    結束日期</td>
                <td style="width: 359px; height: 16px">
                    <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="126px"></asp:TextBox><input id="Button1" class="CalendarBotton" onclick="popUpCalendar(this, txtToTime, dateFormat,-1,-1,false);"
                            type="button" /></td>
            </tr>
            <tr>
                <td style="width: 120px; height: 16px">
                    Team模式</td>
                <td style="width: 242px; height: 16px; ">
                    <asp:DropDownList ID="dlTeamMode" runat="server" Width="158px">
                    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem Value="A">A</asp:ListItem>
                    <asp:ListItem Value="B">B</asp:ListItem>
                </asp:DropDownList></td>
                <td style="width: 221px; height: 16px">
                        批狀態</td>
                <td style="width: 359px; height: 16px">
                    <asp:DropDownList ID="DlLotType" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 19px; width: 120px;">
                        班別型態</td>
                <td style="height: 19px; width: 242px;">
                    <div id="prodIdDropDownList">
                        &nbsp;<asp:DropDownList ID="dlShiftID" runat="server" Width="158px">
                            <asp:ListItem Value="D">白班</asp:ListItem>
                            <asp:ListItem Value="N">晚班</asp:ListItem>
                            <asp:ListItem Value="ALL">白晚班</asp:ListItem>
                        </asp:DropDownList></div>
                </td>
                <td style="width: 221px; height: 19px;">
                        Team號碼</td>
                <td style="width: 359px; height: 19px;">
                    <asp:DropDownList ID="DlTeamID" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 14px; width: 120px;">
                    站點</td>
                <td style="height: 14px; width: 242px;">
                    <div id="partIdList">
                    <asp:DropDownList ID="DLWorkctr" runat="server" Width="154px">
                </asp:DropDownList>&nbsp;</div>
                </td>
                <td style="width: 221px; height: 14px">
                        機種群組</td>
                <td style="width: 359px; height: 14px">
                    <asp:DropDownList ID="DlProdGroup" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 11px; width: 120px;">
                        產線</td>
                <td style="height: 11px; width: 242px;">
                    <div id="subLineList">
                        <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                            z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                            width: 161px; border-bottom: black 1px solid; top: 78px; height: 61px; text-align: left">
                            <asp:CheckBox ID="chkAllSubLineIDs" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllSubLineIDs_CheckedChanged"
                                Text="ALL" Width="100%" />
                            <asp:CheckBoxList ID="lstSubLineId" runat="server" AutoPostBack="True" Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </td>
                <td style="width: 221px; height: 11px">
                        機種</td>
                <td style="width: 359px; height: 11px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 62px; text-align: left">
                        <asp:CheckBox ID="chkAllProd" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProd_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProd" runat="server" AutoPostBack="True" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="height: 20px; width: 120px;">
                        </td>
                <td style="height: 20px; width: 242px;">
                    </td>
                <td style="width: 221px; height: 20px">
                </td>
                <td style="width: 359px; height: 20px">
                    <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢報表" /></td>
            </tr>
            <tr>
            </tr>
        </table>
        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label><br />
<%--        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="831px">
        </rsweb:ReportViewer>--%>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server"  Width="831px">
        </rsweb:ReportViewer>
        &nbsp;
    
        
    
    </div>
    </form>
</body>
</html>
