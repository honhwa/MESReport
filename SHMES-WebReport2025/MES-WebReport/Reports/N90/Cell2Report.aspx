<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cell2Report.aspx.cs" Inherits="Report.Reports.N90.Cell2Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cell2Report</title>
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd 07:30:00'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <table width="1000" height="30" >
            <tr>
                <td style="width: 1000px; height: 26px; text-align: center;">
                    <span style="font-size: 24pt; font-family: Arial"><strong><em>Cell2 Weekly &amp; Monthly
                        Report Download</em></strong></span></td>
            </tr>
        </table>
    
    </div>
        <table>
            <tr>
                <td style="width: 78px; height: 22px; text-align: left">
                    <strong>起始時間:</strong></td>
                <td style="width: 100px; height: 22px">
                    <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox></td>
                <td style="width: 100px; height: 22px">
                <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromTime, dateFormat,-1,-1,false);"type="button" style="width: 29px" />
                </td>
            </tr>
            <tr>
                <td style="width: 78px; height: 21px; text-align: left">
                    <strong>結束時間:</strong></td>
                <td style="width: 100px; height: 21px">
                    <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox></td>
                <td style="width: 100px; height: 21px">
                <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToTime, dateFormat,-1,-1,false);"type="button" style="width: 29px" /></td>
            </tr>
            <tr>
                <td style="width: 78px; height: 23px; text-align: left">
                    <strong>廠別:</strong></td>
                <td style="width: 100px; height: 23px">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="A" Width="35px" Font-Bold="True" AutoPostBack="True" OnCheckedChanged="RadioButton1_CheckedChanged" />
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="B" Width="35px" Font-Bold="True" AutoPostBack="True" OnCheckedChanged="RadioButton2_CheckedChanged" /></td>
                <td style="width: 100px; height: 23px">
                </td>
            </tr>
        </table>
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Height="25px" Width="317px"></asp:Label><br />
        <asp:Button ID="butYes" runat="server" Height="25" Text="下載" Width="79px" OnClick="butYes_Click" />
        <asp:Button ID="butExc" runat="server" Height="25" Text="匯出Excel" Width="79px" Visible="False" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Visible="False">
            <RowStyle ForeColor="#000066" />
            <Columns>
                <asp:BoundField DataField="fabid" HeaderText="FabID">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="verb" HeaderText="Verb">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Cworkctr" HeaderText="Cworkctr">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="tocprodid" HeaderText="Tocprodid">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="frprodid" HeaderText="Frprodid">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="toprodid" HeaderText="Toprodid">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="frlottype" HeaderText="Frlottype">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="tolottype" HeaderText="Tolottype">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="frlotlevel" HeaderText="Frlotlevel">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="tolotlevel" HeaderText="Tolotlevel">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="defectcd" HeaderText="Defectcd">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="qty1" HeaderText="Qty1">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="qty2" HeaderText="Qty2">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="week" HeaderText="Week">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </form>
</body>
</html>
