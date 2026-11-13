<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NForceSwomQuery.aspx.cs" Inherits="WebReport.Reports.PC.NForceSwomQuery" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>二段式 Module 未結案 Report</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
			<table align="center" border="0" style="width: 460px; height: 90px">
					<tr>
						<td style="HEIGHT: 48px; width: 503px;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 503px; height: 6px;" align="center"><asp:image id="Image1" runat="server" ImageUrl="~/Images/line.gif" Height="4px" Width="500px"></asp:image></td>
					</tr>
					<tr>
						<td style="WIDTH: 503px; HEIGHT: 64px" align="center">
							<P><asp:label id="Label3" runat="server" Height="24px" Width="448px" ForeColor="DodgerBlue" Font-Size="X-Large"
									Font-Names="Arial">CELL2結案 待下Module工單產品</asp:label></P>
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 503px; height: 5px;" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image4" runat="server" ImageUrl="~/Images/line.gif" Height="4px" Width="500px"></asp:image></P>
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 503px; height: 18px;" align="center">
						</td>
					</tr>
		    </table>
		   <table id="Table4" style="WIDTH: 250px; HEIGHT: 100px" align="center" border="0">
					<tr>
						<td style="WIDTH: 85px; HEIGHT: 14px" align="center"><asp:label id="Label7" runat="server" Width="72px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">Frm Time:</asp:label></td>
						<td style="HEIGHT: 13px" align="left">
                                       <div id="timePickerLeftPicker" class="timePicker" style="width: 150px; height: 30px">
                                               
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromDate, dateFormat,-1,-1,false);"
                                                type="button" /></div> 
						</td>
					</tr>
					<tr>
						<td style="WIDTH: 85px; HEIGHT: 14px" align="center"><asp:label id="Label6" runat="server" Width="56px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">To Time:</asp:label></td>
						<td style="HEIGHT: 13px" align="left">
                                    <div id="Div1" class="timePicker" style="width: 150px; height: 30px">
                                               
                                        <asp:TextBox ID="txtToDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false);"
                                                type="button" /></div>
						</td>
					</tr>
					<tr>
					    <td colspan="2"></td>
					</tr>
					<tr>
					    <td colspan="2">
					    <asp:Button ID="btnSerch" runat="server" Text="查詢" OnClick="btnSerch_Click" Width="60px" />
                            &nbsp;&nbsp;</td>
					</tr>
					<tr>
					    <td colspan="2">
					    <asp:Label ID="lblErrMsg" runat="server" Width="196px" ForeColor="Red"></asp:Label></td>
					</tr>
			</table>    
        <table >
            <tr >
            <td style="height: 352px; width: 70px;">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            </td>
                <td style="width: 900px; height: 352px;">
                   <%--<rsweb:reportviewer id="ReportViewer" runat="server" width="886px" Height="333px"></rsweb:reportviewer>--%>
					<rsweb:ReportViewer ID="ReportViewer" runat="server"  width="886px" Height="333px"></rsweb:ReportViewer>
                </td>
            </tr>
        </table>.
    </form>
</body>
</html>
