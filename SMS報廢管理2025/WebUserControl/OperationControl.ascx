<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OperationControl.ascx.cs" Inherits="WebUserControl_OperationControl" %>
<style type="text/css">
    .style11
    {
        width: 100%;
        height: 100%;
    }
</style>
<script type="text/javascript">
    function JsOperationDownClick() { document.getElementById('<%=this.ibOperationDown.ClientID %>').click(); }
    function JsOperationUpClick() { document.getElementById('<%=this.ibOperationUp.ClientID %>').click(); }
</script>
<asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/ListBoxDownUp.jpg" 
    Height="412px" HorizontalAlign="Center" Width="198px" BorderStyle="None">
    <asp:Panel ID="pTop" runat="server" BorderStyle="None" Height="18px">
    </asp:Panel>
    <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
        Font-Size="10pt">Operation</asp:Label>
    <asp:Panel ID="Panel2" runat="server" BorderStyle="None" Height="10px">
    </asp:Panel>
    <asp:Panel ID="pTop0" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbFromValue" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px" OnDblClick="JsOperationDownClick()"
            Width="177px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Height="48px">
        <table class="style11" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <asp:ImageButton ID="ibOperationDown" runat="server" Height="43px" 
                        ImageUrl="~/Images/DownButtonColor.jpg"
                        Width="43px" onclick="ibOperationDown_Click" />
                </td>
                <td align="center">
                    <asp:ImageButton ID="ibOperationUp" runat="server" Height="43px" 
                        ImageUrl="~/Images/UpButtonColor.jpg" 
                        Width="43px" onclick="ibOperationUp_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbToValue" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px"  OnDblClick="JsOperationUpClick()"
            Width="180px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
</asp:Panel>
