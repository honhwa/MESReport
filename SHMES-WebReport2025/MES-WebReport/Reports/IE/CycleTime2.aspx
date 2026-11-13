<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CycleTime2.aspx.cs" Inherits="Report.Reports.IE.CycleTime2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CycleTime2</title>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd '</script>
    <script src="popcalendar.js" type="text/javascript"></script>
    <link href="../CARTONID/Pager.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .aa
        {
            border-radius: 5px;
        }
        .bb
        {
            border-radius: 5px;
        }
        .cc
        {
            border-radius: 4px;
        }
        .dd
        {
            border-radius: 4px;
        }
        .ee
        {
            border-radius: 5px;
        }
        .ff
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        .gg
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        .hh
        {
            font-weight: 700;//加粗
            border-radius: 4px;
        }
        table.ii
        {
            border-collapse: collapse;
            border: solid 3px #507CD1;
        }
    </style>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table id="Table1" align="center" border="0" style="margin-bottom:8px" 
            width="270px">
            <tr>
                <td align="center" colspan="2" style="height: 28px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="X-Large" ForeColor="DodgerBlue" Height="25px" Width="270px">CycleTime Report</asp:Label>
                </td>
            </tr>
            <tr style="width: 270px;">
                <td width="120px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="11pt" Height="16px" Width="80px">報表類型:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownReport" runat="server"
                        CssClass="aa" Height="28px" Width="180px" 
                        onselectedindexchanged="DropDownReport_SelectedIndexChanged" 
                        AutoPostBack="True" ontextchanged="DropDownReport_TextChanged">
                        <asp:ListItem>Report1</asp:ListItem>
                        <asp:ListItem>Report2</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="width: 200px;">
                <td>
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="11pt" Height="16px" Width="60px">製程段:</asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownPro" runat="server"
                        CssClass="bb" Height="28px" Width="180px">
                        <asp:ListItem>Module</asp:ListItem>
                        <asp:ListItem>Beol</asp:ListItem>




                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="11pt" Height="16px" Width="80px">From Time:</asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtStartDT" runat="server" CssClass="cc" Height="20px" 
                        Width="145px"></asp:TextBox>
                    <input id="Button11" class="CalendarBotton" 
                        onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)" 
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px; background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px; width: 25px; padding-top: 0px; height: 18px; border-right-width: 0px" 
                        type="button" />
                </td>
            </tr>
            <tr>
                <td style="width: 80px">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="11pt" Height="16px" Width="100px">To Time:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndDT" runat="server" CssClass="dd" Height="20px" 
                        Width="145px"></asp:TextBox>
                    <input id="Button22" class="CalendarBotton" 
                        onclick="popUpCalendar(this, txtEndDT, dateFormat,-1,-1,false)" 
                        style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px; background-image: url(../../Images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px; width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px" 
                        type="button" />
                          </tr>
                           <tr>
                </td>
                                <td style="width: 80px">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" 
                        Font-Size="11pt" Height="16px" Width="100px">分組:</asp:Label>
                </td>
                                <td>
                    <asp:DropDownList ID="DropDownList_Group" runat="server"
                        CssClass="bb" Height="28px" Width="180px" 
                                        onselectedindexchanged="DropDownList_Group_SelectedIndexChanged">
                        <asp:ListItem>分型號、工位、工位</asp:ListItem>
                        <asp:ListItem>分工位</asp:ListItem>

                    </asp:DropDownList>
                </td>

            </tr>
        </table>

        <asp:Button ID="btnQuery" runat="server" CssClass="ff" Text="Query"
            Width="55px" BackColor="#337AB7" Height="30px" BorderColor="#2E6DA4" BorderStyle="Solid"
            ForeColor="White" onclick="btnQuery_Click" />
        <asp:Button ID="btnClear" runat="server" CssClass="gg" Text="Clear" Width="55px"
            Height="30px" BackColor="#D9534F" BorderColor="#D43F3A"
            BorderStyle="Solid" ForeColor="White" onclick="btnClear_Click" />
        <asp:Button ID="btnExport" runat="server" CssClass="hh" Text="Export"
            Width="55px" Height="30px" BackColor="#5CB85C" BorderColor="#4CAE4C"
            BorderStyle="Solid" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
            ForeColor="White" onclick="btnExport_Click" />
    
        <br />
    
        <br />

    
    </div>
        <div id="Div1" align="center" runat="server">
        <table>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" Height="232px" Width="548px" CellPadding="0"
                        ForeColor="#333333" HorizontalAlign="Center" UseAccessibleHeader="False" CssClass="ii">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="red" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
