<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMDDScrapMaterial.aspx.cs" Inherits="WebReport.Reports.MFG.RMDDScrapMaterial" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body topmargin=0 leftmargin=0 style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="width: 832px; height: 43px; text-align: center">
            <tr>
                <td style="height: 18px; width: 899px;">
                    生產會報物料報廢報表</td>
            </tr>
        </table>
        <table style="width: 828px; text-align: right">
            <tr>
                <td style="height: 25px; width: 96px;">
                    <asp:RadioButton ID="rbtnmonth" runat="server" OnCheckedChanged="rbtnmonth_CheckedChanged"
                        Text="開始月份" AutoPostBack="True" /></td>
                <td style="height: 25px; text-align:right; width: 185px;">
                    <asp:DropDownList ID="dlfrMonth" runat="server" Width="158px">
                    </asp:DropDownList></td>
                <td style="width: 221px; height: 25px">
                    結束月分</td>
                <td style="width: 192px; height: 25px">
                    <asp:DropDownList ID="dltoMonth" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 22px; width: 96px;">
                    <asp:RadioButton ID="rbtnweek" runat="server" Text="開始星期" OnCheckedChanged="rbtnweek_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 22px; text-align: right; width: 185px;">
                    <asp:DropDownList ID="dlfrWeek" runat="server" Width="158px">
                    </asp:DropDownList></td>
                <td style="width: 221px; height: 22px">
                    結束星期</td>
                <td style="width: 192px; height: 22px">
                    <asp:DropDownList ID="dltoWeek" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 16px; width: 96px;">
                        產線</td>
                <td style="height: 16px; width: 185px;">
                        <asp:DropDownList ID="DlSubLine" runat="server" Width="158px">
                        </asp:DropDownList>&nbsp;</td>
                <td style="width: 221px; height: 16px">
                    <div id="Div3" class="LabelText">
                        機種</div>
                </td>
                <td style="width: 192px; height: 16px">
                    <asp:DropDownList ID="DlProdID" runat="server" Width="158px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 10px; width: 96px;">
                    <div id="Div5" class="LabelText">
                        機種群組</div>
                </td>
                <td style="height: 10px; width: 185px;">
                    <div id="Div6">
                        &nbsp;<asp:DropDownList ID="dlProdGrop" runat="server" Width="158px">
                        </asp:DropDownList></div>
                </td>
                <td style="width: 221px; height: 10px">
                    <div id="Div7" class="LabelText">
                        Team ID</div>
                </td>
                <td style="width: 192px; height: 10px">
                    <asp:DropDownList ID="DlTeamID" runat="server" Width="161px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 12px; width: 96px;">
                    <div id="Div8" class="LabelText">
                        Team Mode</div>
                </td>
                <td style="height: 12px; width: 185px;">
                    <div id="Div9">
                    <asp:DropDownList ID="dlTeamMode" runat="server" Width="158px">
                        <asp:ListItem>ALL</asp:ListItem>
                        <asp:ListItem Value="A">A</asp:ListItem>
                        <asp:ListItem Value="B">B</asp:ListItem>
                    </asp:DropDownList></div>
                </td>
                <td style="width: 221px; height: 12px">
                </td>
                <td style="width: 192px; height: 12px">
                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢報表" /></td>
            </tr>
            </table>
        &nbsp;<asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" style="text-align: right"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" height="305px" width="830px">
        </rsweb:ReportViewer>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp;  
        <%--<rsweb:reportviewer id="ReportViewer1" runat="server" height="305px" width="830px"></rsweb:reportviewer>--%>
        &nbsp; &nbsp;
<%--        <reportviewer ID="ReportViewer2" runat="server" Height="300px" Width="832px">
        </reportviewer>--%>
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Height="300px" Width="832px">
        </rsweb:ReportViewer>
    </div>

    </form>
</body>
</html>
