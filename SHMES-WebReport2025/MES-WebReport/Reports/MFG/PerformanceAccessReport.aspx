<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceAccessReport.aspx.cs" Inherits="WebReport.Reports.PerformanceAccessReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>績效考核Report</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src=popcalendar.js></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body style="font-family: Times New Roman">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            <div>
                <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                    margin: 0px; width: 100%; padding-top: 0px">
                    <tr>
                        <td style="background-position-x: left; background-image: url(../../Images/right_bg.jpg);
                            width: 1px; background-repeat: repeat-y">
                        </td>
                        <td rowspan="2" style="background-position-x: center; background-image: url(../../Images/right_bg.jpg);
                            width: 761px; background-repeat: repeat-y">
                            <div id="filter">
                                <table>
                                    <tr>
                                        <td style="vertical-align: middle; width: 10px; height: 20px">
                                        </td>
                                        <td style="vertical-align: middle; width: 73px; height: 20px; text-align: center">
                                        </td>
                                        <td style="vertical-align: middle; width: 17px; height: 20px; text-align: center">
                                        </td>
                                        <td style="width: 158px; font-family: Times New Roman; height: 20px">
                                        </td>
                                        <td colspan="2" style="font-family: Times New Roman; height: 20px; text-align: center;">
                                            <strong><span style="font-size: 14pt">績效考核報表</span></strong></td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: middle; width: 10px; height: 19px">
                                        </td>
                                        <td style="vertical-align: middle; width: 73px; height: 19px; text-align: center;">
                                            月份</td>
                                        <td style="vertical-align: middle; width: 17px; height: 19px; text-align: center">
                                        </td>
                                        <td style="width: 158px; font-family: Times New Roman; height: 19px"><asp:DropDownList ID="cmbYear" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="78px">
                                            <asp:ListItem>  </asp:ListItem>
                                            <asp:ListItem>2008</asp:ListItem>
                                            <asp:ListItem>2009</asp:ListItem>
                                            <asp:ListItem>2010</asp:ListItem>
                                            <asp:ListItem>2011</asp:ListItem>
                                            <asp:ListItem>2012</asp:ListItem>
                                        </asp:DropDownList>
                                            <asp:DropDownList ID="cmbMonth" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="74px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>1</asp:ListItem>
                                                <asp:ListItem>2</asp:ListItem>
                                                <asp:ListItem>3</asp:ListItem>
                                                <asp:ListItem>4</asp:ListItem>
                                                <asp:ListItem>5</asp:ListItem>
                                                <asp:ListItem>6</asp:ListItem>
                                                <asp:ListItem>7</asp:ListItem>
                                                <asp:ListItem>8</asp:ListItem>
                                                <asp:ListItem>9</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td style="font-family: Times New Roman; height: 19px; width: 47px;">
                                        </td>
                                        <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 19px">
                                        </td>
                                    </tr>
                                    <tr style="font-size: 12pt; font-family: Times New Roman">
                                        <td style="width: 10px; height: 34px; text-align: left">
                                        </td>
                                        <td style="width: 73px; height: 34px; text-align: center">
                                            機種群組</td>
                                        <td style="width: 17px; height: 34px; text-align: center">
                                        </td>
                                        <td style="width: 158px; height: 34px">
                                            <div id="partIdList">
                                                <asp:DropDownList ID="cmbProdGroup" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="157px">
                                                </asp:DropDownList>&nbsp;</div>
                                        </td>
                                        <td style="height: 34px; text-align: left; width: 47px;">
                                            </td>
                                        <td style="width: 149px; height: 34px">
                                            </td>
                                    </tr>
                                    <tr style="font-size: 12pt; font-family: Times New Roman">
                                        <td style="width: 10px; height: 33px; text-align: left">
                                        </td>
                                        <td style="width: 73px; height: 33px; text-align: center">
                                            Team號碼</td>
                                        <td style="width: 17px; height: 33px; text-align: center">
                                        </td>
                                        <td style="width: 158px; height: 33px">
                                            <div id="subLineList">
                                                <asp:DropDownList ID="cmbTeamID" runat="server" AutoPostBack="True" Font-Names="Arial"
                                                    Font-Size="10pt" Width="157px">
                                                </asp:DropDownList>&nbsp;</div>
                                        </td>
                                        <td style="height: 33px; text-align: left; width: 47px;">
                                            </td>
                                        <td style="width: 149px; height: 33px; text-align: center;">
                                            <asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt"
                                                OnClick="btnQuery_Click" Text="查詢報表" /></td>
                                    </tr>
                                    <tr style="font-size: 12pt; font-family: Times New Roman">
                                        <td colspan="1" style="width: 10px">
                                        </td>
                                        <td colspan="5">
                                            <div id="ErrMsg" style="text-align: center">
                                                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman" Font-Size="11pt"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="report" style="font-family: Times New Roman">
                                <%--<rsweb:reportviewer id="reportViewer" runat="server" height="451px" width="898px"></rsweb:reportviewer>--%>

                                <rsweb:ReportViewer ID="reportViewer" runat="server"  height="451px" width="898px">
                                </rsweb:ReportViewer>

                            </div>
                        </td>
                    </tr>
                    <tr style="font-family: Times New Roman">
                        <td rowspan="2" style="background-color: #3c2679; width: 1px;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
