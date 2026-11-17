<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRateSRQuery.aspx.cs" Inherits="Material_ScrapRateSRQuery" %>

<%@ Register Src="~/WebUserControl/DateControl.ascx" TagName="DateControl" TagPrefix="Date2" %>
<%@ Reference Control="~/WebUserControl/DateControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderControl.ascx" TagName="WorkOrderControl" TagPrefix="WorkOrder" %>
<%@ Reference Control="~/WebUserControl/WorkOrderControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderTypeControl.ascx" TagName="WorkOrderTypeControl" TagPrefix="WorkOrderType" %>
<%@ Reference Control="~/WebUserControl/WorkOrderTypeControl.ascx" %>

<%@ Register Src="~/WebUserControl/ProductControl.ascx" TagName="ProductControl" TagPrefix="Product" %>
<%@ Reference Control="~/WebUserControl/ProductControl.ascx" %>

<%@ Register Src="~/WebUserControl/RespShopControl.ascx" TagName="RespShopControl" TagPrefix="RespShop" %>
<%@ Reference Control="~/WebUserControl/RespShopControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SR報表-區間明細查詢</title>
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
                                    <RespShop:RespShopControl ID="RespShop" runat="server" />
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
                                        NavigateUrl="~/Material/ScrapRateSRQuery.htm" Target="_blank" >報表說明</asp:HyperLink>
                                    <asp:Label id="lblOwner" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue">報表Owner:楊靈</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td><asp:HyperLink ID="hlCost" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/ScrapRateCost.aspx" Target="_blank" Visible="false">料件單價設定</asp:HyperLink></td></tr>
                                    </table>
                                </td>
                            </tr>
                            
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td><asp:HyperLink ID="hlLimitCost" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Limit.aspx?UGC=Cost" Target="_blank" Visible="false">料件單價設定權限開通</asp:HyperLink></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td align="left" valign="top" style="width:100%; height:100%">
                    <asp:Panel ID="PRight" runat="server" Width="100%" Height="100%" ScrollBars="Auto" Font-Size="10pt" Font-Names="Microsoft Sans Serif" >
                        <asp:GridView ID="gvData" runat="server" Font-Size="10pt"  Width="950px"
                            AutoGenerateColumns="False" 
                            DataKeyNames="FABID,PRODGROUP,PROJECT_NAME,PRODID" 
                            onrowcreated="gvData_RowCreated" >
                            <Columns>
                                <asp:BoundField DataField="FABID" HeaderText="廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px" VerticalAlign="Top" /></asp:BoundField>
                                <asp:BoundField DataField="PRODGROUP" HeaderText="客戶名稱"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"  VerticalAlign="Top" /></asp:BoundField>
                                 <asp:BoundField DataField="SIZEID" HeaderText="尺寸"><HeaderStyle Wrap="False" /><ItemStyle  Width="120px" Wrap="False" /></asp:BoundField>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="Project Name"><HeaderStyle Wrap="False" /><ItemStyle  Width="120px" Wrap="False" /></asp:BoundField>
                                <asp:BoundField DataField="PRODID" HeaderText="Module Name"><HeaderStyle Wrap="False" /><ItemStyle Width="120px"  Wrap="False" /></asp:BoundField>
                                <asp:BoundField DataField="OutQty" HeaderText="Output Qty(k)" DataFormatString="{0:N0}"><HeaderStyle Width="100px"  Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="CELL2OutQty" HeaderText="CELL2Output Qty(k)" DataFormatString="{0:N0}"><HeaderStyle Width="100px"  Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="MODULEOutQty" HeaderText="MODULEOutput Qty(k)" DataFormatString="{0:N0}"><HeaderStyle Width="100px"  Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="Cell2Rate" HeaderText="Cell2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="80px"  HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="MODULERate" HeaderText="MODULE" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="80px"  HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="TotalRate" HeaderText="TTL" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="80px"  HorizontalAlign="Right" /></asp:BoundField> 
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
