<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EQParameter.aspx.cs" Inherits="WebReport.Reports.EQParameter" %>

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
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 880px; height: 124px" border="1" bordercolor="#cccc99">
            <tr>
                <td style="width: 80px; font-family: Times New Roman; height: 58px">
                </td>
                <td colspan="4" style="font-family: Times New Roman; height: 58px; text-align: center">
                    <span><span style="font-size: 14pt"><strong><span style="font-size: 20pt"> <span><span> Equipment Parameter Query</span></span></span></strong></span></span></td>
            </tr>
          <!--  <tr style="font-size: 12pt">
                 <td style="vertical-align: middle; width: 23px; height: 37px;">
                    </td>
                <td style="vertical-align: middle; width: 30px; height: 37px;">
                    <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center; width: 94px; height: 22px;">
                        <span style="font-family: Arial"><strong>
                            開始時間</strong>&nbsp;</span></div>
                </td>
                <td style="width: 142px; font-family: Times New Roman; height: 37px;">
                    <div id="timePickerLeftPicker" class="timePicker">
                        <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                </td>
                <td style="font-family: Times New Roman; width: 73px; height: 37px;">
                    <div id="timePickerLabelRight" class="LabelText" style="width: 69px; height: 19px">
                        <strong>結束時間</strong></div>
                </td>
                <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 37px;">
                    <div id="timePickerRightPicker" class="timePicker">
                        <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                            type="button" /></div>
                </td>
            </tr> --> 
            <tr>
                <td style="width: 80px; height: 32px; text-align: center">
                    <strong>開始時間:</strong></td>
                <td style="width: 167px; height: 32px">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="121px"></asp:TextBox>&nbsp;
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/calendar.gif"
                        OnClick="ImageButton1_Click" />
                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="Black"
                        BorderStyle="Solid" CellSpacing="1" FirstDayOfWeek="Sunday" Font-Names="Verdana"
                        Font-Size="9pt" ForeColor="Black" Height="61px" NextPrevFormat="FullMonth" OnSelectionChanged="Calendar1_SelectionChanged"
                        Visible="False" Width="178px">
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TodayDayStyle BackColor="#999999" ForeColor="White" />
                        <DayStyle BackColor="#CCCCCC" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                        <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt"
                            ForeColor="White" Height="12pt" />
                    </asp:Calendar>
                </td>
                <td style="width: 80px; height: 32px; text-align: center">
                    <strong>結束時間:</strong></td>
                <td style="width: 169px; height: 32px">
                    &nbsp;<asp:TextBox ID="txtToDate" runat="server" Width="124px"></asp:TextBox>
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/calendar.gif"
                        OnClick="ImageButton2_Click" />
                    <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="Black"
                        BorderStyle="Solid" CellSpacing="1" FirstDayOfWeek="Sunday" Font-Names="Verdana"
                        Font-Size="9pt" ForeColor="Black" Height="61px" NextPrevFormat="FullMonth" OnSelectionChanged="Calendar2_SelectionChanged"
                        Visible="False" Width="178px">
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TodayDayStyle BackColor="#999999" ForeColor="White" />
                        <DayStyle BackColor="#CCCCCC" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                        <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt"
                            ForeColor="White" Height="12pt" />
                    </asp:Calendar>
                </td>
            </tr>
            <tr>
                <td style="width: 80px; text-align: center; height: 32px;">
                <span style="font-size: 11pt"><asp:CheckBox ID="ChkPanelID" runat="server"
                    AutoPostBack="True" Font-Bold="True" Font-Size="Larger" Height="21px" OnCheckedChanged="ChkPanelID_CheckedChanged"
                    Text="PanelID:" Width="76px" /></span></td>
                <td style="width: 167px; height: 32px;">
          <!--          <div id="partIdList" style=" POSITION: absolute ; CLIP: rect(2px 260px 21px 120px); width: 124px;" >
                        <asp:DropDownList ID="DDLPanelID" runat="server" Enabled="False" Width="141px"  AutoPostBack="True">
                        </asp:DropDownList>&nbsp;</div>
            -->            
                     <div id="partIdText" style="POSITION: absolute;"> <asp:TextBox ID="TxtPanelID" runat="server" Width="116px" Enabled="False"  ></asp:TextBox></div> 
                </td>
                <td style="width: 72px; height: 32px">
                    <asp:CheckBox ID="ChkSN" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="Larger"
                        Height="3px" OnCheckedChanged="ChkSN_CheckedChanged" Text="SN:" Width="67px" /></td>
                <td style="text-align: left; width: 167px; height: 32px;">
                
        <!--             <div id="SNList" style=" POSITION: absolute ; CLIP: rect(2px 260px 21px 120px); width: 124px;" >
                        <asp:DropDownList ID="DDLSN" runat="server" Enabled="False" Width="141px" AutoPostBack="True" >
                        </asp:DropDownList>&nbsp;</div>
          -->              
                     <div id="SNText" style="POSITION: absolute;"> <asp:TextBox ID="TxtSN" runat="server" Width="116px" Enabled="False"  ></asp:TextBox></div> 
                </td>
               <td style="text-align: center; width: 16px;" rowspan="2">
                    </td>
            </tr>
             <tr>
                <td style="width: 80px; text-align: center">
                    </td>
                <td style="width: 167px;">
                    <div id="Div1">
                        &nbsp;</div>
                </td>
                 <td style="width: 72px">
                 </td>
                <td style="text-align: center; width: 169px;">
                <asp:Button  ID="ButQry" runat="server" Font-Bold="True" Font-Size="Larger" OnClick="ButQry_Click" Text="查詢">
                     </asp:Button></td>
            </tr>
            <tr>
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
                    <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="461px" Width="874px">
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="461px" Width="874px">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
