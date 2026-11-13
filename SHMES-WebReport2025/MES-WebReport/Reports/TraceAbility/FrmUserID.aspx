<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUserID.aspx.cs" Inherits="WebReport.Reports.FrmUserID" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table border="1" bordercolor="#cccccc" style="width: 882px; height: 167px">
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 90px">
                </td>
                <td style="width: 130px; font-family: Times New Roman; height: 90px">
                </td>
                <td colspan="4" style="font-family: Times New Roman; height: 90px; text-align: center">
                    <span><span><span style="font-size: 14pt"><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<span style="font-size: 25px"><span>Operator與SN Mapping查詢</span></span></strong></span></span></span></td>
            </tr>
            <tr style="font-size: 12pt">
                 <td style="vertical-align: middle; width: 23px; height: 50px">
                    </td>
                <td style="vertical-align: middle; width: 130px; height: 50px">
                    <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center; width: 115px; height: 22px;">
                        <span style="font-family: Arial"><strong>&nbsp;&nbsp; 開始時間</strong>&nbsp;</span></div>
                </td>
                <td style="width: 127px; font-family: Times New Roman; height: 50px">
                    <div id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="122px" Height="18px"></asp:TextBox><input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                            type="button" /></div>
                </td>
                <td style="font-family: Times New Roman; height: 50px; width: 67px;">
                    <div id="timePickerLabelRight" class="LabelText">
                        <strong>&nbsp; &nbsp; 結束時間</strong></div>
                </td>
                <td style="font-size: 12pt; width: 112px; font-family: Times New Roman; height: 50px">
                    <div id="timePickerRightPicker" class="timePicker">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px" Font-Size="Smaller"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                            type="button" /></div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px;">
                    </td>
                <td style="width: 130px; text-align: center">
                <span style="font-size: 11pt">&nbsp; &nbsp;
                    <asp:CheckBox ID="ChkSubLine" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Size="Larger" OnCheckedChanged="ChkSubLine_CheckedChanged1" Text="SubLineID" /></span></td>
                <td style="width: 127px;">
                    <div id="partIdList">
                        <asp:DropDownList ID="DDLineId" runat="server"
                            Font-Size="Smaller" Width="148px">
                        </asp:DropDownList>&nbsp;</div>
                </td>
                <td style="text-align: center; width: 67px;">
                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Size="Larger" Text="SN:" /></td>
                <td style="width: 112px;">
                    <asp:TextBox ID="txtSn" runat="server" Font-Size="Smaller"></asp:TextBox></td>
                <td style="text-align: center; width: 60px;">
                    </td>
                <td style="width: 173px;">
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td style="vertical-align: middle; width: 23px; height: 1px">
                </td>
                <td style="width: 130px; height: 1px; text-align: center">
                    <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Size="Larger" OnCheckedChanged="CheckBox2_CheckedChanged1" Text="UserID:" /></td>
                <td style="width: 127px; height: 1px">
                    <div id="Div1">
                        <asp:TextBox ID="txtUid" runat="server"
                            Font-Size="Smaller" Width="145px"></asp:TextBox>&nbsp;</div>
                </td>
                <td style="height: 1px; text-align: center; width: 67px;">
                    </td>
                <td style="width: 112px; height: 1px">
                    <asp:Button ID="Button1" runat="server"
                        Font-Bold="True" Font-Size="Larger" OnClick="Button1_Click1" Text="查詢" /></td>
                <td style="height: 1px; text-align: center; width: 60px;">
                    </td>
                <td style="width: 173px; height: 1px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="vertical-align: middle; width: 23px; height: 27px">
                </td>
                <td colspan="4" style="height: 27px">
                        <asp:Label ID="lblErr" runat="server" Font-Underline="True" ForeColor="Red" Width="331px" Font-Names="Arial" Font-Size="Small"></asp:Label>
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
     <table style="width: 977px" >
            <tr >
                <td style="width: 21px; height: 428px;">
                </td>
                <td style="width: 803px; height: 428px;">
<%--                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="522px" Width="782px" Font-Names="Verdana" Font-Size="8pt">
                    </rsweb:ReportViewer>--%>
                    <br />
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="522px" Width="782px" Font-Names="Verdana" Font-Size="8pt">
                    </rsweb:ReportViewer>
                </td>
        </tr>
          </table>
    </form>
</body>
</html>
