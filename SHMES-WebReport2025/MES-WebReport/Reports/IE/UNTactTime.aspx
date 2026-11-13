<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnTactTime.aspx.cs" Inherits="Report.Reports.IE.UnTactTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="uc" TagName="datepicker"Src="WebUserControl1.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
</head>
<body>
     <form id="form1" runat="server">
    <div>
    <table width ="98%" >
    <tr>
    <td colspan="2" align ="center" >未   維   護   機   種   TACT TIME  信   息</td>
    </tr>    
    <tr>
    <td style="width: 60px" align="right"  >日期：</td>
         <td style="width: 169px"  > <uc:datepicker id="dt"   runat ="server" visible="true"  />
    </td>
   <%-- <td align ="right" style="width: 141px" > Factory :</td>            
    <td align="left"><asp:DropDownList ID="ddlfactory" runat ="server"  Width="50%"></asp:DropDownList>
        </td>--%>
    </tr>
    <tr><td colspan ="2" align ="center" ><asp:Button ID="btnquery" Text="查  詢" runat ="server" OnClick="btnquery_Click" /></td></tr>
    <tr>
    <td colspan ="2" ><asp:Label ID="lblerr" runat ="server"  ForeColor ="red"></asp:Label></td>
    </tr>
    </table>
    </div>
        <asp:GridView ID="gvcontent" RowStyle-VerticalAlign="middle"   RowStyle-HorizontalAlign="center"  AutoGenerateColumns="true"  runat="server" Width="95%">
        </asp:GridView>
    </form>
</body>
</html>
