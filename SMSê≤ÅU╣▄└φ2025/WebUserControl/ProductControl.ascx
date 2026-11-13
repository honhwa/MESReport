<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductControl.ascx.cs" Inherits="WebUserControl_ProductControl" %>
<style type="text/css">
    .style11
    {
        width: 100%;
        height: 100%;
    }
    .style12
    {
        width: 100%;
        height: 100%;
    }
    .style13
    {
        width: 15px;
    }
</style>
<script type="text/javascript">
    function JsProductDownClick() { document.getElementById('<%=ibProductDown.ClientID %>').click(); }
    function JsProductUpClick() { document.getElementById('<%=ibProductUp.ClientID %>').click(); }
</script>
<asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/OptionsProduct.jpg" 
    Height="549px" HorizontalAlign="Center" Width="198px" BorderStyle="None">
    <asp:Panel ID="pTop" runat="server" BorderStyle="None" Height="18px">
    </asp:Panel>
    <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif" 
        Font-Size="10pt">Product:</asp:Label>
    <asp:Panel ID="Panel2" runat="server" BorderStyle="None" Height="10px">
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" BorderStyle="None" Height="139px" 
        BorderWidth="0px">
        <table class="style12" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="style13">
                    &nbsp;</td>
                <td align="left">
                    <asp:Label ID="lblSize" runat="server" Font-Names="Microsoft Sans Serif" 
                        Font-Size="10pt">SIZE:</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style13">
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSIZE" runat="server" Width="165px" AutoPostBack="True" 
                        onselectedindexchanged="ddlSIZE_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style13">
                    &nbsp;</td>
                <td align="left">
                    <asp:Label ID="ProjectName" runat="server" Font-Names="Microsoft Sans Serif" 
                        Font-Size="10pt">ProjectName:</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    &nbsp;</td>
                <td align="left">
                    <asp:DropDownList ID="ddlProjectName" runat="server" Width="165px" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ddlProjectName_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pTop0" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbFromProduct" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px" OnDblClick="JsProductDownClick()"
            Width="177px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Height="48px">
        <table class="style11" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center">
                    <asp:ImageButton ID="ibProductDown" runat="server" Height="43px" 
                        ImageUrl="~/Images/DownButtonColor.jpg"
                        Width="43px" onclick="ibProductDown_Click" />
                </td>
                <td align="center">
                    <asp:ImageButton ID="ibProductUp" runat="server" Height="43px" 
                        ImageUrl="~/Images/UpButtonColor.jpg" 
                        Width="43px" onclick="ibProductUp_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" BorderStyle="None" Height="140px">
        <asp:ListBox ID="lbToProduct" runat="server" 
            Font-Names="Microsoft Sans Serif" Font-Size="10pt" Height="140px"  OnDblClick="JsProductUpClick()"
            Width="180px" SelectionMode="Multiple">
        </asp:ListBox>
    </asp:Panel>
</asp:Panel>
<p>
    <asp:TextBox ID="tbPlant" runat="server" Visible="False" Width="39px"></asp:TextBox>
</p>

