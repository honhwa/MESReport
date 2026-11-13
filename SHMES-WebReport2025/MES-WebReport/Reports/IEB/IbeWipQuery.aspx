<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="IbeWipQuery.aspx.cs" Inherits="WebReport.Reports.IbeWipQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WipQuery</title>
    <%--      <link href="../Stylesheet.css" rel="Stylesheet" type="text/html" />--%>
    <script type="text/javascript" src="popcalendar.js"></script>

    <script language="javascript" type="text/javascript">
        // <!CDATA[

        dateFormat = 'yyyy-mm-dd'

        function report_onclick() {

        }

// ]]>
    </script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <div>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br />
                <table style="width: 1018px">
                    <tr>
                        <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="IEB WIP Query">
                        </asp:Label>
                        </td>
                    </tr>
                </table>
                <strong><span style="font-family: Arial"></span></strong>
                <table width="1000">
                    <tr>
                        <td colspan="2" width="1000">
                            <strong><span style="font-family: Arial">查詢條件:</span></strong><span style="font-family: Arial">
                            </span>&nbsp; &nbsp;&nbsp; <strong><span style="font-family: Arial">&nbsp;</span></strong>
                            &nbsp;
                            <asp:RadioButton ID="raRecent" runat="server" Checked="True" Text="近三天查詢" AutoPostBack="True" OnCheckedChanged="raRecent_CheckedChanged" />
                            &nbsp;&nbsp; &nbsp;
                            <asp:RadioButton ID="raHistory" runat="server" Text="歷史查詢" AutoPostBack="True" OnCheckedChanged="raHistory_CheckedChanged" />
                            &nbsp; &nbsp; &nbsp; 
                            <asp:RadioButton ID="raCurretWIP" runat="server" Text="當前WIP" AutoPostBack="True"  OnCheckedChanged="raCurretWIP_CheckedChanged" />
                            &nbsp; &nbsp; &nbsp; 
                            <span style="font-family: Arial"><strong>結果顯示類型:</strong><span>
                            </span></span>
                            <asp:DropDownList ID="DDlstShowStyle" runat="server">
                                <asp:ListItem Value="0">Summary</asp:ListItem>
                                <asp:ListItem Value="1">Details</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 294px">
                            <strong><span style="font-family: Arial">請選擇日期: &nbsp;</span></strong>&nbsp;
                        <asp:TextBox ID="txtDate" runat="server" Width="120px" Font-Bold="False" Font-Italic="False"></asp:TextBox><input id="btnFromTime" onclick="popUpCalendar(this, txtDate, dateFormat, -1, -1, false)"
                            style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px; background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px; width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px"
                            type="button" /></td>
                        <td style="width: 231px">
                            <asp:Button ID="Seac_but" runat="server" OnClick="Seac_but_Click" Text="查詢" BackColor="#CCFFFF" BorderStyle="Ridge" Width="75px" />
                        </td>
                    </tr>
                </table>
                &nbsp;<table style="width: 1018px">
                    <tr>
                        <td style="height: 664px; width: 1026px;">
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="653px" Width="720px" ShowRefreshButton="False" SizeToReportContent="true">
                            </rsweb:ReportViewer>
                            <rsweb:ReportViewer ID="ReportViewer2" runat="server" Height="653px" Width="1020px" ShowRefreshButton="False">
                            </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="653px" Width="720px" ShowRefreshButton="False" SizeToReportContent="true">
                            </rsweb:ReportViewer>
                            <rsweb:ReportViewer ID="ReportViewer2" runat="server" Height="653px" Width="1020px" ShowRefreshButton="False">
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp;
            <br />
            </div>
        </div>
    </form>
</body>
</html>
