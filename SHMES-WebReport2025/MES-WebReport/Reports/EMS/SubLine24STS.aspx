<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubLine24STS.aspx.cs" Inherits="Report.Reports.EMS.SubLine24STS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <div>
            <div style="width: 846px; height: 33px; text-align: center; font-variant: small-caps">
                產線 前24小時狀態</div>
        </div>
        <table style="width: 764px">
            <tr>
                <td style="width: 44px; height: 134px">
                    站別</td>
                <td style="width: 106px; height: 134px">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 109px">
                        <asp:CheckBox ID="chkAllSls" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllSls_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstsls" runat="server" AutoPostBack="True" Height="19px" OnSelectedIndexChanged="lstsls_SelectedIndexChanged"
                            Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 39px; height: 134px">
                    產線</td>
                <td style="width: 125px; height: 134px">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 148px; border-bottom: black 1px solid; top: 78px; height: 117px">
                        <asp:CheckBox ID="CheckEqID" runat="server" AutoPostBack="True" OnCheckedChanged="CheckEqID_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstEqID" runat="server" AutoPostBack="True" Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 86px; height: 134px">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢報表" /></td>
                <td style="width: 170px; height: 134px">
                    <div id="Div9990" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 585px; border-left: black 1px solid; width: 181px; border-bottom: black 1px solid;
                        position: absolute; top: 54px; height: 120px">
                        <asp:Label ID="lblDown" runat="server" Text="Down" Width="41px"></asp:Label>
                        <asp:Label ID="lblENG" runat="server" Text="ENG" Width="41px"></asp:Label>
                        <asp:Label ID="lblIDLE" runat="server" Text="IDLE" Width="41px"></asp:Label>
                        <asp:Label ID="lblOther" runat="server" Text="Other" Width="41px"></asp:Label>
                        <asp:Label ID="lblPM" runat="server" Text="PM" Width="41px"></asp:Label>
                        <asp:Label ID="lblRUN" runat="server" Text="RUN" Width="41px"></asp:Label>
                        <asp:Label ID="lblSetup" runat="server" Text="Setup" Width="41px"></asp:Label>
                        <asp:Label ID="lblWait" runat="server" Text="Wait" Width="41px"></asp:Label><br />
                        <asp:Label ID="lblNonOperation" runat="server" Text="Nonoperation" Width="87px"></asp:Label>
                        <asp:Label ID="lblPartial" runat="server" Text="PartialDown " Width="83px"></asp:Label><br />
                        <asp:Label ID="Label6" runat="server" Height="24px" Width="174px"></asp:Label></div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>&nbsp;
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BorderColor="Black"
            CellPadding="0" Font-Size="Medium" GridLines="Horizontal" Height="333px" OnRowDataBound="GridView1_RowDataBound"
            ShowFooter="True" Width="317px">
            <Columns>
                <asp:BoundField DataField="SubLineID" HeaderText="產線  ">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left"
                        Wrap="False" />
                    <FooterStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
                <asp:BoundField />
            </Columns>
            <RowStyle Wrap="False" />
            <FooterStyle Wrap="False" />
            <SelectedRowStyle Wrap="False" />
            <HeaderStyle Wrap="False" />
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
