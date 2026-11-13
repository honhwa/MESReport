<%@ Page language="c#" Codebehind="FrmInputANDOutput.aspx.cs" AutoEventWireup="True" Inherits="WebReport.Reports.FrmInputANDOutput" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FrmSBInput</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body background="pic\background.jpg" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
			<FONT face="新細明體">
				<asp:textbox id="TextBox2" style="Z-INDEX: 102; LEFT: 744px; POSITION: absolute; TOP: 32px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox5" style="Z-INDEX: 105; LEFT: 744px; POSITION: absolute; TOP: 80px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox4" style="Z-INDEX: 104; LEFT: 744px; POSITION: absolute; TOP: 64px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox3" style="Z-INDEX: 103; LEFT: 744px; POSITION: absolute; TOP: 48px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox>
				<TABLE id="Table2" align="center" border="0">
					<tr>
						<TD style="HEIGHT: 16px"></TD>
					</tr>
					<tr>
						<TD style="WIDTH: 501px; HEIGHT: 1px" align="center"><asp:image id="Image4" runat="server" Height="4px" Width="500px" ImageUrl="pic\line.gif"></asp:image></TD>
					</tr>
					<tr>
						<TD style="WIDTH: 501px; HEIGHT: 42px" align="center">
							<P><asp:label id="Label3" runat="server" Height="24px" Width="443px" Font-Names="Arial" Font-Size="X-Large"
									ForeColor="DodgerBlue"></asp:label></P>
						</TD>
					</tr>
					<tr>
						<TD style="WIDTH: 501px; HEIGHT: 3px" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image2" runat="server" Height="4px" Width="500px" ImageUrl="pic\line.gif"></asp:image></P>
						</TD>
					</tr>
				</TABLE>
                <asp:TextBox ID="TextBox6" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="TextBox7" runat="server" Visible="False"></asp:TextBox>
				<TABLE id="Table1" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 17px; width: 873px;"><asp:label id="Label2" runat="server" Height="8px" Width="672px" Font-Names="Arial" Font-Size="Smaller"></asp:label></TD>
					</TR>
					<TR>
						<TD style="width: 873px">
							<div id="DIV1" style="WIDTH: 714px" ms_positioning="FlowLayout">
<%--                                <rsweb:ReportViewer ID="ReportViewer" runat="server" Width="870px">
                                </rsweb:ReportViewer>--%>
								<rsweb:ReportViewer ID="ReportViewer" runat="server">
                                </rsweb:ReportViewer>
                                &nbsp;
<%--                                <rsweb:ReportViewer ID="ReportViewerDefect" runat="server" Width="870px">
                                </rsweb:ReportViewer>--%>

                                <rsweb:ReportViewer ID="ReportViewerDefect" runat="server" Width="870px">
                                </rsweb:ReportViewer>

                            </DIV>
							<P>
                                &nbsp;</P>
						</TD>
					</TR>
				</TABLE>
                &nbsp;
				<asp:textbox id="TextBox1" style="Z-INDEX: 101; LEFT: 744px; POSITION: absolute; TOP: 16px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox></FONT></form>
	</body>
</HTML>
