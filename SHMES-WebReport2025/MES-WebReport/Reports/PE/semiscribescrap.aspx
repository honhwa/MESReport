<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="semiscribescrap.aspx.cs" Inherits="WebReport.Reports.PE.semiscribescrap" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register TagPrefix="uc" TagName="datepicker"Src="datepicker.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Semi Scribe Track Out Scrap Panel ID Report</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
       <asp:Panel id="pnltitle" runat ="server" HorizontalAlign ="center" Width ="95%">
       <asp:label ID="lbltitle" Font-Bold ="true" Font-Size ="Larger" runat ="server" Width="100%" >Semi Scribe Track Out Scrap Panel ID Query</asp:label>
       </asp:Panel>       
        <table width="95%">
        <tr >       
        <td style="width:6%" align ="right" >
        <asp:Label ID="Label1" runat="server" Width ="47%" Font-Bold="True" Font-Size="Medium"
            Text=" From:" BackColor="AliceBlue"></asp:Label>
        </td>
        <td style="width:16%">
            &nbsp;<uc:datepicker ID="dtfr"   runat ="server" Visible="true"  /> </td>
            <td style="width:4%" align ="right" ><asp:Label ID="Label2" runat="server" Font-Bold="True" Text="To:" BackColor="AliceBlue"></asp:Label></td>
            <td style="width:16%">&nbsp;&nbsp;
                <uc:datepicker ID="dtto" runat ="server" Visible="true"  />
       </td>       
        <td style="width:18%" align ="right" ><asp:Label ID='lblprod' Font-Bold ="True" Width="98%" runat ="server" BorderWidth ="0px" BackColor="AliceBlue" >CELL-2 Product ID:</asp:Label></td>
        <td style="width:37%" >&nbsp;
        <div  runat ="server"  style="border-right: black 1px solid; border-top: black 1px solid; overflow: scroll; overflow-x:hidden;
                        z-index: 104; left: 202px;  border-left: black 1px solid;
                        width: 153px; border-bottom: black 1px solid; top: 78px; height: 100px">
                        <asp:CheckBox ID="chkprodall" runat="server" AutoPostBack="True"
                            Text="ALL" Width="90%" OnCheckedChanged="chkprodall_CheckedChanged"   />
                        <asp:CheckBoxList ID="lstProduct" runat="server" AutoPostBack="True" Width="90%"   Height="87px" >
                           
                        </asp:CheckBoxList>
                    </div> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp;<asp:Button ID="btnquery" runat ="server"  Text="查询" Height="43px" Width="50" OnClick="btnquery_Click"/>&nbsp;&nbsp;&nbsp;
            </td>
        
        </tr>
       <tr><td colspan ="6">
        <asp:Label  ID="lblgls" runat ="server" >Panel ID:</asp:Label>
        <asp:TextBox ID="txtpnl" runat ="server" Columns="1" TextMode="MultiLine" Width="102px" ></asp:TextBox>If some ,Please seperate them by enter key,For example &nbsp; A<br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; B<br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; C</td></tr>
        </table> 
        
      <div runat ="server" >
          &nbsp;<%--<rsweb:ReportViewer ID="ReportViewer" runat="server" Width="901px">
          </rsweb:ReportViewer>--%>

          <rsweb:ReportViewer ID="ReportViewer" runat="server"  Width="901px">
          </rsweb:ReportViewer>

      </div>
         
    
    </form>
</body>
</html>
