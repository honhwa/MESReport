<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NonKeyMaterialTransferQuery.aspx.cs" Inherits="WebReport.Reports.NonKeyMaterialTransferQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>耗材耗用量查詢</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 100%; padding-top: 0px">
                <tr>
                    <td style="background-position-x: left; background-image: url(../../Images/right_bg.jpg);
                        width: 12px; background-repeat: repeat-y">
                    </td>
                    <td rowspan="2" style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                        width: 765px; background-repeat: repeat-y">
                        <div id="filter">
                            <table>
                                <tr>
                                    <td style="vertical-align: middle; width: 73px; height: 27px">
                                    </td>
                                    <td style="width: 158px; font-family: Times New Roman; height: 27px">
                                    </td>
                                    <td style="font-family: Times New Roman; height: 27px; text-align: center;" colspan="2">
                                        <strong><span style="font-size: 14pt">NonKey耗用量查詢</span></strong></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle; width: 73px; height: 27px">
                                        <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center">
                                            <span style="font-family: Arial"><strong>開始時間</strong>&nbsp;</span></div>
                                    </td>
                                    <td style="width: 158px; font-family: Times New Roman; height: 27px">
                                        <div id="timePickerLeftPicker" class="timePicker">
                                            <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                                                type="button" /></div>
                                    </td>
                                    <td style="font-family: Times New Roman; height: 27px">
                                        <div id="timePickerLabelRight" class="LabelText">
                                            <strong>結束時間</strong></div>
                                    </td>
                                    <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 27px">
                                        <div id="timePickerRightPicker" class="timePicker">
                                            <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                                                type="button" /></div>
                                    </td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td style="width: 73px; height: 34px; text-align: center">
                                        WorkCtr</td>
                                    <td style="width: 158px; height: 34px">
                                        <div id="partIdList">
                                            <asp:DropDownList ID="cmbWorkctr" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                Font-Size="10pt" Width="157px" OnSelectedIndexChanged="cmbWorkctr_SelectedIndexChanged">
                                            </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="height: 34px; text-align: center">
                                        Sub Line</td>
                                    <td style="width: 149px; height: 34px">
                                        <asp:DropDownList ID="cmbSubLine" runat="server" AutoPostBack="True" Font-Names="Arial"
                                            Font-Size="10pt" Width="160px" OnSelectedIndexChanged="cmbSubLine_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td style="width: 73px; height: 33px; text-align: center">
                                        Product ID</td>
                                    <td style="width: 158px; height: 33px">
                                        <div id="subLineList">
                                            <asp:DropDownList ID="cmbProductID" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                Font-Size="10pt" Width="157px" OnSelectedIndexChanged="cmbProductID_SelectedIndexChanged">
                                            </asp:DropDownList>&nbsp;</div>
                                    </td>
                                    <td style="height: 33px; text-align: center">
                                        Part ID</td>
                                    <td style="width: 149px; height: 33px">
                                        <asp:DropDownList ID="cmbPartlID" runat="server" AutoPostBack="True" Font-Names="Arial"
                                            Font-Size="10pt" Width="160px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td colspan="4" style="text-align: right">
                                        <asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt" Text="查詢報表" OnClick="btnQuery_Click" /></td>
                                </tr>
                                <tr style="font-size: 12pt; font-family: Times New Roman">
                                    <td colspan="4">
                                        <div id="ErrMsg" style="text-align: center">
                                            <asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman" Font-Size="11pt"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="report" style="font-family: Times New Roman">
                            <%--<rsweb:ReportViewer ID="reportViewer" runat="server" Height="454px" Width="884px">
                            </rsweb:ReportViewer>--%>

                            <rsweb:ReportViewer ID="reportViewer" runat="server" Height="454px" Width="884px">
                            </rsweb:ReportViewer>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
