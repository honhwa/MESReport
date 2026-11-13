<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUserRole.aspx.cs" Inherits="Report.RoleSetMgt.EditUserRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 105px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbTitle" runat="server" ForeColor="Blue" Text="[編輯群組]" Font-Bold="True"></asp:Label>
            <br />
            <table style="width:100%;">
                <tr>
                    <td class="auto-style1">群組代碼</td>
                    <td>
                        <asp:TextBox ID="tbGroupCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">群組名稱</td>
                    <td>
                        <asp:TextBox ID="tbGroupName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
            <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" OnRowEditing="gvGroup_RowEditing" OnRowCancelingEdit="gvGroup_RowCancelingEdit" OnRowUpdating="gvGroup_RowUpdating" OnRowDeleting="gvGroup_RowDeleting">
                <AlternatingRowStyle BackColor="Gainsboro" />
                <Columns>
                    <asp:TemplateField HeaderText="序號">
                        <EditItemTemplate>
                            <asp:Label ID="lbID_EIT" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                            <asp:HiddenField ID="hfID_EIT" runat="server" value='<%# Bind("ID") %>'/>
                        </EditItemTemplate>
                        <ItemTemplate>                            
                            <asp:Label ID="lbRowNumber_IT" runat="server" Text='<%# Bind("RowNum") %>'></asp:Label>
                            <asp:HiddenField ID="hfID_IT" runat="server" value='<%# Bind("ID") %>'/>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbID_FT" runat="server" Text="新增群組：" Font-Size="Small" ForeColor="Blue"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="群組代碼">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbGroupCode_EIT" runat="server" Text='<%# Bind("udepart") %>'></asp:TextBox>                            
                            <asp:HiddenField ID="hfGroupCode_EIT" runat="server" value='<%# Bind("udepart") %>'/>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbGroupCode_FT" runat="server" Text="輸入新增群組代碼：(最長10個英文字)" Font-Size="Small" ForeColor="Blue"></asp:Label><br/>
                            <asp:TextBox ID="tbGroupCode_FT" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>                            
                            <asp:Label ID="lbGroupCode_IT" runat="server" Text='<%# Bind("udepart") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="群組名稱">
                        <EditItemTemplate>
                            <asp:TextBox ID="tbGroupName_EIT" runat="server" Text='<%# Bind("UDEPART_NAME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbGroupName_FT" runat="server" Text="輸入新增群組名稱：(最長50個中英文字)" Font-Size="Small" ForeColor="Blue"></asp:Label><br/>
                            <asp:TextBox ID="tbGroupName_FT" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbGroupName_IT" runat="server" Text='<%# Bind("UDEPART_NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="操作" ShowHeader="False">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate_EIT" runat="server" CausesValidation="True" CommandName="Update" Text="更新" />
                            &nbsp;<asp:Button ID="btnCancel_EIT" runat="server" CausesValidation="False" CommandName="Cancel" Text="取消" />
                            &nbsp;<asp:Button ID="btnDelete_EIT" runat="server" CausesValidation="False" CommandName="Delete" Text="刪除此列" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnInsert_FT" runat="server" Text="新增" OnClick="btnInsert_FT_Click" />
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit_IT" runat="server" CausesValidation="False" CommandName="Edit" Text="编辑" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
