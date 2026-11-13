<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="8180Outputquery.aspx.cs" Inherits="Report.Reports.ACC._180Outputquery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>8180Outputquery</title>
    <script type ="text/javascript" src ="NewDate.js"></script>
    
    <script language="javascript" type="text/javascript">
    
    dateFormat = 'yyyy-mm-dd'
    
        function Check_Uncheck_All(cb) {
            var cbl = document.getElementById("<%=chkML.ClientID%>");
            var input = cbl.getElementsByTagName("input");
            if (cb.checked) {
                for (var i = 0; i < input.length; i++) {
                    input[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < input.length; i++) {
                    input[i].checked = false;
                }
            }
        } 

    </script>
        <style type="text/css">
           .style1
            {
                border-right: black 1px solid; 
                border-top: black 1px solid;
                overflow-y: scroll;
                z-index: 104; 
                left: 202px;  
                overflow-x: hidden;
                border-left: black 1px solid;
                <%--width: 300px;--%> 
                border-bottom: black 1px solid; 
                top: 78px; 
                height: 100px
            }
            td 
            {
               <%-- height:40px;--%> 
                border:#CCCCCC 1px solid;
             }
        </style>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div style="height:250px; width:1000px;">
    <table>
    <tr><td align="center" colspan="10"><strong><span style="font-size: 24pt">Output Query</span></strong></td></tr>
    <tr style="height:40px">
    <td>FROM:</td>
    <td><asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
    <td>TO:</td>
    <td><asp:TextBox ID="txtToDate" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>
        站點:</td>
    <td>
        <div class="style1" style="background:#FFF; width:180px;">
        <asp:CheckBoxList ID="chkWOR" runat="server" style="font-size: 11pt;">
        </asp:CheckBoxList></div>
    </td>
    <td>
        來源批號形態:</td>
    <td>
        <div class="style1" style="background:#FFF;  width:350px;">
        <table>
        <tr>
        <td>
        <asp:CheckBox ID="CheckBox1" runat="server" onClick="javascript:Check_Uncheck_All(this);"  Text="全選" Font-Size="Small" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:CheckBox ID="CheckBox2" runat="server" Text="反選" Font-Size="Small" />
        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Red"
                        Height="16px" Width="120px">(不包含)</asp:Label></td></tr>
        <tr>
        <td>
        <asp:CheckBoxList ID="chkML" runat="server" style="font-size: 11pt" >
        </asp:CheckBoxList>
        </td>
        </tr>
        </table>
        </div>
    </td>
    <td>等級:</td>
    <td><div class="style1" style="background:#FFF; width:180px;">
        <asp:CheckBoxList ID="chkMG" runat="server" style="font-size: 11pt">
        </asp:CheckBoxList></div> </td>
    </tr>
    <tr>
    <td colspan=2>
        <asp:Label ID="Label2" runat="server" Text="查詢近一個星期" ForeColor="red"></asp:Label>
    </td>
    </tr>
    <tr>
    <td colspan="5" >
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btQuery" runat="server" Text="查詢" onclick="Query_Click" /></td>
    <td align="right">
        <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Red"
                        Height="16px" Width="120px">◆Excel導出</asp:Label>
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/xls.jpg" OnClick="ImageButton1_Click"/></td>   
    </tr>
    </table>
    </div>
    <div style="height:300px; width:800px;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="True">
        <Columns>
        <asp:TemplateField HeaderText="序号">
          <ItemTemplate>
            <asp:Literal ID="lit" runat="server" Text="<%#Container.DataItemIndex+1%>" />
          </ItemTemplate>
        </asp:TemplateField>
            <asp:BoundField DataField="CWORKCTR" HeaderText="CWORKCTR" ReadOnly="True" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="TIMES" HeaderText="TIMES" ReadOnly="True" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="FRCPRODID" HeaderText="FRCPRODID" ReadOnly="True" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="FRWONBR" HeaderText="FRWONBR" ReadOnly="True" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="NUMBER" HeaderText="NUMBER" ReadOnly="True" ItemStyle-Width="100px"/> 
            <asp:BoundField DataField="M1PRODID" HeaderText="M1PRODID" ReadOnly="True" ItemStyle-Width="100px"/>
            
        </Columns>
        <RowStyle Font-Names="Arial" HorizontalAlign="Center" BackColor="#FFFFFF"  />
        <HeaderStyle Font-Names="Arial" HorizontalAlign="Center" BackColor="#507CD1" />
        <PagerTemplate>
                当前第:
                
                <asp:Label ID="LabelCurrentPage" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageIndex + 1 %>"></asp:Label>
                页/共:
                
                <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                页
                 
                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                    Visible='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>首页</asp:LinkButton>
                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                    CommandName="Page" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>'>上一页</asp:LinkButton>
               
                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                    Visible='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>下一页</asp:LinkButton>
                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                    Visible='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>尾页</asp:LinkButton>
                转到第
                <asp:TextBox ID="txtNewPageIndex" runat="server" Width="20px" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />页
                
                <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-2"
                    CommandName="Page" Text="GO" />
            </PagerTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
