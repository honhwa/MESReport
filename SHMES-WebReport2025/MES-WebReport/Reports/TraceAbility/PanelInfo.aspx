<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PanelInfo.aspx.cs" Inherits="WebReport.Reports.PanelInfo" %>

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
            <table style="width:1018px">
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                       <strong><asp:Label ID="Label1" runat="server" Font-Size="20" ForeColor="BlACK" Text="Panel History"></asp:Label></td>
                </tr>
            </table>
            <br />
            <table style="width: 709px; height: 100px">
              <!--  <tr>
                    <td style="width: 265px;">
                        FromTime:<asp:TextBox ID="txtFromDate" runat="server" Width="158px"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/calendar.gif"
                            OnClick="ImageButton1_Click" /><br />
                        <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="Black"
                            BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black"
                            Height="62px" NextPrevFormat="ShortMonth" OnSelectionChanged="Calendar1_SelectionChanged1"
                            Visible="False" Width="211px">
                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                            <TodayDayStyle BackColor="#999999" ForeColor="White" />
                            <DayStyle BackColor="#CCCCCC" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                            <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt"
                                ForeColor="White" Height="12pt" />
                        </asp:Calendar>
                    </td>
                    <td style="width: 253px;">
                        To Time:<asp:TextBox ID="txtToDate" runat="server" Width="158px"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/calendar.gif"
                            OnClick="ImageButton2_Click1" /><br />
                        <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="Black"
                            BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black"
                            Height="62px" NextPrevFormat="ShortMonth" OnSelectionChanged="Calendar2_SelectionChanged1"
                            Visible="False" Width="211px">
                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                            <TodayDayStyle BackColor="#999999" ForeColor="White" />
                            <DayStyle BackColor="#CCCCCC" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                            <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt"
                                ForeColor="White" Height="12pt" />
                        </asp:Calendar>
                    </td>
                </tr>-->
                <tr>
                    <td style="width: 265px">
                        Lot ID/Panel ID :
                        <asp:TextBox ID="txtPanel" runat="server" Width="147px"></asp:TextBox></td>
                    <td style="width: 253px">
                        SN :&nbsp;
                        <asp:TextBox ID="txtSN" runat="server" Width="153px"></asp:TextBox>
                        &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 265px; height: 21px;">
                        Production :
                        <asp:DropDownList ID="DDLProdid" runat="server" OnSelectedIndexChanged="DDLProdid_SelectedIndexChanged"
                            Width="148px">
                        </asp:DropDownList></td>
                    <td style="width: 253px; height: 21px;">
                        <asp:Button ID="ButQ" runat="server" BorderStyle="Ridge" Font-Bold="True" Font-Size="12pt"
                            OnClick="ButQ_Click" Text="查詢" /></td>
                </tr>
                <tr>
                    <td style="width: 265px;">
                        </td>
                    <td style="width: 253px;">
                        </td>
                </tr>
            </table>
            <br />
            <table style="width: 1018px">
                <tr>
                    <td style="height: 21px">
                        <%--<rsweb:reportviewer id="ReportViewer10" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"></rsweb:reportviewer>--%>

                        <rsweb:ReportViewer ID="ReportViewer10" runat="server" height="483px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote">
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

