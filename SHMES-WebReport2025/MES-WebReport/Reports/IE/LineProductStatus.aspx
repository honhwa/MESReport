<%@ Page Language="C#" AutoEventWireup="true" Codebehind="LineProductStatus.aspx.cs" Inherits="WebReport.Reports.LineProductStatus" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LineProductStatus</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px;
                padding-top: 0px; width: 100%" cellspacing="0">
                <tr>
                <td colspan="4" style="height: 15px">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnQuery" runat="server" Font-Names="Times New Roman" Font-Size="10pt"
                        OnClick="btnQuery_Click" Text="查詢報表" Width="66px" /></td>
                </tr>
                <tr>
                    <td style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        background-repeat: repeat-y; vertical-align: top; height: 517px; width: 994px;" rowspan="2">
                        <div id="report" style="width: 81%; height: 454px">
<%--                            <rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="109%" Height="478px">
                            </rsweb:ReportViewer>--%>

                            <rsweb:ReportViewer ID="reportViewer" runat="server" ShowFindControls="False"
                                Width="109%" Height="478px">
                            </rsweb:ReportViewer>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
