<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LcRecipeCheckQuery.aspx.cs" Inherits="WebReport.Reports.LcRecipeCheckQuery" %>
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
                    <td style="height: 29px">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="BarcodeSystemQuary"></asp:Label></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 294px">
                        <asp:Label ID="Label3" runat="server" Text="開始日期"></asp:Label>
                        <asp:TextBox ID="txtDate" runat="server" Width="143px"></asp:TextBox><input id="btnFromTime" onclick="popUpCalendar(this, txtDate, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" />
                        </td>
                    <td style="width: 231px">
                        <asp:Button ID="Seac_but" runat="server" OnClick="Seac_but_Click" Text="查詢" BackColor="#CCFFFF" BorderStyle="Ridge" Width="75px" /></td>
                </tr>
                <tr>
                    <td style="width: 294px">
                        <asp:Label ID="Label2" runat="server" Text="結束日期"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="144px"></asp:TextBox><input id="btnToTime" onclick="popUpCalendar(this, txtToDate, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" /></td>
                    <td style="width: 231px">
                        <asp:DropDownList ID="DdlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlType_SelectedIndexChanged"
                            OnTextChanged="DdlType_TextChanged" Width="78px">
                            <asp:ListItem Value="SEAL">封口</asp:ListItem>
                            <asp:ListItem Value="IJTN">液晶</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <table style="width: 1004px">
                <tr>
                    <td style="width: 64px">
                        <asp:CheckBox ID="chkProdid" runat="server" Text="產品" /></td>
                    <td style="width: 224px">
                        <asp:DropDownList ID="ddlProdid" runat="server" Width="151px">
                        </asp:DropDownList></td>
                    <td style="width: 74px">
                        <asp:CheckBox ID="ChkEQID" runat="server" Text="EQID" /></td>
                    <td>
                        <asp:DropDownList ID="ddlEqid" runat="server" Width="147px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 64px">
                        <asp:CheckBox ID="chkLotId" runat="server" Text="LotId" /></td>
                    <td style="width: 224px">
                        <asp:TextBox ID="txtlotid" runat="server" Width="142px"></asp:TextBox></td>
                    <td style="width: 74px">
                        <asp:CheckBox ID="ChkUserid" runat="server" Text="UserId" /></td>
                    <td>
                        <asp:TextBox ID="txtuserid" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <table style="width: 1018px">
                <tr>
                    <td>
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1006px" Height="550px">
                                </rsweb:ReportViewer>--%>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server"  Width="1006px" Height="550px">
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
