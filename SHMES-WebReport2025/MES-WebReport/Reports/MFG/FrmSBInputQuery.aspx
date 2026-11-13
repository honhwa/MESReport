<%@ Page language="c#" Codebehind="FrmSBInputQuery.aspx.cs" AutoEventWireup="True" Inherits="WebReport.Reports.FrmSBInputQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrmSBInputQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="popcalendar.js"></script>
		<SCRIPT Language="JavaScript">dateFormat='yyyy-mm-dd'</SCRIPT>
	</HEAD>
	<body background="pic\background.jpg" MS_POSITIONING="GridLayout">
		<FONT face="新細明體">
			<FORM id="Form1" method="post" runat="server">
				<TABLE id="Table1" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 48px"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px" align="center"><asp:image id="Image2" runat="server" Width="500px" Height="4px" ImageUrl="pic\line.gif"></asp:image></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 64px" align="center">
							<P><asp:label id="Label1" runat="server" Width="468px" Height="24px" Font-Names="Arial" Font-Size="X-Large"
									ForeColor="DodgerBlue">Start Bank Input Query</asp:label></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image3" runat="server" Width="500px" Height="4px" ImageUrl="pic\line.gif"></asp:image></P>
						</TD>
					</TR>
				</TABLE>
 
			    <TABLE id="Table3" align="center" border="0">
					<TR>
						<TD align="left" style="height: 18px; width: 327px;"><asp:label id="Label3" runat="server" Width="144px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">Please Select:</asp:label></TD>
					</TR>
					<TR>
					 	<TD align="left" style="width: 327px"><asp:radiobuttonlist id="RadioButtonList1" runat="server" Width="324px" Height="16px" Font-Names="Arial"
								Font-Size="10pt" AutoPostBack="True" RepeatLayout="Flow" CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
								<asp:ListItem Value="CRLT" Selected="True">Start Bank Input Query</asp:ListItem>
							</asp:radiobuttonlist></TD>
					</TR>

				</TABLE>

				<TABLE id="Table4" style="WIDTH: 244px; HEIGHT: 210px" align="center" border="0">
					<TR>
						<TD style="WIDTH: 89px; HEIGHT: 13px" align="left"><asp:label id="Label7" runat="server" Width="72px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">Frm Time:</asp:label></TD>
						<TD style="HEIGHT: 13px" align="center">
							<P align="left"><asp:textbox id="TextBox1" runat="server" Width="128px"></asp:textbox><INPUT style="BACKGROUND-IMAGE: url(../../Images/Date.gif); WIDTH: 21px; HEIGHT: 21px" onclick="popUpCalendar(this, Form1.TextBox1, dateFormat,-1,-1,true)"
									type="button"></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px; HEIGHT: 14px" align="left"><asp:label id="Label6" runat="server" Width="56px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">To Time:</asp:label></TD>
						<TD style="HEIGHT: 13px" align="center">
							<P align="left"><asp:textbox id="TextBox2" runat="server" Width="128px"></asp:textbox><INPUT style="BACKGROUND-IMAGE: url(../../Images/Date.gif); WIDTH: 21px; HEIGHT: 21px" onclick="popUpCalendar(this, Form1.TextBox2, dateFormat,-1,-1,true)"
									type="button"></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px; HEIGHT: 5px" align="left"><asp:checkbox id="CheckBox2" runat="server" Width="64px" Height="24px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True" AutoPostBack="True" Text="ProdID:" oncheckedchanged="CheckBox2_CheckedChanged"></asp:checkbox></TD>
						<TD style="HEIGHT: 5px"><FONT face="新細明體"><asp:dropdownlist id="DropDownList2" runat="server" Width="152px" Height="32px" Enabled="False"></asp:dropdownlist></FONT></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px" align="left"><FONT face="新細明體"><asp:checkbox id="CheckBox3" runat="server" Width="64px" Height="24px" Font-Names="Arial" Font-Size="10pt"
									Font-Bold="True" AutoPostBack="True" Text="Wonbr:" oncheckedchanged="CheckBox3_CheckedChanged"></asp:checkbox></FONT></TD>
						<TD><FONT face="新細明體"><asp:dropdownlist id="DropDownList3" runat="server" Width="152px" Height="32px" Enabled="False"></asp:dropdownlist></FONT></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px" align="left"><asp:checkbox id="CheckBox4" runat="server" Width="80px" Height="24px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True" AutoPostBack="True" Text="Customer:" oncheckedchanged="CheckBox4_CheckedChanged"></asp:checkbox></TD>
						<TD><asp:dropdownlist id="DropDownList4" runat="server" Width="152px" Height="32px" Enabled="False"></asp:dropdownlist></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px" align="center"><asp:button id="Button1" runat="server" Width="64px" Text="Query" onclick="Button1_Click"></asp:button></TD>
						<TD align="center"><asp:button id="Button2" runat="server" Width="64px" Text="Reset" onclick="Button2_Click"></asp:button></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 89px"></TD>
						<TD></TD>
					</TR>
					<tr>
						<td style="WIDTH: 89px" align="left"><asp:label id="Label5" runat="server" Width="80px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								ForeColor="Red">◆請選擇條件</asp:label></td>
						<td></td>
					</tr>
				</TABLE>
			</FORM>
		</FONT>
	</body>
</HTML>
