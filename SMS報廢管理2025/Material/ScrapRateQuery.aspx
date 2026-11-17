<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRateQuery.aspx.cs" Inherits="Material_ScrapRateQuery" %>

<%@ Register Src="~/WebUserControl/PlantControl.ascx" TagName="PlantControl" TagPrefix="Plant" %>
<%@ Reference Control="~/WebUserControl/PlantControl.ascx" %>

<%@ Register Src="~/WebUserControl/DateControl.ascx" TagName="DateControl" TagPrefix="Date2" %>
<%@ Reference Control="~/WebUserControl/DateControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderControl.ascx" TagName="WorkOrderControl" TagPrefix="WorkOrder" %>
<%@ Reference Control="~/WebUserControl/WorkOrderControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderTypeControl.ascx" TagName="WorkOrderTypeControl" TagPrefix="WorkOrderType" %>
<%@ Reference Control="~/WebUserControl/WorkOrderTypeControl.ascx" %>

<%@ Register Src="~/WebUserControl/ProductControl.ascx" TagName="ProductControl" TagPrefix="Product" %>
<%@ Reference Control="~/WebUserControl/ProductControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部材超耗查詢</title>
    <style type="text/css">
        html,body{width:100%;height:100%;margin:0;overflow:auto;}      /*頁面Panel, Div等自動調整高度需要加此代碼*/
        </style>
        <script type="text/javascript">
            function scroll() {
                //alert(typeof document.getElementById('xx').style.height);
                document.getElementById('PRight').style.width = document.documentElement.clientWidth - 230 + "px";
                document.getElementById('PLeft').style.height = document.documentElement.clientHeight - 10 + "px";
            }
            window.onload = scroll;
            window.onresize = scroll;
        </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <table id="Table1" align="center"  runat="server" style="height:99%; width:100%">
            <tr>
                <td style="height:100%; width:220px;" valign="top">
                    <asp:Panel ID="PLeft" runat="server" Height="100%" Width="220px" ScrollBars="Vertical">
                        <table>
                            <tr>
                                <td align="center" style="height:10px;">條件查詢</td>
                            </tr>
                            <tr>
                                <td>
                                    <Date2:DateControl ID="Date" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <Plant:PlantControl ID="Plant" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <WorkOrder:WorkOrderControl ID="WorkOrder" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <WorkOrderType:WorkOrderTypeControl ID="WorkOrderType" runat="server" /></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <Product:ProductControl ID="Product" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnQuery" runat="server" BackColor="#4F81BC" Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="16pt" ForeColor="White" Text="Query" onclick="btnQuery_Click" />
                                    <asp:Button ID="btnExcel" runat="server" BackColor="#4F81BC" Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="16pt" ForeColor="White" Text="Excel" onclick="btnExcel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btClear" runat="server" BackColor="#4F81BC" Font-Bold="True" 
                                        Font-Names="Microsoft Sans Serif" Font-Size="16pt" ForeColor="White" 
                                        Text="Clear" onclick="btClear_Click"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:50px;" align="center" valign="top">
                                    <asp:Label ID="lblRunTime" runat="server" Font-Bold="False" 
                                        Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  align="center">
                                    <asp:HyperLink ID="hlHelp" runat="server"  Font-Names="Microsoft Sans Serif" 
                                        Font-Size="10pt" ForeColor="Blue" Font-Bold="True" 
                                        NavigateUrl="~/Material/ScrapRateQuery.htm" Target=“_blank”>報表說明</asp:HyperLink>
                                    <asp:Label id="lblOwner" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue">報表Owner:楊靈</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  align="center">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td align="left" valign="top" style="width:100%; height:100%">
                    <asp:Panel ID="PRight" runat="server" Width="100%" Height="100%" ScrollBars="Auto" Font-Size="10pt" Font-Names="Microsoft Sans Serif" >
                        <asp:GridView ID="gvData" runat="server" Font-Size="10pt" Width="1050px"
                            AutoGenerateColumns="False" 
                            DataKeyNames="SHOPCD,PRODGROUP,PROJECT_NAME,COMP_M_GROUP" >
                            <Columns>
                                <asp:BoundField DataField="FABID" HeaderText="廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="30px" /></asp:BoundField>
                                <asp:BoundField DataField="SHOPCD" HeaderText="段別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top" Width="50px" /></asp:BoundField>
                                <asp:BoundField DataField="PRODGROUP" HeaderText="客戶名稱"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px" /> </asp:BoundField>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="Project Name"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px" /></asp:BoundField>
                                <asp:BoundField DataField="PRODID" HeaderText="Module Name"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px" /></asp:BoundField>
                                <asp:BoundField DataField="TP_PART_NUM" HeaderText="TP"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px" /></asp:BoundField>
                                <asp:BoundField DataField="FP_PART_NUM" HeaderText="FP"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_M_GROUP" HeaderText="物料類別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_PART" HeaderText="物料料號"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="110px" /></asp:BoundField>
                                <asp:BoundField DataField="SCRAP_QTY" HeaderText="Scrap Qty" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" Width="80px" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="OUT_QTY" HeaderText="Output Qty" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" Width="80px" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="ScrapRate" HeaderText="Scrap Rate" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" Width="80px" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
                            </Columns>
                            <HeaderStyle BackColor="YellowGreen" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="tbNTName" runat="server" Visible="False"></asp:TextBox>
    </form>
</body>
</html>
