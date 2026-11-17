<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RPT_EIS.aspx.cs" Inherits="Material_RPT_EIS" %>

<%@ Register Src="~/WebUserControl/DateControl.ascx" TagName="DateControl" TagPrefix="Date" %>
<%@ Reference Control="~/WebUserControl/DateControl.ascx" %>

<%@ Register Src="~/WebUserControl/PlantSelectControl.ascx" TagName="PlantSelectControl" TagPrefix="PlantSelect" %>
<%@ Reference Control="~/WebUserControl/PlantSelectControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderControl.ascx" TagName="WorkOrderControl" TagPrefix="WorkOrder" %>
<%@ Reference Control="~/WebUserControl/WorkOrderControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderTypeControl.ascx" TagName="WorkOrderTypeControl" TagPrefix="WorkOrderType" %>
<%@ Reference Control="~/WebUserControl/WorkOrderTypeControl.ascx" %>

<%@ Register Src="~/WebUserControl/ProductControl.ascx" TagName="ProductControl" TagPrefix="Product" %>
<%@ Reference Control="~/WebUserControl/ProductControl.ascx" %>

<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>報廢率數據查詢</title>
        <style type="text/css">
        html,body{width:100%;height:100%;margin:0 0 0 0;overflow:auto;}      /*頁面Panel, Div等自動調整高度需要加此代碼*/
        </style>
        <script type="text/javascript">
            function scroll() {
                //alert(typeof document.getElementById('xx').style.height);
                document.getElementById('PRight').style.width = document.documentElement.clientWidth - 230 + "px";
                //document.getElementById('PRight').style.height = document.documentElement.clientHeight - 10 + "px";
                document.getElementById('PLeft').style.height = document.documentElement.clientHeight - 10 + "px";
            }
            window.onload = scroll;
            window.onresize = scroll;
        </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <table id="Table1" align="center"  runat="server" style="height:99%; width:100%;">
            <tr>
                <td style="height:50%; width:220px;" valign="top">
                    <asp:Panel ID="PLeft" runat="server" Height="100%" Width="220px" ScrollBars="Vertical">
                        <table>
                            <tr>
                                <td align="center" style="height:10px;">條件查詢</td>
                            </tr>
                            <tr>
                                <td>
                                    <Date:DateControl ID="Date" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <PlantSelect:PlantSelectControl ID="PlantSelect" runat="server" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                     <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Images/ChooseBox.jpg" Height="328px" HorizontalAlign="Center" Width="195px" BorderStyle="None">
                                        <asp:Panel ID="pTop" runat="server" BorderStyle="None" Height="45px">
                                            <asp:Panel ID="Panel2" runat="server" BorderStyle="None" Height="20px"></asp:Panel>
                                            <asp:Label ID="lblTitle" runat="server" Font-Names="Microsoft Sans Serif"  Font-Size="10pt">顯示欄位選擇</asp:Label>
                                        </asp:Panel>
                                        <asp:Panel ID="pMid" runat="server" style="margin:0 auto;" BackColor="White" BorderStyle="None" 
                                            Width="177px" Height="243px" HorizontalAlign="Left" ScrollBars="Auto">
                                            &nbsp;&nbsp;<asp:CheckBox ID="REPORTDATE" Text="日期" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="SHOPCD" Text="SHOPCD" runat="server" 
                                                Font-Names="Microsoft Sans Serif" Font-Size="10pt" Checked="True" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="PROJECT_NAME" Text="PROJECT_NAME" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="PRODGROUP" Text="客戶名稱" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="SIZEID" Text="尺寸" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="SHOP_CODE" Text="料號段別" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="WONBR" Text="工單" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="WOTYPE" Text="工單類型" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="WORKCTR" Text="站點" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="DEFECTCODE" Text="不良代碼" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="SOURCE_FAB" Text="來源廠別" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                            &nbsp;&nbsp;<asp:CheckBox ID="ScatteredMaterial" Text="零散物料標識" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" /><br />
                                        </asp:Panel>
                                        <asp:Panel ID="pDown" runat="server" BorderStyle="None" Height="40px"></asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <WorkOrder:WorkOrderControl ID="WorkOrder" runat="server" /></td>
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
                            <tr><td align="center" style="font-size:10px; font-family:Microsoft Sans Serif">超1萬筆數據會自動匯出EXCEL</td></tr>
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
                                    <asp:HyperLink ID="hlHelp" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS.htm" Target="_blank">報表說明</asp:HyperLink>
                                    <asp:Label id="lblOwner" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue">報表Owner:楊靈</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td><asp:HyperLink ID="hlProdInfo" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS_BRM_Query.aspx?Name=PI" Target="_blank" Visible="true">產品信息查詢</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlProdBom" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS_BRM_Query.aspx?Name=BOM" Target="_blank" Visible="true">產品BOM查詢</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlDFRATE" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS_BRM_Query.aspx?Name=DFRATE" Target="_blank" Visible="true">前廠責不良代碼查詢</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlSF" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS_BRM_Query.aspx?Name=SF" Target="_blank" Visible="true">產品對應前廠別查詢</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlPMSP" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/RPT_EIS_BRM_Query.aspx?Name=PMSP" Target="_blank" Visible="true">產品對應唯一PANEL料號查詢</asp:HyperLink></td></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td></td></tr>
                                        <tr><td></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td align="left" valign="top" style="width:100%; height:100%">
                    <asp:Panel ID="PRight" runat="server" Width="100%" Height="100%" ScrollBars="Auto" Font-Size="10pt" Font-Names="Microsoft Sans Serif" >
                        <asp:GridView ID="gvData" runat="server" Font-Size="10pt"  Width="1200px"
                            AutoGenerateColumns="False" onrowdatabound="gvData_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="REPORTDATE" HeaderText="日期"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="FABID" HeaderText="廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="SHOPCD" HeaderText="SHOPCD"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="PRODID" HeaderText="產品號碼"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="TP_PART_NUM" HeaderText="TP產品13NC"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="FP_PART_NUM" HeaderText="FP產品13NC"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="PROJECT_NAME"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="PRODGROUP" HeaderText="客戶名稱"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="SIZEID" HeaderText="尺寸"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="SHOP_CODE" HeaderText="料號段別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="WONBR" HeaderText="工單"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="WOTYPE" HeaderText="工單類型"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="WORKCTR" HeaderText="站點"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="DEFECTCODE" HeaderText="不良代碼"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="DEFECTDESC" HeaderText="不良代碼描述"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_PART" HeaderText="COMP_PART"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_M_GROUP" HeaderText="COMP_M_GROUP"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="OUTPUTQTY" HeaderText="產出數量" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="SCRAPQTY" HeaderText="報廢數量" DataFormatString="{0:N0}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="PRE_SCRAPQTY" HeaderText="前廠責報廢數量" DataFormatString="{0:N2}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="SOURCE_FAB" HeaderText="來源廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="ScatteredMaterial" HeaderText="零散物料標識"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                            </Columns>
                            <HeaderStyle BackColor="YellowGreen" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="tbReportName" runat="server" Visible="False" Width="60px"></asp:TextBox>
        <asp:TextBox ID="tbNTName" runat="server" Visible="False"></asp:TextBox>
    </form>
</body>
</html>
