<%@ Page language="c#" Codebehind="FrmEQSMMSummary.aspx.cs" AutoEventWireup="True" Inherits="WebReport.Reports.FrmEQSMMSummary" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrmEQSMMSummary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body background="pic\background.jpg" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
			<FONT face="新細明體">
				<asp:textbox id="TextBox2" style="Z-INDEX: 102; LEFT: 96px; POSITION: absolute; TOP: 72px" runat="server"
					Width="104px" Height="16px" Visible="False"></asp:textbox><asp:textbox id="TextBox3" style="Z-INDEX: 103; LEFT: 104px; POSITION: absolute; TOP: 80px" runat="server"
					Width="104px" Height="16px" Visible="False"></asp:textbox>
				<TABLE id="Table2" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 16px"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 1px" align="center"><asp:image id="Image4" runat="server" Width="500px" Height="4px" ImageUrl="pic\line.gif"></asp:image></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 42px" align="center">
							<P><asp:label id="Label3" runat="server" Width="473px" Height="24px" ForeColor="DodgerBlue" Font-Size="X-Large"
									Font-Names="Arial">Line Status Summary</asp:label></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 3px" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image2" runat="server" Width="500px" Height="4px" ImageUrl="pic\line.gif"></asp:image></P>
						</TD>
					</TR>
				</TABLE>
				<TABLE id="Table1" style="WIDTH: 1000px; HEIGHT: 128px" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 17px; width: 996px;">&nbsp;
							<asp:label id="Label1" runat="server" Width="465px" Height="16px"></asp:label></TD>
					</TR>
					<TR>
						<TD style="width: 996px">
							<DIV id="DIV2" ms_positioning="FlowLayout">
                                <%--<rsweb:ReportViewer ID="ReportViewer" runat="server" Width="992px">
                                </rsweb:ReportViewer>--%>
                                &nbsp;<rsweb:ReportViewer ID="ReportViewer" runat="server" Width="992px">
                                </rsweb:ReportViewer>
                            </DIV>
						</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
		<asp:TextBox id="TextBox1" style="Z-INDEX: 101; LEFT: 88px; POSITION: absolute; TOP: 64px" runat="server"
			Width="104px" Height="16px" Visible="False"></asp:TextBox>
	</body>
</HTML>
