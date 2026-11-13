<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRate.aspx.cs" Inherits="Material_ScrapRate" %>

<%@ Register Src="~/WebUserControl/PlantControl.ascx" TagName="PlantControl" TagPrefix="Plant" %>
<%@ Reference Control="~/WebUserControl/PlantControl.ascx" %>

<%@ Register Src="~/WebUserControl/WorkOrderTypeControl.ascx" TagName="WorkOrderTypeControl" TagPrefix="WorkOrderType" %>
<%@ Reference Control="~/WebUserControl/WorkOrderTypeControl.ascx" %>

<%@ Register Src="~/WebUserControl/ProductControl.ascx" TagName="ProductControl" TagPrefix="Product" %>
<%@ Reference Control="~/WebUserControl/ProductControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        <table id="Table1" align="center"  runat="server" style="height:99%; width:100%">
            <tr>
                <td style="height:100%; width:220px;" valign="top">
                    <asp:Panel ID="PLeft" runat="server" Height="100%" Width="220px" ScrollBars="Vertical">
                        <table>
                            <tr>
                                <td align="center" style="height:10px;">條件查詢</td>
                            </tr>
                            <%--<tr>
                                <td><asp:RadioButtonList ID="rblMergeModule" runat="server" AutoPostBack="True" 
                                        BorderStyle="Solid" CellPadding="0" CellSpacing="0" Font-Bold="False" 
                                        Font-Size="10pt" 
                                        onselectedindexchanged="rblMergeModule_SelectedIndexChanged" Width="195px">
                                    <asp:ListItem Value="N" Selected="True">LCM+LAM不合併</asp:ListItem>
                                    <asp:ListItem Value="Y">LCM+LAM合併</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <Plant:PlantControl ID="Plant" runat="server" />
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
                                <td><asp:CheckBox ID="cbShowPartID" runat="server" Font-Names="Microsoft Sans Serif" 
                                        Font-Size="10pt" Text="顯示TP料號&amp;物料料號" />
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
                                    <asp:HyperLink ID="hlHelp" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/ScrapRate.htm" Target=“_blank”>報表說明</asp:HyperLink>
                                    <asp:Label id="lblOwner" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue">報表Owner:楊靈</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td><asp:HyperLink ID="hlCost" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/ScrapRateCost.aspx" Target="_blank" Visible="false">料件單價設定</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlPEPlan" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Material/ScrapRatePlan.aspx?PlanType=PEPlan" Target="_blank" Visible="false">PE目標值設定</asp:HyperLink></td></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <table>
                                        <tr><td><asp:HyperLink ID="hlLimitCost" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Limit.aspx?UGC=Cost" Target="_blank" Visible="false">料件單價設定權限開通</asp:HyperLink></td></tr>
                                        <tr><td><asp:HyperLink ID="hlLimitPEPlan" runat="server"  Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue" Font-Bold="true" NavigateUrl="~/Limit.aspx?UGC=PEPlan" Target="_blank" Visible="false">PE目標值設定權限開通</asp:HyperLink></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td align="left" valign="top" style="width:100%; height:100%">
                    <asp:Panel ID="PRight" runat="server" Width="100%" Height="100%" ScrollBars="Auto" Font-Size="10pt" Font-Names="Microsoft Sans Serif" >
                        <asp:GridView ID="gvData" runat="server" Font-Size="10pt"  Width="1200px"
                            AutoGenerateColumns="False" 
                            DataKeyNames="FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP" 
                            onrowcreated="gvData_RowCreated" >
                            <Columns>
                                <asp:BoundField DataField="FABID" HeaderText="廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" /></asp:BoundField>
                                <asp:BoundField DataField="PLANTID" HeaderText="SHOPCD"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" /></asp:BoundField>
                                <asp:BoundField DataField="PRODGROUP" HeaderText="客戶名稱"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" /></asp:BoundField>
                                 <asp:BoundField DataField="SIZEID" HeaderText="尺寸"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" /></asp:BoundField>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="ProjectName"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                                <asp:BoundField DataField="TP_PART_NUM" HeaderText="TP"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_M_GROUP" HeaderText="物料類別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_PART" HeaderText="物料料號"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" /></asp:BoundField>
                                <asp:BoundField DataField="M5" HeaderText="M5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M4" HeaderText="M4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M3" HeaderText="M3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M2" HeaderText="M2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M1" HeaderText="M1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M0" HeaderText="MTD" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="W4" HeaderText="W4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W3" HeaderText="W3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W2" HeaderText="W2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W1" HeaderText="W1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D7" HeaderText="D7" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D6" HeaderText="D6" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D5" HeaderText="D5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D4" HeaderText="D4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D3" HeaderText="D3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D2" HeaderText="D2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D1" HeaderText="D1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                            </Columns>
                            <HeaderStyle BackColor="YellowGreen" />
                        </asp:GridView>
                        <asp:GridView ID="gvDataCost" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="FABID,PLANTID,PRODGROUP,SIZEID,PROJECT_NAME,COMP_M_GROUP" Font-Size="10pt" 
                            Width="2100px" onrowcreated="gvDataCost_RowCreated" >
                            <Columns>
                                <asp:BoundField DataField="FABID" HeaderText="廠別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" Width="50px" /></asp:BoundField>
                                <asp:BoundField DataField="PLANTID" HeaderText="SHOPCD"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" Width="50px" /></asp:BoundField>
                                <asp:BoundField DataField="PRODGROUP" HeaderText="客戶名稱"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" Width="120px" /></asp:BoundField>
                                <asp:BoundField DataField="SIZEID" HeaderText="尺寸"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Top" Width="120px" /></asp:BoundField>
                                <asp:BoundField DataField="PROJECT_NAME" HeaderText="ProjectName"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="120px" /></asp:BoundField>
                                <asp:BoundField DataField="COMP_M_GROUP" HeaderText="物料類別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="80px" HorizontalAlign="Center" /></asp:BoundField>
                                <asp:BoundField DataField="M5" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="M5Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M4" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="M4Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M3" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="M3Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M2" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="M2Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M1" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="M1Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="M0" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField><asp:BoundField DataField="M0Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField>
                                <asp:BoundField DataField="W4" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="W4Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W3" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="W3Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W2" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="W2Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="W1" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="W1Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D7" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D7Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D6" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D6Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D5" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D5Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D4" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D4Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D3" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D3Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D2" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D2Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
                                <asp:BoundField DataField="D1" HeaderText="SR" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> <asp:BoundField DataField="D1Rate" HeaderText="%" DataFormatString="{0:P}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" Width="50px" HorizontalAlign="Right" /></asp:BoundField> 
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
