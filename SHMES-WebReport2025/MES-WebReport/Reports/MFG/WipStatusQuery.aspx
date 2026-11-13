<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WipStatusQuery.aspx.cs" Inherits="Report.Reports.MFG.WipStatusQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>WipStatusQuery</title>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
    <div>
        <br />
        <table style="width: 1041px; height: 67px;">
            <tr>
                <td style="width: 1000px; text-align: center; height: 50px;">
                    <strong>N90&nbsp; &nbsp;STATUS QUERY</strong></td>
            </tr>
        </table>
        <br />
        &nbsp;FABID: &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlFabid" runat="server" Width="135px">
            <asp:ListItem>LCM1+LCM2</asp:ListItem>
            <asp:ListItem Value="LCM1">LCM1</asp:ListItem>
            <asp:ListItem Value="LCM2">LCM2</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp;
        <br />
        <table>
            <tr>
                <td style="width: 100px">
                    <strong>ProjectCode:</strong></td>
                <td style="width: 100px">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkProjectCode" runat="server" AutoPostBack="True" OnCheckedChanged="chkProjectCode_CheckedChanged"
                            Text="ALL" Width="100%" Checked="True" />
                        <asp:CheckBoxList ID="lstProjectCode" runat="server" AutoPostBack="True" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 100px">
                    <strong>ProductID:</strong></td>
                <td style="width: 100px">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkProdid" runat="server" AutoPostBack="True" OnCheckedChanged="chkProdid_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProdid" runat="server" AutoPostBack="True" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 51px">
                    <strong>
                    Station: </strong>
                </td>
                <td style="width: 100px">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkStation" runat="server" AutoPostBack="True" OnCheckedChanged="chkStation_CheckedChanged"
                            Text="ALL" Width="100%" Checked="True" />
                        <asp:CheckBoxList ID="lstStation" runat="server" AutoPostBack="True" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 134px;">
                    <strong>Wonbr:</strong></td>
                <td style="width: 100px; height: 134px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkWo" runat="server" AutoPostBack="True" OnCheckedChanged="chkWo_CheckedChanged"
                            Text="ALL" Width="100%" Checked="True" />
                        <asp:CheckBoxList ID="lstWonbr" runat="server" AutoPostBack="True" Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 100px; height: 134px;">
                    <strong>Grade:</strong></td>
                <td style="width: 100px; height: 134px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkGrade" runat="server" AutoPostBack="True" OnCheckedChanged="chkGrade_CheckedChanged"
                            Text="ALL" Width="100%" /><br />
                        <asp:CheckBoxList ID="lstGrade" runat="server" AutoPostBack="True" Width="100%" Height="86px">
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>R</asp:ListItem>
                            <asp:ListItem Value="S"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                </td>
                <td style="width: 51px; height: 134px;">
                </td>
                <td style="width: 100px; height: 134px;">
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px; height: 21px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="420px"></asp:Label></td>
                <td style="width: 167px; height: 21px">
                    &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="ButQuery" runat="server" Height="25px" Text="查詢" Width="65px" OnClick="ButQuery_Click" />
                    <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="匯出EXCEL"
                        Height="19px" Width="29px" ImageUrl="~/Images/xls.jpg" OnClick="ImageButton1_Click" /></td>
            </tr>
        </table>
    </div>
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False" DataKeyNames="cworkctr,workname,prodtype,qty_wait,qty_run,qty_w+r,qty_hold">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Fabid">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("fabid") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("fabid") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Station">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("cworkctr") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("cworkctr") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Station_Desc">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("workname") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("workname") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product_Type">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("prodtype") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("prodtype") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProjectCode">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("project_code") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("project_code") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prodid">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("prodid") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Bind("prodid") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Wonbr">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("Wonbr") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label11" runat="server" Text='<%# Bind("Wonbr") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LotGrade">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("lotgrade") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# Bind("lotgrade") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Qty_Run">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Qty_Run") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Qty_Run") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Qty_Wait">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Qty_Wait") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Qty_Wait") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Qty_Wait&amp;Run">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("[Qty_W+R]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("[Qty_W+R]") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Qty_Hold">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Qty_Hold") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("Qty_Hold") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
