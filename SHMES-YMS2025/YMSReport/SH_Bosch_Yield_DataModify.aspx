<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SH_Bosch_Yield_DataModify.aspx.cs" Inherits="YMSReport_SH_Bosch_Yield_DataModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>後台數據修改</title>
    <script type="text/javascript" language="javascript" src="popcalendar.js"></script>
    <script type="text/javascript" language="JavaScript">dateFormat='yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <div>
        <asp:TextBox ID="tbQueryDate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            MaxLength="10" Width="80px"></asp:TextBox>
        <img id="Img1" runat="server" height="17"
                        onclick="popUpCalendar(this, form1.tbQueryDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" />&nbsp;
        <asp:Button ID="btQuery" runat="server" Text="Query" OnClick="btQuery_Click" /><br />
        <br />
        <table style="width: 820px">
        <tr>
            <td valign="top" style="width: 492px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Font-Names="Microsoft Sans Serif" Font-Size="9pt" OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="TransDate" HeaderText="記錄時間"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="ReportDate" HeaderText="日期"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="PRODUCT_ID" HeaderText="產品型號"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="OPERATION_ID" HeaderText="站點"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="LOT_GRADE" HeaderText="等級"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="DEF_CODE" HeaderText="不良代碼"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:BoundField DataField="LOT_T_CNT" HeaderText="數量"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
            <td valign="top">
                <asp:Label ID="lblCmt" runat="server"></asp:Label><br />
                日期<asp:TextBox ID="txtDate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                    MaxLength="10" Width="80px"></asp:TextBox><img id="imgF" runat="server" height="17"
                        onclick="popUpCalendar(this, form1.txtDate, dateFormat,-1,-1,false)" src="../images/calendar.gif"
                        type="button" width="30" /><br />
                產品型號<asp:TextBox ID="tbProductID" runat="server" MaxLength="13"></asp:TextBox><br />
                站點<asp:DropDownList ID="ddlStation" runat="server" Font-Bold="True" Width="75px">
                    <asp:ListItem>M2</asp:ListItem>
                    <asp:ListItem>M3</asp:ListItem>
                </asp:DropDownList><br />
                等級<asp:DropDownList ID="ddlGrade" runat="server" Font-Bold="True" Width="75px">
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>S</asp:ListItem>
                </asp:DropDownList><br />
                不良代碼<asp:TextBox ID="tbDefectCode" runat="server" MaxLength="4"></asp:TextBox><br />
                數量<asp:TextBox ID="tbQty" runat="server" MaxLength="8"></asp:TextBox><br />
                <asp:Button ID="Button1" runat="server" Text="Ok" Width="58px" OnClick="Button1_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="Button2" runat="server" Text="Cancel" OnClick="Button2_Click" /></td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
