<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZJStatusHisQuery.aspx.cs" Inherits="WebReport.Reports.EMS.ZJStatusHisQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
   public override void VerifyRenderingInServerForm(Control control)
   {
   }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>治具歷史信息查詢</title>
        <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>    
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <table style="font-family: Arial">
            <tr>
                <td style="width: 105px; height: 26px;">
                    Fr Time:</td>
                <td style="height: 26px; width: 92px;">
                    <asp:TextBox ID="txtFromTime" runat="server"></asp:TextBox></td>
                <td style="height: 26px; width: 24px;">
                    <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, txtFromTime, dateFormat,-1,-1,true);"
                        style="width: 29px" type="button" /></td>
                <td style="height: 26px; width: 293px;">
                    </td>
                <td style="width: 83px; height: 26px">
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 7px">
                    To Time:</td>
                <td style="height: 7px; width: 92px;">
                    <asp:TextBox ID="txtToTime" runat="server"></asp:TextBox></td>
                <td style="height: 7px; width: 24px;">
                    <input id="Button2" class="CalendarBotton" onclick="popUpCalendar(this, txtToTime, dateFormat,-1,-1,true);"
                        style="width: 29px" type="button" />&nbsp;</td>
                <td style="height: 7px; width: 293px;">
                    </td>
                <td style="width: 83px; height: 7px">
                </td>
            </tr>
            <tr>
                <td style="width: 105px">
                    EQID:</td>
                <td colspan="2">
                    <asp:DropDownList ID="DropDownList1" runat="server">
                    </asp:DropDownList></td>
                <td style="width: 293px">
                </td>
                <td style="width: 83px">
                </td>
            </tr>
            <tr>
                <td style="width: 105px">
                </td>
                <td colspan="2">
                    <asp:Button ID="Button1" runat="server" Text="Query" OnClick="Button1_Click" /></td>
                <td style="width: 293px">
                </td>
                <td style="width: 83px">
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 1px">
                </td>
                <td colspan="2" style="height: 1px">
                </td>
                <td style="height: 1px; width: 293px;">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 
                </td>
                <td style="width: 83px; height: 1px">
                </td>
            </tr>
            <tr>
                <td colspan="6" style="height: 21px; width: 982px;">
                    <%--<rsweb:reportviewer id="ReportViewer" runat="server" width="950px"></rsweb:reportviewer>--%>

                    <rsweb:ReportViewer ID="ReportViewer" runat="server" width="950px">
                    </rsweb:ReportViewer>

                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
