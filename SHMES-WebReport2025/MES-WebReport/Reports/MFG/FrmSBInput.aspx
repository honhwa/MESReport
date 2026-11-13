<%@ Page language="c#" Codebehind="FrmSBInput.aspx.cs" AutoEventWireup="True" Inherits="WebReport.Reports.FrmSBInput" %>
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
			<FONT face="新細明體">
				<asp:textbox id="TextBox2" style="Z-INDEX: 102; LEFT: 744px; POSITION: absolute; TOP: 32px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox5" style="Z-INDEX: 105; LEFT: 744px; POSITION: absolute; TOP: 80px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox4" style="Z-INDEX: 104; LEFT: 744px; POSITION: absolute; TOP: 64px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox><asp:textbox id="TextBox3" style="Z-INDEX: 103; LEFT: 744px; POSITION: absolute; TOP: 48px" runat="server"
					Visible="False" Height="12px" Width="64px"></asp:textbox>
				<TABLE id="Table2" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 16px"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 1px" align="center"><asp:image id="Image4" runat="server" Height="4px" Width="500px" ImageUrl="pic\line.gif"></asp:image></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 42px" align="center">
							<P><asp:label id="Label3" runat="server" Height="24px" Width="443px" Font-Names="Arial" Font-Size="X-Large"
									ForeColor="DodgerBlue">Start Input Query Result</asp:label></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 501px; HEIGHT: 3px" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image2" runat="server" Height="4px" Width="500px" ImageUrl="pic\line.gif"></asp:image></P>
						</TD>
					</TR>
				</TABLE>
                <asp:TextBox ID="TextBox6" runat="server" Visible="False"></asp:TextBox>
				<TABLE id="Table1" align="center" border="0">
					<TR>
						<TD style="HEIGHT: 17px"><asp:label id="Label2" runat="server" Height="8px" Width="672px" Font-Names="Arial" Font-Size="Smaller"></asp:label><asp:imagebutton id="ImageButton2" runat="server" Height="17px" Width="25px" ImageUrl="~/Reports/MFG/xls.jpg"
								ToolTip="Export Excel"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD>
							<DIV id="DIV1" style="WIDTH: 714px" ms_positioning="FlowLayout"><asp:datagrid id="DataGrid1" runat="server" Height="96px" Width="720px" ToolTip="一頁最大顯示2000筆"
									PageSize="2000" BorderColor="Black" AutoGenerateColumns="False">
									<HeaderStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" Height="20px" ForeColor="White"
										BackColor="#367AC7"></HeaderStyle>
									<Columns>
										<asp:BoundColumn DataField="WONBR" HeaderText="Wonbr">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PRODID" HeaderText="ProdID">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="WORKCTR" HeaderText="Station">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="lotGrade" HeaderText="Grade">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="lotLevel" HeaderText="Level">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="opt1" HeaderText="Customer">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="QTY" HeaderText="QTY">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
									</Columns>
									<PagerStyle Font-Size="X-Small" Font-Names="Arial" Position="TopAndBottom" Wrap="False" Mode="NumericPages"></PagerStyle>
								</asp:datagrid></DIV>
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
