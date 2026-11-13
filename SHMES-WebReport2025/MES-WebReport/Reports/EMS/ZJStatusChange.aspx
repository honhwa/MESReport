<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZJStatusChange.aspx.cs" Inherits="Report.Reports.EMS.ZJStatusChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <style>.hidden{display:none;}</style>
<script language="javascript" type="text/javascript">
// <!CDATA[

function TABLE1_onclick() {

}

// ]]>
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;</div>
        <table id="TABLE1" onclick="return TABLE1_onclick()" style="width: 1100;">
            <tr>
                <td rowspan="3" style="width: 23px">
                    <table style="width: 300px; font-size: 10pt; font-family: Arial;">
                        <tr>
                            <td style="width: 86px">
                                ProdID:</td>
                            <td colspan="2" style="width: 199px">
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="111px">
                                </asp:DropDownList></td>
                            <td colspan="1" style="width: 366px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 86px; height: 24px;">
                                Next Status:</td>
                            <td style="height: 24px; width: 199px;" colspan="2">
                                <asp:DropDownList ID="DropDownList2" runat="server" Width="110px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                </asp:DropDownList>
                                </td>
                            <td colspan="1" style="width: 366px; height: 24px">
                                <asp:TextBox ID="TextBox2" runat="server" Visible="False" Width="45px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 86px; height: 20px;">
                                SubStatus</td>
                            <td style="width: 199px; height: 20px;"colspan="2">
                                <asp:DropDownList ID="DropDownList3" runat="server" Width="108px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
                                </asp:DropDownList>&nbsp;</td>
                            <td colspan="1" style="width: 41px; height: 20px">
                                <asp:TextBox ID="TextBox3" runat="server" Width="142px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 86px">
                                CMT</td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox1" runat="server" Width="262px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 86px">
                            </td>
                            <td colspan="2" style="width: 199px" >
                                <asp:Button ID="Button" runat="server" Text="OK" Width="44px" OnClick="Button_Click" /></td>
                            <td colspan="1" style="width: 366px">
                            </td>
                        </tr>
                    </table>
                    &nbsp;<asp:CheckBoxList ID="CheckBoxList1" runat="server" Width="88px" EnableTheming="True" RepeatColumns="6" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" AutoPostBack="True" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                    </asp:CheckBoxList></td>
                <td style="width: 499px" colspan="2" rowspan="0">
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                        Style="font-size: 10pt; font-family: Arial" AutoGenerateColumns="False" Height="93px" OnRowDataBound="GridView1_RowDataBound">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="EQID" HeaderText="ZJID" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="pstatus" HeaderText="P Status" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="STATUS" HeaderText="Status" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="startdate" HeaderText="StartTime" >
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProdID" HeaderText="ProdID" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserID" HeaderText="UserID" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EQCDDESC" HeaderText="StatusType" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CMT" HeaderText="CMT" >
                                                            <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PCol" HeaderText="PCol"  > 
                                                            <HeaderStyle Wrap="False" CssClass="hidden" />
                                <ItemStyle Wrap="False" CssClass="hidden" />
                                <FooterStyle Wrap="False" CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Col" HeaderText="Col" >
                                                                                        <HeaderStyle Wrap="False" CssClass="hidden" />
                                <ItemStyle Wrap="False" CssClass="hidden" />
                                <FooterStyle Wrap="False" CssClass="hidden" />
                                                                                        <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                                <FooterStyle Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    </form>
</body>
</html>
