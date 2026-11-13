<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMDDPMModify.aspx.cs" Inherits="Report.Reports.MFG.RMDDPMModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>生產會報表人力工時修改功能</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>

        <table style="width: 844px; height: 54px;text-align:center">
            <tr>
                <td>
                    Module生產日報修改功能</td>
            </tr>
        </table>
        <table style="width: 845px; text-align: right">
            <tr>
                <td style="height: 43px; width: 96px;">
                    <div id="timePickerLabelLeft" class="LabelText">
                        日期</div>
                </td>
                <td style="height: 43px; text-align: left; width: 212px;">
                    <div id="timePickerLeftPicker" class="timePicker">
                        &nbsp;<asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                        <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromTime, dateFormat,-1,-1,false);"
                            type="button" />
                    </div>
                </td>
                <td style="height: 43px; width: 121px;">
                    <div id="timePickerLabelRight" class="LabelText">
                        機種</div>
                </td>
                <td style="width: 284px; height: 43px">
                    <asp:DropDownList ID="DlProdID" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 96px">
                    <div id="prodIdLabel" class="LabelText">
                        班別型態</div>
                </td>
                <td style="width: 212px">
                    <div id="prodIdDropDownList">
                        &nbsp;<asp:DropDownList ID="dlShiftID" runat="server" Width="158px">
                            <asp:ListItem Value="D">白班</asp:ListItem>
                            <asp:ListItem Value="N">晚班</asp:ListItem>
                            <asp:ListItem Value="ALL">白晚班</asp:ListItem>
                        </asp:DropDownList></div>
                </td>
                <td style="width: 121px">
                    <div id="Div1" class="LabelText">
                        Team ID</div>
                </td>
                <td style="width: 284px">
                    <asp:DropDownList ID="DlTeamID" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 21px; width: 96px;">
                    <div id="subLineLabel" class="LabelText">
                        產線</div>
                </td>
                <td style="height: 21px; width: 212px;">
                    <div id="partIdList">
                        <asp:DropDownList ID="DlSubLine" runat="server" Width="158px">
                        </asp:DropDownList>&nbsp;</div>
                </td>
                <td style="height: 21px; width: 121px;">
                    <div id="Div3" class="LabelText">
                        Lot Type</div>
                </td>
                <td style="width: 284px; height: 21px">
                    <asp:DropDownList ID="DlLotType" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 43px">
                    <div id="subLineList">
                        &nbsp;</div>
                </td>
                <td style="width: 284px; height: 43px">
                    <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢報表" /></td>
            </tr>
            <tr>
            </tr>
        </table>
        <table style="text-align: left">
            <tr>
                <td colspan="4" style="width: 613px">
                    <div id="ErrMsg">
                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>&nbsp;</div>
                </td>
            </tr>
        </table>
    
    </div>
        <asp:GridView ID="GVRMDD" runat="server" AutoGenerateColumns="False" BackColor="White"
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4"
            GridLines="Vertical" Height="220px" Width="844px" OnRowDataBound="GVRMDD_RowDataBound" ForeColor="Black" OnSelectedIndexChanged="GVRMDD_SelectedIndexChanged">
            <FooterStyle BackColor="#CCCC99" />
            <RowStyle BackColor="#F7F7DE" />
            <Columns>
                <asp:BoundField DataField="REPORTDATE" HeaderText="日期" />
                <asp:BoundField DataField="prodid" HeaderText="機種" />
                <asp:BoundField DataField="shiftid" HeaderText="班別型態" />
                <asp:BoundField DataField="userid" HeaderText="工號" Visible="False" />
                <asp:BoundField DataField="SubLineID" HeaderText="產線" />
                <asp:BoundField DataField="LotType" HeaderText="Lot Type" />
                <asp:BoundField DataField="TeamID" HeaderText="Team ID" />
                <asp:BoundField DataField="WORKINGHOUR" HeaderText="工時" />
                <asp:BoundField DataField="MANPOWER" HeaderText="人力" />
                <asp:BoundField DataField="teammode" HeaderText="Team Mode" />
            </Columns>
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </form>
</body>
</html>
