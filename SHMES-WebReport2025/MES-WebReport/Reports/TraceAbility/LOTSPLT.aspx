<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LOTSPLT.aspx.cs" Inherits="WebReport.Reports.LOTSPLT" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <%--      <link href="../Stylesheet.css" rel="Stylesheet" type="text/html" />--%>
    <script type ="text/javascript" src ="popcalendar.js"></script>
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
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <strong><asp:Label ID="Label1" runat="server" Font-Size="20pt" ForeColor="BLACK" Text="Lot vs Panel"></asp:Label></td>
                </tr>
            </table>
            &nbsp; &nbsp; &nbsp; &nbsp;<table style="width: 753px; height: 50px">
                <tr>
                    <td style="width: 304px; height: 21px">
                        Lot ID: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtLot" runat="server" Width="171px"></asp:TextBox></td>
                    <td style="width: 278px; height: 21px">
                        <asp:Button ID="ButQ" runat="server" BorderStyle="Ridge" Font-Bold="True" Font-Size="12pt"
                            OnClick="ButQ_Click" Text="查詢" /></td>
                </tr>
                <tr>
                    <td style="width: 304px">
                        </td>
                    <td style="width: 278px">
                        </td>
                </tr>
            </table>
            <table style="width: 1018px">
                <tr>
                    <td style="height: 21px">
                        &nbsp;</td>
                </tr>
            </table>
                        <%--<rsweb:reportviewer id="ReportViewer3" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"></rsweb:reportviewer>--%>
            <rsweb:ReportViewer ID="ReportViewer3" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote">
            </rsweb:ReportViewer>
            <br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp;
            <br />
        </div>
    </div>
    </form>
</body>
</html>
