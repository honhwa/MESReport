<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryOfflineHandling.aspx.cs" Inherits="Report.Reports.MFG.QueryOfflineHandling" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Query Offline Handling</title>
    <style type="text/css">
        .auto-style1 {
            width: 102px;
        }
        .auto-style2 {
            width: 102px;
            height: 20px;
        }
        .auto-style3 {
            height: 20px;
        }        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="background: linear-gradient(to bottom, #f0f9ff 0%,#cbebff 47%,#a1dbff 100%);">
                    <asp:Label ID="lbTitle" runat="server" Text="[Query Offline Handling]" Font-Bold="True" ForeColor="Blue"></asp:Label>
                    <hr />
                    <br />
                    <table style="border: thin solid #000000; padding: inherit; width:50%; table-layout: fixed; border-spacing: 1px;" id="Menu">
                        <tr>
                            <td class="auto-style1">                        
                                時間範圍：</td>
                            <td style="font-weight: bold">
                                <asp:TextBox ID="tbDateFrom" runat="server" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="tbDateFrom_CalendarExtender" runat="server" Format="yyyyMMdd" TargetControlID="tbDateFrom">
                                </cc1:CalendarExtender>
                                &nbsp;～ 
                                <asp:TextBox ID="tbDateTo" runat="server" Width="100px"></asp:TextBox>
                                <cc1:CalendarExtender ID="tbDateTo_CalendarExtender" runat="server" Format="yyyyMMdd" TargetControlID="tbDateTo">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style2">線別：<br />
                            </td>
                            <td class="auto-style3">
                                <asp:DropDownList ID="ddlSubline" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">類別：</td>
                            <td>
                                <asp:DropDownList ID="ddlReason" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">PanelID：</td>
                            <td>
                                <asp:TextBox ID="tbPanelID" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">處理人工號：</td>
                            <td>
                                <asp:TextBox ID="tbUserID" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                                &nbsp;
                                <asp:Button ID="btnExport" runat="server" Text="導出" OnClick="btnExport_Click" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:GridView ID="gvResult" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AutoGenerateColumns="False">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:BoundField DataField="Timestamp" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" HeaderText="記錄時間" />
                            <asp:BoundField DataField="PanelID" HeaderText="PanelID" />
                            <asp:BoundField DataField="SublineID" HeaderText="線別" />
                            <asp:BoundField DataField="Reason" HeaderText="類別" />
                            <asp:BoundField DataField="UserID" HeaderText="處理人工號" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
