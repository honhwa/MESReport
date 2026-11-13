<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WIPCheck.aspx.cs" Inherits="WebReport.Reports.ACC.WIPCheck" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>會計WIP盤點報表</title>
   		<script language="javascript" src="popcalendar.js"></script>
		<SCRIPT Language="JavaScript">dateFormat='yyyy-mm-dd'</SCRIPT> 
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            <div>
                <table style="width: 1018px">
                    <tr>
                        <td>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="會計WIP盤點報表"></asp:Label>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table style="font-size: 12pt; width: 1021px">
                    <tr>
                        <td colspan="2">
                            開始日期 &nbsp;<asp:TextBox ID="txtStartDT" runat="server" Width="143px"></asp:TextBox><input
                                id="btnFromTime" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
                                style="border-top-width: 0px; padding-right: 0px; padding-left: 0px; border-left-width: 0px;
                                background-image: url(../../images/Button.gif); border-bottom-width: 0px; padding-bottom: 0px;
                                width: 25px; padding-top: 0px; height: 17px; border-right-width: 0px" type="button" />
                            &nbsp; 模式 &nbsp;
                            <asp:DropDownList ID="DDLCSHOPCD" runat="server" Width="74px">
                                <asp:ListItem Value="0">WIP</asp:ListItem>
                                <asp:ListItem Value="1">R-&gt;S</asp:ListItem>
                                <asp:ListItem Value="2">BANK</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                </table>
                <table style="font-size: 12pt; width: 1000px; height: 10px">
                    <tr>
                        <td colspan="3" rowspan="3" style="width: 1008px; height: 21px">
                            <asp:Button ID="Seac_but" runat="server" BackColor="#CCFFFF" BorderStyle="Ridge"
                                OnClick="Seac_but_Click" Text="查詢" Width="75px" />
                            &nbsp; &nbsp; 
                            <asp:Label ID="Label2" runat="server" Width="563px"></asp:Label>
                            &nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="20px" ImageUrl="~/Images/help.gif"
                                OnClick="ImageButton2_Click" Width="17px" /></td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                    </tr>
                </table>
                &nbsp;<br />
                <table style="border-right: black 1px solid; border-top: black 1px solid;
                    border-left: black 1px solid; border-bottom: black 1px solid">
                    <tr>
                        <td style="width: 991px; height: 420px">
                            &nbsp;
                            <%--<rsweb:reportviewer id="ReportViewer" runat="server" width="950px"></rsweb:reportviewer>--%>

                            <rsweb:ReportViewer ID="ReportViewer" runat="server" width="950px">
                            </rsweb:ReportViewer>

                        </td>
                    </tr>
                </table>
                &nbsp;
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
