<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubLineUnit.ascx.cs" Inherits="ESM.EQCUserControl.SubLineUnit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<div style="border-right: black 1px solid; border-top: black 1px solid; left: 0px;
    border-left: black 1px solid; width: 1px; border-bottom: black 1px solid; position: absolute;
    top: 8px; height: 1px" >
    <asp:LinkButton ID="Button1" runat="server" BackColor="White" Height="80px" OnClick="Button1_Click"
        Width="67px"></asp:LinkButton></div>
<div style="left: 6px; width: 30px; position: absolute; top: 24px; height: 1px">
    <asp:Label ID="lbID" runat="server" Font-Size="X-Small" Width="1px" Height="1px"></asp:Label></div>

<script language="javascript" type="text/javascript">
function DIV1_onclick()
{
    var op;
//    op = EQCUserControl_SubLineUnit.SubLineID().value;
//    alert(op);
//    op='';
//    op = frameElement;
////    op = document.getElementById("SubLineUnitRough1_lbID").innerHTML;
//    alert(op);

////    window.open("WebEQSM.aspx?userid=" +op+" ",null,'toolbar=no,scrollbars=yes,width=1024,height=768,resizable=no');
//    window.open("WebEQSM.aspx?userid=" +op+" ");
}
function DIV1_onclick() {

}

</script>

<div style="left: 6px; width: 40px; position: absolute; top: 64px; height: 8px">
    <asp:Label ID="lbKT" runat="server" Font-Size="X-Small" Width="1px" Height="5px"></asp:Label></div>
&nbsp;
