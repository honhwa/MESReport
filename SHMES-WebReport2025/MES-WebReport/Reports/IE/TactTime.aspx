<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TactTime.aspx.cs" Inherits="Report.Reports.IE.TactTime" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="uc" TagName="datepicker"Src="WebUserControl1.ascx" %>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
</head>
<body>
     <form id="form1" runat="server">  
    
       <%-- <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering ="true"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true"   UpdateMode ="always"  >
        
          <ContentTemplate >--%>
          
           <table  width="100%">
            <tr >
            <td align="center" colspan ="4" style="font-size:larger">Tact Time Query</td>
            </tr>
            <tr>
            <td align ="center" style="width: 75px" >Time From :</td>
            <td><uc:datepicker ID="dtfr"   runat ="server" Visible="true"  /></td>
            <td align ="center" style="width: 73px">Time To :</td>
            <td><uc:datepicker ID="dtto"   runat ="server" Visible="true"  /></td>
            </tr>
            <tr>
            <td align ="center" style="width: 75px" > Plant :</td>
            <td    ><asp:DropDownList ID="ddlfactory" runat ="server"  Width="95%"></asp:DropDownList></td>
            <td align ="center" style="width: 73px">Process :</td>
            <td><asp:DropDownList ID="ddlpro" runat ="server" Width="95%" AutoPostBack="True" ></asp:DropDownList></td>
            </tr>            
            <tr>
            <td align ="center" style="width: 75px"> Subline ID :</td>
            <td> <asp:DropDownList ID="ddlline" runat ="server" Width="95%"></asp:DropDownList></td>
            <td align ="center" style="width: 73px">Product ID :</td>
            <td> <asp:DropDownList ID="ddlprod" runat ="server" Width="95%"></asp:DropDownList></td>
            </tr>
            <tr>
            
            <td colspan ="2" align ="center"  >
            <asp:Button ID="cmdquery" runat ="server" AutoPostBack="True"  Text="Query" OnClick="cmdquery_Click"/>
           
            </td>
            <td colspan ="2" align ="center"> 
            <asp:Button ID="cmtexport" runat ="server" Text="Export"  AutoPostBack="True" OnClick="cmtexport_Click" />
             &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
            <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial"
                                Font-Size="10pt" NavigateUrl="~/Description/TACTTIMEHELP.HTM" Target="_blank" ToolTip="報表說明">Help</asp:HyperLink>
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
        <asp:DataGrid ID="dgcontent" runat ="server" AutoGenerateColumns ="true" HeaderStyle-Font-Size="smaller" HeaderStyle-ForeColor="ButtonShadow"  AlternatingItemStyle-ForeColor="blueviolet">
        <ItemStyle Font-Size="smaller" />
        </asp:DataGrid>
        </asp:Panel>
        <%--    </ContentTemplate>
        </asp:UpdatePanel>  &nbsp; &nbsp;&nbsp; &nbsp;
       --%>
    </form>
</body>
</html>
