<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DTactTime.aspx.cs" Inherits="Report.Reports.IE.DTactTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="uc" TagName="datepicker"Src="WebUserControl1.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
   
    <table  width="100%">
            <tr >
            <td align="center" colspan ="4" style="font-size:larger">Tact Time Detail Query</td>
            </tr>
            <tr>
            <td align ="center" style="width: 75px" >Time From :</td>
            <td><uc:datepicker ID="dtfr"   runat ="server" Visible="true"  /></td>
            <td align ="center" style="width: 73px"></td>
            <td></td>
        <%--    <td align ="center" style="width: 73px" visible="false" >Time To :</td>
            <td><uc:datepicker ID="dtto"   Visible="false" runat ="server"   /></td>--%>
            </tr>
            <tr>
            <td align ="right" style="width: 10%" > Plant :</td>
            <td  style="width: 20%"  ><asp:DropDownList ID="ddlfactory" runat ="server"  Width="95%"></asp:DropDownList></td>
            <td align ="right" style="width: 10%">Process :</td>
            <td style="width: 20%"><asp:DropDownList ID="ddlpro" runat ="server" Width="95%" AutoPostBack="True" ></asp:DropDownList></td>
            </tr>           
            <tr>
            <td align ="right" style="width: 10%"> Subline ID :</td>
            <td> <asp:DropDownList ID="ddlline" runat ="server" Width="95%"></asp:DropDownList></td>
            <td align ="right" style="width: 10%">Product ID :</td>
            <td> <asp:DropDownList ID="ddlprod" runat ="server" Width="95%"></asp:DropDownList></td>
            </tr>
            <tr>
            
            <td colspan ="2" align ="center"  >
            <asp:Button ID="cmdquery" runat ="server" AutoPostBack="True"  Text="Query" OnClick="cmdquery_Click"/>
           
            </td>
            <td colspan ="2" align ="center"> 
            <asp:Button ID="cmtexport" runat ="server" Text="Export" AutoPostBack="True" OnClick="cmtexport_Click" Visible="False" />
            </td>
            </tr>
            <tr >
            <td align="center" colspan ="4" style="font-size:larger; color:Red "><asp:Label ID="lblerror" runat ="server" Text="" ></asp:Label></td>
            </tr>
            </table>
           <%-- <rsweb:ReportViewer ID="ReportViewer1" Width ="98%" runat="server" Visible="true" >
                <LocalReport DisplayName="OEE Report">
                </LocalReport>
            
        </rsweb:ReportViewer>--%>
        <asp:Panel ID="pnlmain" runat ="server" >
        <asp:DataGrid ID="dgcontent" runat ="server" AutoGenerateColumns ="true" ></asp:DataGrid>
        </asp:Panel>
  
    </form>
</body>
</html>
