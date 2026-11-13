<%@ Page language="c#" Codebehind="FrmEQSMMReport.aspx.cs" AutoEventWireup="True" Inherits="WebReport.Reports.FrmEQSMMReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrmMYieldReportQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript" src="popcalendar.js"></script>
		<SCRIPT Language="JavaScript">dateFormat='yyyy-mm-dd'</SCRIPT>
	</HEAD>
	<body MS_POSITIONING="GridLayout" background="pic\background.jpg">
		<form id="Form1" method="post" runat="server">
			<FONT face="新細明體">
				<TABLE id="Table1" align="center" border="0">
					<TR>
						<TD style="WIDTH: 501px" align="center">
							<asp:image id="Image2" runat="server" ImageUrl="pic\line.gif" Height="4px" Width="500px"></asp:image></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 64px" align="center">
							<P>
								<asp:label id="Label1" runat="server" Height="24px" Width="426px" ForeColor="DodgerBlue" Font-Size="X-Large"
									Font-Names="Arial"> 設備狀態信息查詢</asp:label></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px" align="center">
							<P style="LINE-HEIGHT: 1pt">
								<asp:image id="Image3" runat="server" ImageUrl="pic\line.gif" Height="4px" Width="500px"></asp:image></P>
						</TD>
					</TR>
				</TABLE>
			</FONT>
			<TABLE id="Table5" align="center" cellSpacing="0" cellPadding="0" width="427" border="0"
				style="WIDTH: 427px; HEIGHT: 288px" runat="server">
				<TR>
				</TR>
			</TABLE>
			<TABLE id="Table4" align="center" border="0">
				<TR>
					<TD align="left" style="HEIGHT: 22px">
						<asp:label id="Label3" runat="server" Width="80px" Height="16px" Font-Names="Arial" Font-Size="10pt"
							Font-Bold="True">Type:</asp:label></TD>
					<td style="HEIGHT: 22px">
						<asp:RadioButtonList id="RadioButtonList2" runat="server" Height="16px" Width="136px" Font-Size="10pt"
							Font-Names="Arial" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0" RepeatLayout="Flow">
							<asp:ListItem Value="His" Selected="True">His</asp:ListItem>
							<asp:ListItem Value="Summary">Summary</asp:ListItem>
						</asp:RadioButtonList></td>
				</TR>
				<TR>
					<TD style="WIDTH: 80px; HEIGHT: 13px" align="left">
						<asp:label id="Label7" runat="server" Width="80px" Height="16px" Font-Names="Arial" Font-Size="10pt"
							Font-Bold="True">Frm Time:</asp:label></TD>
					<TD style="HEIGHT: 13px" align="center">
						<P align="left">
							<asp:textbox id="TextBox1" runat="server" Width="128px"></asp:textbox><INPUT style="BACKGROUND-IMAGE: url(pic\Date.gif); WIDTH: 21px; HEIGHT: 21px" onclick="popUpCalendar(this, Form1.TextBox1, dateFormat,-1,-1,true)"
								type="button"></P>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 80px; HEIGHT: 14px" align="left">
						<asp:label id="Label6" runat="server" Width="56px" Height="16px" Font-Names="Arial" Font-Size="10pt"
							Font-Bold="True">To Time:</asp:label></TD>
					<TD style="HEIGHT: 13px" align="center">
						<P align="left">
							<asp:textbox id="TextBox2" runat="server" Width="128px"></asp:textbox><INPUT style="BACKGROUND-IMAGE: url(pic\Date.gif); WIDTH: 21px; HEIGHT: 21px" onclick="popUpCalendar(this, Form1.TextBox2, dateFormat,-1,-1,true)"
								type="button"></P>
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 80px" align="left"><FONT face="新細明體">
							<asp:label id="Label2" runat="server" Width="56px" Height="16px" Font-Names="Arial" Font-Size="10pt"
								Font-Bold="True">Process:</asp:label></FONT></TD>
					<TD><FONT face="新細明體">
							<asp:dropdownlist id="DropDownList2" runat="server" Width="152px" Height="32px">
								<asp:ListItem Value="M1">M1</asp:ListItem>
								<asp:ListItem Value="CO">CO</asp:ListItem>
								<asp:ListItem Value="M2">M2</asp:ListItem>
								<asp:ListItem Value="M3">M3</asp:ListItem>
								<asp:ListItem Value="8015">8015</asp:ListItem>
								<asp:ListItem Value="8020">8020</asp:ListItem>
								<asp:ListItem Value="8030">8030</asp:ListItem>
								<asp:ListItem Value="8040">8040</asp:ListItem>
								<asp:ListItem Value="8050">8050</asp:ListItem>
								<asp:ListItem Value="8055">8055</asp:ListItem>
								<asp:ListItem Value="8060">8060</asp:ListItem>
								<asp:ListItem Value="MODULE">838L</asp:ListItem>
							</asp:dropdownlist></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 80px" align="center">
						<asp:button id="Button1" runat="server" Width="64px" Text="Query" onclick="Button1_Click"></asp:button></TD>
					<TD align="center">
						<asp:button id="Button2" runat="server" Width="64px" Text="Reset" onclick="Button2_Click"></asp:button></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 80px" align="left">
						<asp:label id="Label5" runat="server" Width="80px" Height="16px" Font-Names="Arial" Font-Size="10pt"
							ForeColor="Red">◆請選擇條件</asp:label></TD>
					<TD><FONT face="新細明體"></FONT></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
