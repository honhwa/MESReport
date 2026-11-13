<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WipInfoQuery.aspx.cs" Inherits="WebReport.Reports.PC.WipInfoQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<HEAD>
		<title>FrmWipSummaryQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	
<script language="javascript" type="text/javascript">
// <!CDATA[

// ]]>
</script>
</HEAD>
	<body MS_POSITIONING="GridLayout" background="pic\background.jpg">
		<form id="Form2" method="post" runat="server">
            <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
			<FONT face="新細明體">
				<TABLE id="Table1" align="center" border="0" onclick="return Table1_onclick()" style="width: 427px; height: 90px">
					<TR>
						<TD style="HEIGHT: 48px; width: 503px;"></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 503px; height: 6px;" align="center"><asp:image id="Image2" runat="server" ImageUrl="~/Images/line.gif" Height="4px" Width="500px"></asp:image></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 503px; HEIGHT: 64px" align="center">
							<P><asp:label id="Label1" runat="server" Height="24px" Width="448px" ForeColor="DodgerBlue" Font-Size="X-Large"
									Font-Names="Arial">WIP Summary Report</asp:label></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 503px; height: 5px;" align="center">
							<P style="LINE-HEIGHT: 1pt"><asp:image id="Image3" runat="server" ImageUrl="~/Images/line.gif" Height="4px" Width="500px"></asp:image></P>
						</TD>
					</TR>
					<TR>
						<TD style="WIDTH: 503px; height: 18px;" align="center">
						</TD>
					</TR>
				</TABLE>
				<table align="center" border="0" style="WIDTH: 504px; HEIGHT: 90px">
					<TR>
						<TD style="WIDTH: 77px; HEIGHT: 20px"><asp:checkbox id="chkProdid" runat="server" Height="26px" Width="64px" Font-Size="X-Small" Font-Names="Arial"
								Font-Bold="True" Text="ProdID:" AutoPostBack="True" OnCheckedChanged="chkProdid_CheckedChanged"></asp:checkbox></TD>
						<td style="WIDTH: 105px; HEIGHT: 20px"><asp:dropdownlist id="ddlProdid" runat="server" Height="30px" Width="105px" Enabled="False"></asp:dropdownlist></td>
						<TD style="WIDTH: 50px; HEIGHT: 20px">
                            <asp:Label ID="Label2" runat="server" Text="Status:" Width="61px"></asp:Label></TD>
						<td style="HEIGHT: 20px; width: 203px;">
							<P>&nbsp;</P>
                            <p>
                                &nbsp;</p>
                            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                                z-index: 104; left: 0px; overflow-x: hidden; border-left: black 1px solid; width: 160px;
                                border-bottom: black 1px solid; position: relative; top: -34px; height: 72px">
                                <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="True" BorderColor="White"
                                    Height="21px" OnCheckedChanged="chkStatus_CheckedChanged" Text="ALL" Width="95%" />
                                <asp:CheckBoxList ID="listlevel" runat="server" AutoPostBack="True"
                                    Width="100%">
                                    <asp:ListItem>C</asp:ListItem>
                                    <asp:ListItem>H</asp:ListItem>
                                    <asp:ListItem>P</asp:ListItem>
                                    <asp:ListItem>R</asp:ListItem>
                                    <asp:ListItem>W</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
						</td>
					</TR>
					<TR>
						<TD style="WIDTH: 77px; height: 25px;">
                            <asp:CheckBox ID="chklevel" runat="server" AutoPostBack="True" OnCheckedChanged="chklevel_CheckedChanged"
                                Text="Level:" Width="93px" /></TD>
						<td style="WIDTH: 105px; height: 25px;">
                            <asp:DropDownList ID="ddlLevel" runat="server" Enabled="False" Width="106px">
                                <asp:ListItem>AA</asp:ListItem>
                                <asp:ListItem>BS</asp:ListItem>
                                <asp:ListItem>G1</asp:ListItem>
                                <asp:ListItem>G3</asp:ListItem>
                                <asp:ListItem>G7</asp:ListItem>
                                <asp:ListItem>RR</asp:ListItem>
                                <asp:ListItem>S</asp:ListItem>
                                <asp:ListItem>S1</asp:ListItem>
                                <asp:ListItem>S2</asp:ListItem>
                                <asp:ListItem>S5</asp:ListItem>
                                <asp:ListItem>SA</asp:ListItem>
                                <asp:ListItem>ST</asp:ListItem>
                            </asp:DropDownList></td>
						<TD style="WIDTH: 50px; height: 25px;"></TD>
						<td style="height: 25px; width: 203px;"></td>
					</TR>
					<TR>
						<TD align="center" style="WIDTH: 77px; HEIGHT: 25px">
							<asp:Label id="Label5" runat="server" Height="16px" Width="95px" ForeColor="Red" Font-Size="10pt"
								Font-Names="Arial"></asp:Label></TD>
						<td style="WIDTH: 105px; HEIGHT: 25px" align="left">
							<asp:button id="butQuery" runat="server" Width="64px" Text="Query" OnClick="butQuery_Click"></asp:button></td>
						<TD align="center" style="WIDTH: 50px; height: 25px;">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/xls.jpg" OnClick="ImageButton1_Click"
                                Width="33px" Visible="False" /></TD>
						<TD style="height: 25px; width: 203px;"></TD>
					</TR>
				</table>
                <div style="text-align: center; vertical-align: middle;">
                    <%--<rsweb:reportviewer id="reportViewer" runat="server" height="454px" width="884px">
                            </rsweb:reportviewer>--%>

                    <rsweb:ReportViewer ID="reportViewer" runat="server" height="454px" width="884px">
                    </rsweb:ReportViewer>

                    &nbsp;
                    &nbsp;&nbsp;</div>
			</FONT>
		</form>
	</body>
</HTML>
