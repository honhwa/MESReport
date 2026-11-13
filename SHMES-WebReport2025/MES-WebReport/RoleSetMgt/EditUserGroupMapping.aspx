<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUserGroupMapping.aspx.cs" Inherits="Report.RoleSetMgt.EditUserGroupMapping" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用戶權限設置</title>
    <style type="text/css">
        .style1 {
            width: 50%;
            height: 287px;
        }

        .style5 {
            height: 306px;
            width: 157px;
        }

        .style7 {
            height: 306px;
            width: 554px;
        }

        .style12 {
            width: 554px;
        }

        .style13 {
            width: 22px;
        }

        .style14 {
            width: 33px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbTitle" runat="server" ForeColor="Blue" Text="[人員群組設置]" Font-Bold="True"></asp:Label>
        </div>
        <table cellspacing="1" class="style1">
            <tr>
                <td class="style14"></td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="請輸入查詢條件：" Style="font-size: small"></asp:Label>
                    <asp:TextBox ID="tbQuery" runat="server" Font-Size="Small" Width="215px"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="(輸入：工號 或 姓名)" ForeColor="Blue" Font-Size="Small" Width="200px"></asp:Label>
                    <asp:ImageButton ID="ibtnQuery" runat="server" ImageUrl="~/Images/查詢.gif" OnClick="ibtnQuery_Click" />
                    <asp:ImageButton ID="ibtnExport" runat="server" ImageUrl="~/Images/匯出.gif" OnClick="ibtnExport_Click" />
                    &nbsp;
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style14"></td>

                <td class="style7">

                    <asp:Panel ID="Panel1" runat="server" Height="302px" ScrollBars="Vertical" Width="920px">
                        <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" 
                            BorderStyle="Double" BorderWidth="2px" CellPadding="4" Font-Size="10pt" Height="16px" 
                            OnRowDataBound="GridView1_RowDataBound" 
                            OnRowEditing="GridView1_RowEditing"
                            OnRowDeleting="GridView1_RowDeleting"
                            OnRowUpdating="GridView1_RowUpdating"
                            OnRowCancelingEdit="GridView1_RowCancelingEdit">
                            <RowStyle BackColor="White" ForeColor="#003399" Height="25px" />
                            <Columns>
                                 
                                <asp:ButtonField CommandName="SingleClick" DataTextField="entry_no" Text="SingleClick" Visible="False">
                                    <ControlStyle Font-Underline="False" />
                                </asp:ButtonField>
                                
                                <asp:BoundField DataField="RowNumber" HeaderText="序號">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserID" HeaderText="工號">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="姓名">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="群組">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Eval("UDEPART_NAME") %>' ID="tbRole"></asp:TextBox>
                                        <asp:DropDownList ID="ddlRole" runat="server" Width="145px" DataTextField="udepart_name" DataValueField="udepart"></asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="編輯" ShowEditButton="True" CancelText="取消" UpdateText="更新" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                    <ItemStyle Width="120px" />
                                </asp:CommandField>
                                <%-- 
                                <asp:CommandField HeaderText="刪除" ShowDeleteButton="True" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:CommandField>
                                --%>
                            </Columns>
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <HeaderStyle BackColor="#5A8ED7" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                </td>
                <td class="style5">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
