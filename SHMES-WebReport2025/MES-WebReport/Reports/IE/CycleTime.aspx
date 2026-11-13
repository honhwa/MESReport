<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CycleTime.aspx.cs" Inherits="WebReport.Reports.CycleTime" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CycleTimeReport</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css"  rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            <div>
                <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                    margin: 0px; width: 100%; padding-top: 0px">
                    <tr>
                        <td rowspan="2" style="background-position-x: center;
                            width: 496px; background-repeat: repeat-y; height: 349px;">
                            <div id="filter">
                                <table style="width: 875px">
                                    <tr>
                                        <td style="vertical-align: middle; width: 1px; height: 28px">
                                        </td>
                                        <td style="vertical-align: middle; width: 73px; height: 28px">
                                        </td>
                                        <td style="width: 149px; font-family: Times New Roman; height: 28px; text-align: left">
                                        </td>
                                        <td colspan="2" style="font-family: Times New Roman; height: 28px; text-align: center;">
                                            <span style="font-size: 14pt">C<strong>ycleTime報表</strong></span></td>
                                        <td colspan="1" style="width: 56px; font-family: Times New Roman; height: 28px; text-align: center">
                                        </td>
                                        <td colspan="1" style="font-family: Times New Roman; height: 28px; text-align: center; width: 199px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: middle; width: 1px; height: 19px">
                                        </td>
                                        <td style="vertical-align: middle; width: 73px; height: 19px">
                                                <div id="Div1" class="LabelText" style="vertical-align: middle; text-align: center">
                                                    <span style="font-family: Arial"><strong>時間範圍</strong>&nbsp;</span></div>
                                        </td>
                                        <td style="width: 149px; font-family: Times New Roman; height: 19px; text-align: left">
                                            <asp:RadioButtonList ID="rboDateScope" runat="server" AutoPostBack="True">
                                                <asp:ListItem Selected="True">Month</asp:ListItem>
                                                <asp:ListItem>Week</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="font-family: Times New Roman; height: 19px; width: 65px;">
                                            開始<br />
                                            時間</td>
                                        <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 19px">
                                            <div id="timePickerLeftPicker" class="timePicker">
                                                <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                                <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                                                    type="button" /></div>
                                        </td>
                                        <td style="font-size: 12pt; width: 56px; font-family: Times New Roman; height: 19px">
                                            結束<br />
                                            時間</td>
                                        <td style="font-size: 12pt; width: 199px; font-family: Times New Roman; height: 19px">
                                            <div id="timePickerRightPicker" class="timePicker">
                                                <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                                <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                                                    type="button" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: middle; width: 1px; height: 28px">
                                        </td>
                                        <td style="vertical-align: middle; width: 73px; height: 28px">
                                            Lot Type</td>
                                        <td style="width: 149px; font-family: Times New Roman; height: 28px">
                                            <asp:DropDownList ID="dlLOTTYPE" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="157px">
                                            </asp:DropDownList></td>
                                        <td style="font-family: Times New Roman; height: 28px; width: 65px;">
                                            WO Type</td>
                                        <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 28px">
                                                <asp:DropDownList ID="cmbWOType" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="157px">
                                                </asp:DropDownList></td>
                                        <td style="font-size: 12pt; width: 56px; font-family: Times New Roman; height: 28px">
                                            機種</td>
                                        <td style="font-size: 12pt; width: 199px; font-family: Times New Roman; height: 28px">
                                                <asp:DropDownList ID="cmbProductID" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="157px">
                                                </asp:DropDownList></td>
                                    </tr>
                                    <tr style="font-size: 12pt; font-family: Times New Roman">
                                        <td style="width: 1px; text-align: left">
                                        </td>
                                        <td style="width: 73px; text-align: center">
                                            <span>Cell2
                                                <br />
                                                站點</span></td>
                                        <td style="width: 149px">
                                            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                                width: 170px; border-bottom: black 1px solid; top: 78px; height: 156px">
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                                                    Text="ALL" Width="100%" />
                                                <asp:CheckBoxList ID="lstStation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstStation_SelectedIndexChanged"
                                                    Width="100%">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td style="height: 34px; text-align: left; width: 65px;">
                                            Module<br />
                                            站點</td>
                                        <td style="width: 149px; height: 34px">
                                            <br /><div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                                width: 176px; border-bottom: black 1px solid; top: 78px; height: 156px">
                                                <asp:CheckBox ID="CheckModuleALL" runat="server" AutoPostBack="True" OnCheckedChanged="CheckModuleALL_CheckedChanged"
                                                    Text="ALL" Width="100%" />
                                                <asp:CheckBoxList ID="lstModuleStation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstModuleStation_SelectedIndexChanged"
                                                    Width="100%">
                                                </asp:CheckBoxList>
                                            </div>
                                            </td>
                                        <td style="width: 56px; height: 34px">
                                            Lot Size<br />
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                        <td style="width: 199px; height: 34px">
                                            <asp:DropDownList ID="cmbLotSize" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                Font-Size="10pt" Width="160px">
                                                <asp:ListItem Value="0">ALL</asp:ListItem>
                                                <asp:ListItem Value="1">0~100</asp:ListItem>
                                                <asp:ListItem Value="2">100~300</asp:ListItem>
                                                <asp:ListItem Value="3">&gt;=300</asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <br />
                                            <br />
                                            <asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt"
                                                OnClick="btnQuery_Click" Text="查詢報表" /></td>
                                    </tr>
                                    <tr style="font-size: 12pt; font-family: Times New Roman">
                                        <td colspan="1" style="width: 1px">
                                        </td>
                                        <td colspan="4">
                                            <div id="ErrMsg" style="text-align: center">
                                                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman" Font-Size="11pt"></asp:Label>
                                            </div>
                                        </td>
                                        <td colspan="1" style="width: 56px">
                                        </td>
                                        <td colspan="1" style="width: 199px">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    
    </div>
                            <%--<rsweb:reportviewer id="reportViewer" runat="server" width="888px" Height="339px"></rsweb:reportviewer>--%>
        <rsweb:ReportViewer ID="reportViewer" runat="server" width="888px" Height="339px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
