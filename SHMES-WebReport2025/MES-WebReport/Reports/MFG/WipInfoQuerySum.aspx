<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="WipInfoQuerySum.aspx.cs" Inherits="Report.Reports.MFG.WipInfoQuerySum" %>
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
                <td style="width: 1000px; text-align: center; height: 20px;">
                    <strong>N90&N94&nbsp; &nbsp; WIP 統計查詢</strong></td>
            </tr>
        </table>
        &nbsp;<br />
        &nbsp;FABID: &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlFabid" runat="server" Width="135px"  >
            <asp:ListItem>LCM1+LCM2</asp:ListItem>
            <asp:ListItem Value="LCM1">LCM1</asp:ListItem>
            <asp:ListItem Value="LCM2">LCM2</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;<asp:Label ID="lbln94"  
            runat="server" Text="N90&N94:"></asp:Label>
        &nbsp; &nbsp;<asp:DropDownList ID="ddln904"  runat="server" Width="141px" OnSelectedIndexChanged="ddln904_SelectedIndexChanged1" AutoPostBack="True"  >
        </asp:DropDownList>
        <br />
        <table>
            <tr>
                <td style="width: 92px; height: 95px;">
                    <strong>ProductType:</strong></td>
                <td style="width: 100px; height: 95px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow: scroll;
                        z-index: 104; left: 202px; overflow: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 194px">
                        <asp:CheckBox ID="chkProductType" runat="server" AutoPostBack="True"
                            Text="ALL" Width="100%" Checked="True" OnCheckedChanged="chkProductType_CheckedChanged" />
                        <asp:CheckBoxList ID="lstProductType" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="lstProductType_SelectedIndexChanged" Height="87px">
                            <asp:ListItem>Cell2 OCF Product</asp:ListItem>
                            <asp:ListItem>Cell2 ICF Product</asp:ListItem>
                            <asp:ListItem>Module OCF</asp:ListItem>
                            <asp:ListItem>Module ICF</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 46px; height: 95px;">
                    <strong>Grade:</strong></td>
                <td style="width: 100px; height: 95px;">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow: scroll;
                        z-index: 104; left: 202px;  overflow: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 94px">
                        <asp:CheckBox ID="chkGrade" runat="server" AutoPostBack="True" OnCheckedChanged="chkGrade_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstGrade" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="lstGrade_SelectedIndexChanged">
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>R</asp:ListItem>
                            <asp:ListItem Value="S"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 100px; height: 23px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="420px"></asp:Label></td>
                <td style="width: 94px; height: 23px">
                    &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="ButQuery" runat="server" Height="25px" Text="查詢" Width="65px" OnClick="ButQuery_Click" />
                    </td>
            </tr>
        </table>
    </div>
      <asp:Panel ID="pnlsummary" runat ="server" Width ="60%" HorizontalAlign ="center">
      <asp:GridView ID="gvsummary" runat ="server" Width ="100%"   AlternatingRowStyle-BackColor="AliceBlue"></asp:GridView>
      </asp:Panel>
     <asp:Panel ID="pnln94" runat ="server" Width ="60%" HorizontalAlign ="center" ></asp:Panel>
    </form>
</body>
</html>

