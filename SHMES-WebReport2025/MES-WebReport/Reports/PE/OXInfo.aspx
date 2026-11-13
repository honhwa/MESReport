<%@ Page Language="C#" AutoEventWireup="true"CodeBehind="OXInfo.aspx.cs" Inherits="Report.Reports.PE.OXInfo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OX</title>
</head>
<body background="../../Images/right_bg.jpg" style="background-color: #ffffff">
    <form id="form1" runat="server">
    <div>
        <table style="width: 1000px">
            <tr>
                <td style="width: 1000px; height: 26px; text-align: center">
                    <strong><span style="font-size: 16pt; font-family: Arial">Cell2 OX Info Analysis</span></strong></td>
            </tr>
        </table>
    
    </div>
        <table style="width: 500px" runat = "server" >
            <tr>
                <td style="width: 300px; height: 80px">
        <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" Font-Bold="True"
            Text="By:" Width="52px" /><asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="True" Width="103px">
            <asp:ListItem>PanelID</asp:ListItem>
            <asp:ListItem>CartonID</asp:ListItem>
            <asp:ListItem>GlassID</asp:ListItem>
        </asp:DropDownList></td>
                <td style="width: 500px; height: 80px">
                    <asp:TextBox ID="txtInfo" runat="server" Columns="1" TextMode="MultiLine" Width="200px" Height="70px"></asp:TextBox><span
                        style="color: #ff0033; font-family: Arial"></span></td>
                <td style="width: 100px; height: 40px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 26px;">
                    <asp:Button ID="butQuery" runat="server" Font-Bold="True" Font-Names="Arial" Text="Query"
                        Width="100px" OnClick="butQuery_Click" /></td>
                <td style="width: 250px; height: 26px;">
                    <asp:Button ID="butExport" runat="server" Font-Bold="True" Font-Names="Arial" Text="Export Data"
                        Width="100px" OnClick="butExport_Click" Enabled="False" />
                    <asp:Button ID="butRefresh" runat="server" Font-Bold="True" Font-Names="Arial" Text="Refresh"
                        Width="100px" OnClick="butRefresh_Click" /></td>
                <td style="width: 100px; height: 26px;">
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                    <strong><span style="color: #ff0000"><span style="font-size: 10pt; font-family: Arial">
                        总數</span>:&nbsp;</span></strong>
                    <asp:Label ID="lblCount" runat="server" ForeColor="Red" Text="0" Width="50px" Font-Names="Arial"></asp:Label></td>
                <td style="width: 100px">
                </td>
            </tr>
            <tr>
                <td colspan="3">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" Width="500px">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="CartonID">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CartonID") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("CartonID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="GlassID">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("GlassID") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("GlassID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Panel ID">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PanelID") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("PanelID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Time">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Time") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Judge">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Judge") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Judge") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Defect Code">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("DefectCode") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("DefectCode") %>' Width="100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
