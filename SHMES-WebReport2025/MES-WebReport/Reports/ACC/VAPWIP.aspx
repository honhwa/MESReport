<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VAPWIP.aspx.cs" Inherits="WebReport.Reports.VAPWIPReport" %>

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
     <table border="1" style="font-size: 10pt; color: green; font-family: 新細明體; mso-ascii-font-family: 'Courier New';
                        mso-hansi-font-family: 'Courier New'; mso-bidi-font-family: 'Courier New'; mso-font-kerning: 0pt;
                        mso-no-proof: yes; width: 982px; border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge;">
            <tr>
                <td style="width: 103px; height: 19px; text-align: right;">
                                        Bussiness owner：</td>
                <td style="width: 82px; height: 19px">
                    徐 &nbsp;&nbsp; 琴&nbsp; 55122</td>
                <td style="width: 81px; text-align: center;" rowspan="2">
                   <a href="../../Description/VAPWIP.mht" target="_blank">報表規格書</a></td>
                <td rowspan="2" style="width: 654px; text-align: left">
                    </td>
            </tr>
            <tr>
                <td style="width: 103px; text-align: right; height: 22px;">
                                        IT owner：</td>
                <td style="width: 82px; height: 22px;">
                    胡俊寶&nbsp; 55883</td>
            </tr>
        </table> 
    <div>
        <div>
            <table style="width: 1018px">
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="VAPWIP報表"></asp:Label>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table style="width: 1021px">
                <tr>
                    <td colspan="2">
                        開始日期&nbsp;：&nbsp;<asp:TextBox ID="txtStartDT" runat="server" Width="143px"></asp:TextBox><input id="btnFromTime" onclick="popUpCalendar(this, txtStartDT, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" />
                        結束日期&nbsp;：&nbsp;<asp:TextBox ID="TxtEndDT" runat="server" Width="143px"></asp:TextBox><input id="Button1" onclick="popUpCalendar(this, TxtEndDT, dateFormat,-1,-1,false)"
          style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
          background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" />&nbsp;
                        <asp:Button ID="Seac_but" runat="server" BackColor="#CCFFFF" BorderStyle="Ridge"
                            OnClick="Seac_but_Click" Text="查詢" Width="75px" /></td>
                </tr>
            </table>
            <table style="width: 1000px">
                <tr>
                    <td colspan="3" rowspan="3" style="width: 1008px; height: 21px">
                        段名： &nbsp;<asp:DropDownList ID="DDLCSHOPCD" runat="server">
                            <asp:ListItem>CELL2</asp:ListItem>
                            <asp:ListItem>MODULE</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp;產品首碼： &nbsp;&nbsp;
                        <asp:DropDownList ID="DDLNonV" runat="server">
                            <asp:ListItem>ALL</asp:ListItem>
                            <asp:ListItem>V</asp:ListItem>
                            <asp:ListItem>Non-V</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp;</td>
                </tr>
                <tr>
                </tr>
                <tr>
                </tr>
            </table>
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1053px" Height="620px">
                                </rsweb:ReportViewer>--%>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1053px" Height="620px">
            </rsweb:ReportViewer>
            &nbsp;<br />
            &nbsp;
        </div>
    </div>
    </form>
</body>
</html>
