<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tptReport.aspx.cs" Inherits="WebReport.Reports.tptReport" %>

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
            <table style="width: 1018px">
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="TPT Report"></asp:Label></td>
                </tr>
            </table>
            <table style="width: 1021px">
                <tr>
                    <td colspan="2">
                        開始日期<asp:TextBox ID="txtStartDT" runat="server" Width="143px"></asp:TextBox><input id="btnFromTime" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" />
                        結束日期<asp:TextBox ID="TxtEndDT" runat="server" Width="143px"></asp:TextBox><input id="Button1" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" />&nbsp;
                        <asp:DropDownList ID="DdlFab" runat="server" Width="102px" Visible="False">
                            <asp:ListItem>LCM1</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <table style="width: 1000px">
                <tr>
                    <td colspan="3" rowspan="3" style="width: 1008px; height: 21px">
                        選擇BU<asp:DropDownList ID="ddldata" runat="server" Width="151px">
                        </asp:DropDownList>
                        <asp:Button ID="Seac_but" runat="server" BackColor="#CCFFFF" BorderStyle="Ridge"
                            OnClick="Seac_but_Click" Text="查詢" Width="75px" />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        分析比例
                        <asp:TextBox ID="txtrate" runat="server" Width="61px"></asp:TextBox><asp:Button ID="Button2"
                            runat="server" BackColor="#CCFFFF" BorderStyle="Ridge" OnClick="Button2_Click"
                            Text="分析" Width="77px" /></td>
                </tr>
                <tr>
                </tr>
                <tr>
                </tr>
            </table>
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1006px" Height="506px">
                                </rsweb:ReportViewer>--%>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1006px" Height="506px">
                </rsweb:ReportViewer>
            <br />
            <asp:GridView ID="gvdata" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="None" Width="530px">
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundField DataField="Rate" HeaderText="比例" />
                    <asp:BoundField DataField="GroupName" HeaderText="群組" />
                    <asp:BoundField DataField="TPT" HeaderText="平均值" />
                    <asp:BoundField DataField="Count" HeaderText="LOTCount" />
                </Columns>
                <RowStyle BackColor="#E3EAEB" />
                <EditRowStyle BackColor="#7C6F57" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
