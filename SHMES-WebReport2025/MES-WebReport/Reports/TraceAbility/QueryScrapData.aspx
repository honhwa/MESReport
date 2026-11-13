<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryScrapData.aspx.cs" Inherits="WebReport.Reports.QueryScrapData" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd '</script>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 699px; height: 294px" border="1" bordercolor="#cccc33">
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 7px">
                </td>
                <td style="width: 69px; font-family: Times New Roman; height: 7px">
                </td>
                <td colspan="3" style="font-family: Times New Roman; height: 7px; text-align: center">
                    <span style="font-size: 14pt"><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
                        Query </strong><span style="font-size: 16pt">Scrap Infomation</span><span><br />
                        </span>
                        <br />
                    </span></td>
            </tr>
            <tr>
                 <td style="vertical-align: middle; width: 23px; height: 1px">
                    </td>
                <td style="vertical-align: middle; width: 69px; height: 1px">
                    <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center">
                        <span style="font-family: Arial"><strong>開始時間</strong>&nbsp;</span></div>
                </td>
                <td style="width: 134px; font-family: Times New Roman; height: 1px">
                    <div id="timePickerLeftPicker" class="timePicker" style="width: 179px; height: 27px">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="142px" Font-Size="11pt" Height="18px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                </td>
                <td style="font-family: Times New Roman; height: 1px; width: 68px;">
                    <div id="timePickerLabelRight" class="LabelText">
                        <strong>結束時間</strong></div>
                </td>
                <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 1px">
                    <div id="timePickerRightPicker" class="timePicker" style="width: 237px; height: 19px;">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="155px" Font-Size="11pt"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                    </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 25px">
                    </td>
                <td colspan="4" style="height: 25px">
                <span style="font-size: 11pt"><asp:CheckBox ID="ChkProDID" runat="server"
                    OnCheckedChanged="ChkProDID_CheckedChanged" Text="PRODID:" AutoPostBack="True" Font-Bold="True" />
                    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:DropDownList ID="DDProdID" runat="server" Enabled="False" Width="170px">
                    </asp:DropDownList>
                    </span></td>
            </tr>
             <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td style="height: 27px; text-align: center" colspan="4">
                    <div id="Div1" style="text-align: left">
                        <asp:CheckBox ID="ChkWorkCenter" runat="server" OnCheckedChanged="ChkWorkCenter_CheckedChanged"
                            Text="WorkStation:" AutoPostBack="True" Font-Bold="True" />
                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:DropDownList ID="DDWorkCenter" runat="server" Enabled="False"
                            Width="170px">
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp;&nbsp;&nbsp;<asp:Button ID="ButQuery" runat="server" Font-Bold="True"
                        Font-Size="14pt" OnClick="ButQuery_Click" Text="查詢" Height="29px" Width="61px" /></div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 8px">
                </td>
                <td colspan="4" style="height: 8px; text-align: left">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td colspan="4" style="height: 27px">
                        <asp:Label ID="lblErrMsg" runat="server" Font-Underline="True" ForeColor="Red" Width="362px" Font-Names="Arial" Font-Size="Small"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</td>
            </tr>
        </table>    
    </div>
        &nbsp; &nbsp; &nbsp;
&nbsp;<table style="left: 29px; position: relative; top: -11px">
            <tr>
                <td style="width: 100px; height: 19px">
                    <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="385px" Width="789px">
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="385px" Width="789px">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
