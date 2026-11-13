<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true"  CodeBehind="MOEE_Tgt.aspx.cs" Inherits="Report.Reports.IE.MOEE_Tgt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>BMW OEE Target Mapping Table維護主檔</title>
    <%--防止頁面緩存之前的內容導致顯示內容不對的問題--%>
    <meta http-equiv="pragram" content="no-cache">
    <meta http-equiv="cache-control" content="no-cache, must-revalidate">
    <meta http-equiv="expires" content="0">
    <%--防止頁面緩存之前的內容導致顯示內容不對的問題--%>
    <style type="text/css">
        .auto-style6 {
            height: 25px;
        }
        .auto-style7 {
            height: 25px;
            width: 799px;
        }
        .auto-style8 {
            margin-left: 0px;
        }
        .auto-style9 {
            width: 793px;
            height: 14px;
        }
    </style>
    </head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server" autocomplete="off">
    <div>
        &nbsp;</div>
        <table class="auto-style9">
            <tr>
                <td align="center" colspan="2" style="height: 41px">
                    <strong><span style="font-size: 16pt">BMW OEE Target Mapping Table維護主檔</span></strong></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblPLANT" runat="server" Text="PLANT"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlfab" runat="server" Width="170px" >
                        <asp:ListItem>SHLCM</asp:ListItem>                       
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblProcess" runat="server" Text="Process"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlProcess" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblEQ_ID" runat="server" Text="EQ_ID"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlEQID" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblEQ_TYPE" runat="server" Text="EQ_TYPE"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px;">
                    <asp:DropDownList ID="ddlEQTYPE" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td  style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblOEE_Tgt" runat="server" Text="OEE_Tgt"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px">
                    <asp:TextBox ID="txtOEE_Tgt" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="lblQuality" runat="server" Text="Quality"></asp:Label>
                    &nbsp;</td>
                <td style="height: 25px">
                    <asp:TextBox ID="txtQuality" runat="server" Width="170px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" ></asp:Label>
                </td>
                <td style="height: 25px">
                </td>
            </tr>
        </table>
        <table  style="width: 799px; height: 14px" >
                <tr>
                <td colspan="7" >
                    <asp:Button ID="btnAdd" runat="server" Text="添加" Width="50px" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="修改" Width="50px" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="刪除" Width="50px" OnClick="btnDelete_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="提交" Visible="False" Width="50px" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" Visible="False" Width="50px" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="刷新" Width="50px" OnClick="btnRefresh_Click" />
                    <asp:Button ID="btnExport" runat="server" Text="匯出" Width="50px" OnClick="btnExport_Click" />
                </td>
            </tr> 
            <tr>
                <td colspan="10" >
                    <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto" Width="800px">
                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="PLANT,PROCESS,EQ_ID,EQ_TYPE" Height="52px" Width="800px">
                            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Left" Wrap="False" />
                            <Columns>
                                <asp:TemplateField HeaderText="全選">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="Selected" runat="server" AutoPostBack="True" oncheckedchanged="Selected_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Selected" runat="server"  />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Size="5px" Width="5px" HorizontalAlign="Left" />
                                    <ItemStyle Font-Size="5px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PLANT" HeaderText="PLANT"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" Width="80px" /></asp:BoundField>
                                <asp:BoundField DataField="PROCESS" HeaderText="PROCESS"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="EQ_ID" HeaderText="EQ_ID"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="EQ_TYPE" HeaderText="EQ_TYPE"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="OEE_Tgt" HeaderText="OEE_Tgt"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="Quality" HeaderText="Quality"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="UPDATER" HeaderText="更新人員"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" Width="60px" /></asp:BoundField>
                                <asp:BoundField DataField="TIME_STAMP" HeaderText="更新時間"><ItemStyle Wrap="False" /><HeaderStyle Wrap="False" HorizontalAlign="Left" Width="150px" /></asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Wrap="False" HorizontalAlign="Left" />
                            <AlternatingRowStyle BackColor="#F7F7F7" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
