<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccWipReport.aspx.cs" Inherits="WebReport.Reports.AccWipReport" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="Acc  WIP Report"></asp:Label></td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="width: 294px">
                        請選擇日期<asp:TextBox ID="txtDate" runat="server" Width="143px"></asp:TextBox><input id="btnFromTime" onclick="popUpCalendar(this, txtDate, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" /></td>
                    <td style="width: 231px">
                        <asp:Button ID="Seac_but" runat="server" OnClick="Seac_but_Click" Text="查詢" BackColor="#CCFFFF" BorderStyle="Ridge" Width="75px" /></td>
                </tr>
            </table>
            &nbsp;<table style="width: 1018px">
                <tr>
                    <td>
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1006px" Height="581px">
                                </rsweb:ReportViewer>--%>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1006px" Height="581px">
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
