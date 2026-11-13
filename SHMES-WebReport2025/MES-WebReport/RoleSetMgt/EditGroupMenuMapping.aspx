<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGroupMenuMapping.aspx.cs" Inherits="Report.RoleSetMgt.EditGroupMenuMapping" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>群組功能設置</title>
    <style type="text/css">
        .style1 {
            width: 100%;
            margin-left: 0px;
        }

        .style2 {
            width: 187px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--            <asp:SiteMapPath ID="SiteMapPath1" runat="server" Font-Names="Verdana"
                Font-Size="Small" PathSeparator=" > ">
                <CurrentNodeStyle ForeColor="#333333" />
                <NodeStyle Font-Bold="True" ForeColor="#990000" />
                <PathSeparatorStyle Font-Bold="True" ForeColor="#990000" />
                <RootNodeStyle Font-Bold="True" ForeColor="#FF8000" />
            </asp:SiteMapPath>--%>
        </div>
        <table class="style1">
            <tr>
                <td>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <table class="style1" style="font-size: small">
                        <tr>
                            <td class="style2">&nbsp;</td>
                            <td>
                                <asp:Label ID="lbTitle" runat="server" Text="[群組選單分配]" Font-Bold="True" Font-Size="Large" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">&nbsp;</td>
                            <td>群組名稱：&nbsp; 
                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem>---請選擇---</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:Label ID="lbMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">&nbsp;</td>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Height="385px" ScrollBars="Auto"
                                    Width="481px">
                                    <asp:GridView ID="GridView1" runat="server"
                                        AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC"
                                        BorderStyle="Double" BorderWidth="1px" CellPadding="4" Height="20px"
                                        OnPageIndexChanging="GridView1_PageIndexChanging" Width="460px" Font-Size="10pt">
                                        <RowStyle BackColor="White" ForeColor="#003399"
                                            HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="全選">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Selected" runat="server" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="Selected0" runat="server"
                                                        AutoPostBack="True" OnCheckedChanged="Selected_CheckedChanged" />
                                                    全選
                                                </HeaderTemplate>
                                                <HeaderStyle Font-Size="Small" Width="51px" />
                                                <ItemStyle Width="45px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="群組編號" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id" runat="server"
                                                        Text='<%# Bind("id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Size="Small" Width="70px" />
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="群組名稱" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id1" runat="server"
                                                        Text='<%# Bind("fmenu") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Size="Small" Width="80px" />
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="群組菜單" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id2" runat="server"
                                                        Text='<%# Bind("cmenu") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Size="Small" Width="110px" />
                                                <ItemStyle Width="110px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="路徑" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id3" runat="server"
                                                        Text='<%# Bind("url") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Size="Small" Width="70px" />
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="編號" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_id4" runat="server"
                                                        Text='<%# Bind("fmenu_id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Size="Small" Width="70px" />
                                                <ItemStyle Width="70px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                        <PagerStyle BackColor="#99CCCC" ForeColor="#ffffff"
                                            HorizontalAlign="center" />
                                        <SelectedRowStyle BackColor="#009999" Font-Bold="True"
                                            ForeColor="#CCFF99" />
                                        <HeaderStyle BackColor="#5A8ED7" Font-Bold="True"
                                            ForeColor="#CCCCFF" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">&nbsp;</td>
                            <td>

                                <asp:ImageButton ID="IBtn_send" runat="server" ImageUrl="~/Images/保存.gif" OnClick="IBtn_send_Click" />

                                <%--<asp:Button runat="server" ID="IBtn_send" Text="保存" OnClick="IBtn_send_Click"></asp:Button>--%>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
