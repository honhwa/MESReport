<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMDDQueryChart.aspx.cs" Inherits="WebReport.Reports.MFG.RMDDQueryChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server">
    <div>
        <div id="Div4" class="LabelText">
            <table style="width: 781px; height: 43px ; text-align: center">
                <tr>
                    <td style="height: 18px; width: 937px;">生產會報良率達成率報表</td>
                </tr>
            </table>
            <br />
            <br />
            <table style="width: 779px; text-align: right">
                <tr>
                    <td style="width: 95px; height: 25px">
                        <asp:RadioButton ID="rbtnmonth" runat="server" AutoPostBack="True" OnCheckedChanged="rbtnmonth_CheckedChanged"
                            Text="開始月份" /></td>
                    <td style="width: 185px; height: 25px; text-align: right">
                        <asp:DropDownList ID="dlfrMonth" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 221px; height: 25px">
                        結束月分</td>
                    <td style="width: 230px; height: 25px">
                        <asp:DropDownList ID="dltoMonth" runat="server" Width="158px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 22px">
                        <asp:RadioButton ID="rbtnweek" runat="server" AutoPostBack="True" OnCheckedChanged="rbtnweek_CheckedChanged"
                            Text="開始星期" /></td>
                    <td style="width: 185px; height: 22px; text-align: right">
                        <asp:DropDownList ID="dlfrWeek" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 221px; height: 22px">
                        結束星期</td>
                    <td style="width: 230px; height: 22px">
                        <asp:DropDownList ID="dltoWeek" runat="server" Width="158px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 16px">
                        <div id="timePickerLabelLeft" class="LabelText">
                            產線</div>
                    </td>
                    <td style="width: 185px; height: 16px">
                        <asp:DropDownList ID="DlSubLine" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 221px; height: 16px">
                        <div id="timePickerLabelRight" class="LabelText">
                            機種</div>
                    </td>
                    <td style="width: 230px; height: 16px">
                        <asp:DropDownList ID="DlProdID" runat="server" Width="158px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 10px">
                        <div id="prodIdLabel" class="LabelText">
                            機種群組</div>
                    </td>
                    <td style="width: 185px; height: 10px">
                        <div id="prodIdDropDownList">
                            &nbsp;<asp:DropDownList ID="dlProdGrop" runat="server" Width="158px">
                            </asp:DropDownList></div>
                    </td>
                    <td style="width: 221px; height: 10px">
                        <div id="Div1" class="LabelText">
                            Team ID</div>
                    </td>
                    <td style="width: 230px; height: 10px">
                        <asp:DropDownList ID="DlTeamID" runat="server" Width="161px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 12px">
                        <div id="subLineLabel" class="LabelText">
                            資料分組</div>
                    </td>
                    <td style="width: 185px; height: 12px">
                        <div id="subLineList">
                            <asp:RadioButtonList ID="rblDataGroup" runat="server" Height="110px" Width="153px" Style="text-align: left" AutoPostBack="True" BorderColor="Transparent" BorderStyle="Groove" Font-Bold="False" ForeColor="Black">
                                <asp:ListItem Selected="True" Value="0">Team ID</asp:ListItem>
                                <asp:ListItem Value="1">產線</asp:ListItem>
                                <asp:ListItem Value="2">機種</asp:ListItem>
                                <asp:ListItem Value="3">機種群組</asp:ListItem>
                            </asp:RadioButtonList></div>
                    </td>
                    <td style="width: 221px; height: 12px">
                        <div id="Div2" class="LabelText">
                            資料形式</div>
                    </td>
                    <td style="width: 230px; height: 12px">
                        <asp:RadioButtonList ID="rblDataFormat" runat="server" Height="81px" AutoPostBack="True" Width="166px"  Style="text-align: left" BorderStyle="Groove">
                            <asp:ListItem Selected="True" Value="0">Input達成率</asp:ListItem>
                            <asp:ListItem Value="1">Output達成率</asp:ListItem>
                            <asp:ListItem Value="2">良率</asp:ListItem>
                            <asp:ListItem Value="3">R Ratio</asp:ListItem>
                            <asp:ListItem Value="4">S Ratio</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 10px">
                            Team Mode</td>
                    <td style="width: 185px; height: 10px ">
                            <asp:DropDownList ID="dlTeamMode" runat="server" Width="158px">
                                <asp:ListItem>ALL</asp:ListItem>
                                <asp:ListItem Value="A">A</asp:ListItem>
                                <asp:ListItem Value="B">B</asp:ListItem>
                            </asp:DropDownList></td>
                    <td style="width: 221px; height: 10px">
                        站別</td>
                    <td style="width: 230px; height: 10px">
                        <asp:DropDownList ID="DLWorkctr" runat="server" Width="154px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 10px">
                    </td>
                    <td style="width: 185px; height: 10px">
                    </td>
                    <td style="width: 221px; height: 10px">
                    </td>
                    <td style="width: 230px; height: 10px">
                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="產生資料" /></td>
                </tr>
                <tr>
                </tr>
            </table>
            <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" Style="text-align: right"></asp:Label>
        </div>
    
    </div>
    </form>
</body>
</html>
