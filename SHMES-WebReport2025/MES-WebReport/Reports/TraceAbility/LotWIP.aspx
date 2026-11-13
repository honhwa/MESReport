<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LotWIP.aspx.cs" Inherits="WebReport.Reports.LotWIP" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  <title>未命名頁面</title>  

     <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
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
            &nbsp; &nbsp; &nbsp;<table style="width: 1018px">
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <strong><asp:Label ID="Label1" runat="server" Font-Size="20" ForeColor="BLACK" Text="WIP Query"></asp:Label></td>
                </tr>
            </table>
            <br />
            <table>
            <!--    <tr>
                    <td style="width: 258px; height: 30px">
                        &nbsp;&nbsp;
                        FRDATE：<asp:TextBox ID="txtFromDate" runat="server" Width="107px"></asp:TextBox><input
                            id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromDate, dateFormat,-1,-1,false);"
                            type="button" style="width: 23px" /></td>
                    <td style="width: 288px; height: 30px">
                        TODATE &nbsp; &nbsp;：<asp:TextBox ID="txtToDate" runat="server" Width="107px"></asp:TextBox><input
                            id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false);"
                            type="button" style="width: 24px" /></td>
                    <td style="width: 231px; height: 30px; text-align: left">
                    </td>
                </tr>
                -->
                <tr>
                    <td style="width: 258px; height: 30px">
                        &nbsp;&nbsp;
                        LOT ID &nbsp;：<asp:TextBox ID="txtLot" runat="server" Width="143px"></asp:TextBox>
                    </td>
                    <td style="width: 288px; height: 30px;">
                        PRODUCT ：<asp:DropDownList ID="DlProdID" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 231px; height: 30px; text-align: left;">
                        </td>
                </tr>
                <tr>
                    <td style="width: 258px; height: 30px">
                        &nbsp;&nbsp;
                        STATUE：<asp:DropDownList ID="Dlstatus" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 288px; height: 30px">
                        WORKCTR：<asp:DropDownList ID="Dlworkctr" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 231px; height: 30px; text-align: left">
                    </td>
                </tr>
                <tr>
                    <td style="width: 258px; height: 30px">
                        &nbsp;&nbsp;
                        GRADE&nbsp; ：<asp:DropDownList ID="Dlgrade" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 288px; height: 30px; text-align: left">
                        WONBR &nbsp; &nbsp; ：<asp:DropDownList ID="Dlwonbr" runat="server" Width="158px">
                        </asp:DropDownList></td>
                    <td style="width: 231px; height: 30px; text-align: left">
                        <asp:Button ID="ButQ" runat="server" BorderStyle="Ridge" Font-Bold="True" Font-Size="12pt"
                            OnClick="ButQ_Click" Text="脤?" /></td>
                </tr>
            </table>
            &nbsp;<br />
            <table style="width: 1018px">
                <tr>
                    <td style="height: 21px">
                        <%--<rsweb:reportviewer id="ReportViewer1" runat="server" height="342px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"></rsweb:reportviewer>--%>

                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" height="342px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote">
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

