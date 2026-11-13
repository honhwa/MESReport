<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineDefectSummary.aspx.cs" Inherits="WebReport.Reports.OnlineDefectSummary"  StylesheetTheme="UNICOMTheme"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>LotDefectQueryList</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body background="../../Images/right_bg.jpg" >
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
     <div>
        <div>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br />
            <table style="width: 1018px">
                <tr>
                    <td style="height: 31px">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <strong><asp:Label ID="Label1" runat="server" Font-Size="20pt" ForeColor="BLACK" Text="Online Defect Summary"></asp:Label></strong></td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                <td align="center" style="width: 50px; height: 30px">
                    </td>
                    <td style="height: 42px; text-align: left" align="left">
                        FRDATE ：<asp:TextBox ID="txtFromDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromDate, dateFormat,-1,-1,false);"
                                                type="button" />
                    </td>
                    <td colspan="2" style="height: 42px; text-align: left" align="left">
                        TODATE &nbsp; &nbsp; ：<asp:TextBox ID="txtToDate" runat="server" Width="107px"></asp:TextBox>&nbsp;
                                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false);"
                                                type="button" />
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 50px; height: 30px">
                    </td>
                    <td style="width: 294px; height: 30px;" align="left">
                        LOT ID &nbsp; ：<asp:TextBox ID="TxtLotID" runat="server" Width="143px"></asp:TextBox></td>
                    <td style="width: 260px; height: 30px;" align="left">
                        WORKCTR：<asp:DropDownList ID="DropDownList1" runat="server" Width="152px"  >  </asp:DropDownList></td>
                    <td style="width: 231px; height: 30px;">
                        <asp:Button ID="ButQ" runat="server" BorderStyle="Ridge" Font-Bold="True" Font-Size="12pt"
                            OnClick="Button1_Click" Text="查詢" /></td>
                </tr>
                <tr>
                    <td colspan="1" style="height: 30px">
                    </td>
                    <td align="left" colspan="11" style="height: 30px">
                                    <asp:Label ID="lblErr" runat="server" Font-Underline="True" ForeColor="Red" Width="251px" Font-Names="Arial" Font-Size="Small"></asp:Label>
                    </td>
            </tr>
             </table>
            &nbsp;<br />
            <table style="width: 1018px">
                <tr>
                    <td style="height: 21px">
                        &nbsp;</td>
                </tr>
            </table>
                        <%--<rsweb:reportviewer id="ReportViewer1" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"></rsweb:reportviewer>--%>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote">
                        </rsweb:ReportViewer>
            <br />
            <br />
            &nbsp;<br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp;
            <br />
        </div>
    </div>
  
      </form>
</body>
</html>
