<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NonKeyMaterialExchangeQuery.aspx.cs" Inherits="WebReport.Reports.NonKeyMaterialExchangeQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>耗材交易查詢</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body style="font-family: Times New Roman">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 100%; padding-top: 0px">
                <tr>
                    <td rowspan="2" style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                        background-repeat: repeat-y; width: 761px;">
                        <div id="filter">
                            <table>
                                <tr>
                                    <td style="vertical-align: middle; width: 11px; height: 27px">
                                    </td>
                                    <td style="vertical-align: middle; width: 73px; height: 27px">
                                    </td>
                                    <td style="width: 158px; font-family: Times New Roman; height: 27px">
                                    </td>
                                    <td style="font-family: Times New Roman; height: 27px; text-align: center;" colspan="2">
                                        <strong><span style="font-size: 14pt">NonKey交易查詢</span></strong></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle; width: 11px; height: 27px">
                                    </td>
                                    <td style="vertical-align: middle; width: 73px; height: 27px">
                                        <div id="timePickerLabelLeft" class="LabelText" style="text-align: center; vertical-align: middle;">
                                            <span style="font-family: Arial"><strong>開始時間</strong>&nbsp;</span></div>
                                    </td>
                                    <td style="width: 158px; font-family: Times New Roman; height: 27px;">
                                        <div id="timePickerLeftPicker" class="timePicker">
                                            <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                                                type="button" /></div>
                                    </td>
                                    <td style="font-family: Times New Roman; height: 27px">
                                        <div id="timePickerLabelRight" class="LabelText">
                                            <strong>結束時間</strong></div>
                                    </td>
                                    <td style="width: 149px; font-size: 12pt; font-family: Times New Roman; height: 27px;">
                                        <div id="timePickerRightPicker" class="timePicker">
                                            <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                                                type="button" /></div>
                                    </td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td style="width: 11px; text-align: left">
                                    </td>
                                    <td style="width: 73px; text-align: center">
                                        <strong><span style="font-size: 11pt">交易類型</span></strong></td>
                                    <td style="width: 158px">
                                        <div id="prodIdDropDownList">
                                            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                                                width: 153px; border-bottom: black 1px solid; top: 78px; height: 72px">
                                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                                                    Text="ALL" Width="100%" />
                                                <asp:CheckBoxList ID="lstVerb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstVerb_SelectedIndexChanged"
                                                    Width="100%" Font-Names="Arial" Font-Size="10pt">
                                                    <asp:ListItem Value="W2MC">庫房發料</asp:ListItem>
                                                    <asp:ListItem Value="MC2S">發到產線</asp:ListItem>
                                                    <asp:ListItem Value="S2MC">產線退回</asp:ListItem>
                                                    <asp:ListItem Value="MC2Z">發到站點</asp:ListItem>
                                                    <asp:ListItem Value="Z2MC">站點退回</asp:ListItem>
                                                    <asp:ListItem Value="MC2W">NONKEY繳庫</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                    <td style="width: 149px">
                                        &nbsp;</td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td style="width: 11px; height: 34px; text-align: left">
                                    </td>
                                    <td style="width: 73px; height: 34px; text-align: center">
                                        WorkCtr</td>
                                    <td style="width: 158px; height: 34px;">
                                        <div id="partIdList">
                                            <asp:DropDownList ID="cmbWorkctr" runat="server" Width="157px" OnSelectedIndexChanged="cmbWorkctr_SelectedIndexChanged" Font-Names="Arial" Font-Size="10pt" AutoPostBack="True">
                                            </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="text-align: center; height: 34px;">
                                        Sub Line</td>
                                    <td style="width: 149px; height: 34px;">
                                        <asp:DropDownList ID="cmbSubLine" runat="server" Width="160px" OnSelectedIndexChanged="cmbSubLine_SelectedIndexChanged" Font-Names="Arial" Font-Size="10pt" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td style="width: 11px; height: 33px; text-align: left">
                                    </td>
                                    <td style="width: 73px; height: 33px; text-align: center">
                                        Product ID</td>
                                    <td style="width: 158px; height: 33px;">
                                        <div id="subLineList">
                                            <asp:DropDownList ID="cmbProductID" runat="server" Width="157px" OnSelectedIndexChanged="cmbProductID_SelectedIndexChanged" Font-Names="Arial" Font-Size="10pt" AutoPostBack="True">
                                            </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="text-align: center; height: 33px;">
                                        Part ID</td>
                                    <td style="width: 149px; height: 33px;">
                                        <asp:DropDownList ID="cmbPartlID" runat="server" Width="160px" Font-Names="Arial" Font-Size="10pt" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td colspan="1" style="width: 11px; text-align: right">
                                    </td>
                                    <td colspan="4" style="text-align: right">
                                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢報表" Font-Names="Times New Roman" Font-Size="10pt" /></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td colspan="1" style="width: 11px">
                                    </td>
                                    <td colspan="4">
                                        <div id="ErrMsg" style="text-align: center">
                                            <asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman" Font-Size="11pt"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="report" style="font-family: Times New Roman">
                            <%--<rsweb:ReportViewer ID="reportViewer" runat="server" Height="416px" Width="889px">
                            </rsweb:ReportViewer>--%>

                            <rsweb:ReportViewer ID="reportViewer" runat="server" Height="416px" Width="889px">
                            </rsweb:ReportViewer>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
